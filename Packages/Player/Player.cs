using System.Numerics;
using Flinty.Assets;
using Flinty.GameMath;
using Flinty.GameSystem;
using Flinty.Globals;
using Flinty.World;
using Raylib_cs;

namespace Flinty.Player
{
    public class PlayerEntity : Entity
    {
        public Pos Velocity { get; private set; } = Pos.Zero();

        public Terrain Terrain { get; }

        public Cursor Cursor { get; }

        public Metadata Metadata { get; }

        public Camera2D Camera;

        public Inventory Inventory;

        public float StepDelay { get; private set; } = 0;

        public PlayerEntity(Terrain terrain)
        {
            Terrain = terrain;
            Cursor = new(this, Terrain);
            Camera = new(new(0, 0), Pos.ToVector(), 0, 1);
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
            }


            // Handle collision

            if (Terrain.IsSolidBlock(Pos.X + Velocity.X, Pos.Y)) Velocity.X = 0;
            if (Terrain.IsSolidBlock(Pos.X, Pos.Y + Velocity.Y)) Velocity.Y = 0;

            Pos.Change(Velocity.X, Velocity.Y, true); // Inline means no new instances
            Velocity.SetZero();
        }

        public void HandleBlockCycle()
        {
            if (KeyMap.KeyPressed("CycleBlocks")) // CycleBlocks is by default an alias for Tab
            {
                Inventory.AdvanceSelection();
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
            EngineRenderer.TextScale(0, 13, $"Player {Pos.X}, {Pos.Y}", Color.RayWhite, 20); // Player co-ords
            EngineRenderer.TextScale(0, 16, $"Cursor {Cursor.Pos.X}, {Cursor.Pos.Y}", Color.RayWhite, 20); // Cursor co-ords
        }

        public override void Draw()
        {
            Cursor.Draw(); // Draw cursor
            EngineRenderer.Rectangle(new(Pos.Mul(Preferences.TILE_SIZE), Size.TileSize()), new(123, 123, 255)); // Draw player
        }



        public void UpdateCamera()
        {
            Camera.Offset = new(Raylib.GetScreenWidth() / 2, Raylib.GetScreenHeight() / 2); // Get camera offset
            Camera.Target = Vector2.Lerp(Camera.Target, Pos.Mul(Preferences.TILE_SIZE).ToVector(), 0.2f); // Lerp camera to player
        }
    }
}