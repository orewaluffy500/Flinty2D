using Flinty.GameMath;
using Flinty.GameSystem;
using Flinty.Globals;

namespace Flinty.World
{
    public class Player : Entity
    {
        public Pos Velocity { get; private set; } = Pos.Zero();

        public Terrain Terrain { get; }

        public Cursor Cursor { get; }

        public float StepDelay { get; private set; } = 0;

        public Player(Terrain terrain)
        {
            Terrain = terrain;
            Cursor = new(this, Terrain);
        }

        public override void Update(float deltaTime)
        {
            UpdateMovement(deltaTime);
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
    }
}