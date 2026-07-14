using System.Numerics;
using Flinty.GameMath;
using Flinty.GameSystem;
using Flinty.Globals;
using Flinty.World;
using Raylib_cs;

namespace Flinty.Player
{
    public class Cursor(PlayerNode player, Terrain terrain) : BaseNode
    {
        public PlayerNode Player { get; private set; } = player;
        public Terrain Terrain { get; private set; } = terrain;

        public override void Draw()
        {
            EngineRenderer.Line(
                Coords.Mul(Preferences.TILE_SIZE).Change(Preferences.TILE_SIZE / 2),
                Player.Coords.Mul(Preferences.TILE_SIZE).Change(Preferences.TILE_SIZE / 2),
                new(230, 230, 240, 80)
            );

            EngineRenderer.Rectangle(
                new(Coords.Mul(Preferences.TILE_SIZE), Area.TileSize()),
                new(100, 255, 100, 140)
            );
        }

        public override void Update(float deltaTime)
        {
            UpdateAndClampPosition();

            if (MouseMap.ButtonDown("RMB"))
            {
                Terrain.Place(Coords.X, Coords.Y, Player.Inventory.GetSelection());
            }

            else if (MouseMap.ButtonDown("LMB"))
            {
                Terrain.Break(Coords.X, Coords.Y);
            }
        }

        private void UpdateAndClampPosition()
        {
            Vector2 worldPos = Raylib.GetScreenToWorld2D(
                Raylib.GetMousePosition(),
                Player.Camera
            );

            int mouseX = (int)MathF.Floor(worldPos.X / Preferences.TILE_SIZE);
            int mouseY = (int)MathF.Floor(worldPos.Y / Preferences.TILE_SIZE);
            ClampCursor(mouseX, mouseY);
        }

        public void ClampCursor(int mouseX, int mouseY)
        {
            int minX = Player.Coords.X - 5;
            int maxX = Player.Coords.X + 5;

            int minY = Player.Coords.Y - 5;
            int maxY = Player.Coords.Y + 5;

            Coords.X = Math.Clamp(mouseX, minX, maxX);
            Coords.Y = Math.Clamp(mouseY, minY, maxY);
        }
    }
}