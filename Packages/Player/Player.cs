using System.Numerics;
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

            if (KeyMap.KeyDown("CycleBlocks"))
            {
                Inventory.AdvanceSelection();
            }

            if (!Velocity.IsZero())
            {
                StepDelay = Preferences.STEP_DELAY;
            }


            // Handle collision

            if (Velocity.X > 0 && Terrain.IsBlock(Pos.X + 1, Pos.Y)) Velocity.X = 0;
            if (Velocity.X < 0 && Terrain.IsBlock(Pos.X - 1, Pos.Y)) Velocity.X = 0;
            if (Velocity.Y > 0 && Terrain.IsBlock(Pos.X, Pos.Y + 1)) Velocity.Y = 0;
            if (Velocity.Y < 0 && Terrain.IsBlock(Pos.X, Pos.Y - 1)) Velocity.Y = 0;

            Pos.Change(Velocity.X, Velocity.Y, true); // Inline means no new instances
            Velocity.SetZero();
        }

        public override void Draw(EngineRenderer renderer)
        {
            Cursor.Draw(renderer);
            renderer.Rectangle(new(Pos.Mul(Preferences.TILE_SIZE), Size.TileSize()), new(123, 123, 255));
        }



        public void UpdateCamera()
        {
            Camera.Offset = new(Raylib.GetScreenWidth() / 2, Raylib.GetScreenHeight() / 2);
            Camera.Target = Vector2.Lerp(Camera.Target, Pos.Mul(Preferences.TILE_SIZE).ToVector(), 0.1f);
        }
    }
}