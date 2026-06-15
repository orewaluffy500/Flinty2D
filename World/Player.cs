using Flinty.GameMath;
using Flinty.GameSystem;
using Flinty.Globals;

namespace Flinty.World
{
    public class Player(Terrain terrain) : Entity
    {
        public Pos Velocity { get; private set; } = Pos.One();

        public Terrain Terrain { get; } = terrain;

        public float StepDelay { get; private set; } = 0;

        public bool OnGround = false;

        public override void Update(float deltaTime)
        {
            UpdateMovement(deltaTime);
        }

        private void UpdateMovement(float deltaTime)
        {
            if (StepDelay > 0)
            {
                StepDelay -= deltaTime;
                return;
            }


            if (KeyMap.KeyDown("MoveUp") && OnGround)
            {
                Velocity.Y -= 3;
            }

            if (KeyMap.KeyDown("MoveLeft"))
            {
                Velocity.X -= 1;
            }

            if (KeyMap.KeyDown("MoveRight"))
            {
                Velocity.X += 1;
            }

            Velocity.Y += 1; // if 0 then -1, if 1 then 0 then -1
            if (!Velocity.IsZero())
            {
                StepDelay = Preferences.STEP_DELAY;
            }


            // Handle collision
            OnGround = false;


            if (Velocity.X > 0 && Terrain.IsBlock(Pos.X + 1, Pos.Y)) Velocity.X = 0;
            if (Velocity.X < 0 && Terrain.IsBlock(Pos.X - 1, Pos.Y)) Velocity.X = 0;
            if (Velocity.Y < 0 && Terrain.IsBlock(Pos.X, Pos.Y - 1)) Velocity.Y = 0;

            if (Velocity.Y > 0 && Terrain.IsBlock(Pos.X, Pos.Y + 1))
            {
                Velocity.Y = 0;
                OnGround = true;
            }

            Pos.Change(Velocity.X, Velocity.Y, true); // Inline means no new instances
            Velocity.SetZero();
        }

        public override void Draw(EngineRenderer renderer)
        {
            renderer.Rectangle(new(Pos.Mul(Preferences.TILE_SIZE), Size.TileSize()), new(123, 123, 255));
        }
    }
}