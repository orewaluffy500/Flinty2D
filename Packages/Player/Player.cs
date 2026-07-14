using System.Numerics;
using Flinty.Assets;
using Flinty.GameMath;
using Flinty.GameSystem;
using Flinty.Globals;
using Flinty.World;
using Raylib_cs;

namespace Flinty.Player
{
    public class PlayerNode : BaseNode
    {
        public Coordinates Velocity { get; private set; } = Coordinates.Zero();

        public Terrain Terrain { get; }

        public Cursor Cursor { get; }

        public Metadata Metadata { get; }

        public Camera2D Camera;

        public Inventory Inventory;

        public float StepDelay { get; private set; } = 0;

        public PlayerNode(Terrain terrain)
        {
            Terrain = terrain;
            Cursor = new(this, Terrain);
            Camera = new(new(0, 0), Coords.ToVector(), 0, 1);
            Inventory = new(this);
            Metadata = new();
        }

        public override void Update(float deltaTime)
        {
            UpdateMovement(deltaTime);
            HandleBlockCycle();
            HandleMisc();
            UpdateCamera();
            Cursor.Update(deltaTime);
        }

        private void UpdateMovement(float deltaTime)
        {
            if (StepDelay > 0)
            {
                StepDelay -= deltaTime;
                return;
            }

            if (KeyMap.KeyDown("MoveUp"))
            {
                Velocity.Y -= 1;
            }

            if (KeyMap.KeyDown("MoveDown"))
            {
                Velocity.Y += 1;
            }

            if (KeyMap.KeyDown("MoveLeft"))
            {
                Velocity.X -= 1;
            }

            if (KeyMap.KeyDown("MoveRight"))
            {
                Velocity.X += 1;
            }

            if (!Velocity.IsZero())
            {
                StepDelay = Preferences.STEP_DELAY;
                Terrain.Engine.FirePlayerEvent("moved", Coords.X, Coords.Y, Coords.X + Velocity.X, Coords.Y + Velocity.Y);
            }


            // Handle collision

            if (Terrain.IsSolidBlock(Coords.X + Velocity.X, Coords.Y)) Velocity.X = 0;
            if (Terrain.IsSolidBlock(Coords.X, Coords.Y + Velocity.Y)) Velocity.Y = 0;

            Coords.Change(Velocity.X, Velocity.Y, true); // Inline means no new instances
            Velocity.SetZero();
        }

        public void HandleBlockCycle()
        {
            if (KeyMap.KeyPressed("CycleBlocks")) // CycleBlocks is by default an alias for Tab
            {
                var old_selection = Inventory.GetSelection();

                Inventory.AdvanceSelection();
                
                var new_selection = Inventory.GetSelection();

                Terrain.Engine.FirePlayerEvent("reselected", old_selection, new_selection);
            }
        }

        public void HandleMisc()
        {
            if (KeyMap.KeyPressed("G")) // Freeze and unfreeze ticking blocks
            {
                Terrain.TicksFrozen = !Terrain.TicksFrozen;
            }
        }

        public override void DrawHUD()
        {
            var entry = BlockRegistry.GetBlockEntry(Inventory.GetSelection()); // Get the information for the selected block

            var color = entry?.FallbackColor ?? Color.RayWhite; // Use the selected block's color or white by default

            string selecName = Inventory.GetSelection();

            EngineRenderer.TextScale(0, 5, selecName.ToUpper(), color); // Draw selection in upper case
            EngineRenderer.TextScale(0, 8, "[TAB] to cycle.", Color.RayWhite, 20); // Draw how to cycle blocks
            EngineRenderer.TextScale(0, 13, $"Player {Coords.X}, {Coords.Y}", Color.RayWhite, 20); // Player co-ords
            EngineRenderer.TextScale(0, 16, $"Cursor {Cursor.Coords.X}, {Cursor.Coords.Y}", Color.RayWhite, 20); // Cursor co-ords
        }

        public override void Draw()
        {
            Cursor.Draw(); // Draw cursor
            EngineRenderer.Rectangle(new(Coords.Mul(Preferences.TILE_SIZE), Area.TileSize()), new(123, 123, 255)); // Draw player
        }



        public void UpdateCamera()
        {
            Camera.Offset = new(Raylib.GetScreenWidth() / 2, Raylib.GetScreenHeight() / 2); // Get camera offset
            Camera.Target = Vector2.Lerp(Camera.Target, Coords.Mul(Preferences.TILE_SIZE).ToVector(), 0.2f); // Lerp camera to player
        }
    }
}