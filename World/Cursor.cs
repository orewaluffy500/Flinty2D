using Flinty.GameMath;
using Flinty.GameSystem;
using Flinty.Globals;
using Raylib_cs;

namespace Flinty.World
{
    public class Cursor(Player player, Terrain terrain) : Entity
    {
        public Player Player { get; private set; } = player;
        public Terrain Terrain { get; private set; } = terrain;

        public override void Draw(EngineRenderer renderer)
        {
            renderer.Rectangle(
                new(Pos.Mul(Preferences.TILE_SIZE), Size.TileSize()),
                new(100, 255, 100, 140)
            );
        }

        public override void Update(float deltaTime)
        {
            UpdateAndClampPosition();

            if (MouseMap.ButtonDown("RMB"))
            {
                Terrain.Place(Pos.X, Pos.Y, "soil");
            }

            else if (MouseMap.ButtonDown("LMB"))
            {
                Terrain.Break(Pos.X, Pos.Y);
            }
        }

        private void UpdateAndClampPosition()
        {
            Pos.X = (int)Math.Floor((float)(Raylib.GetMouseX() / Preferences.TILE_SIZE));
            Pos.Y = (int)Math.Floor((float)(Raylib.GetMouseY() / Preferences.TILE_SIZE));

            Pos.X = Math.Clamp(Pos.X, Player.Pos.X - 5, Player.Pos.X + 5);
            Pos.Y = Math.Clamp(Pos.Y, Player.Pos.Y - 5, Player.Pos.Y + 5);
        }
    }
}