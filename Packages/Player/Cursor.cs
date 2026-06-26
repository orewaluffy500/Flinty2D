using System.Numerics;
using Flinty.GameMath;
using Flinty.GameSystem;
using Flinty.Globals;
using Flinty.World;
using Raylib_cs;

namespace Flinty.Player
{
    public class Cursor(PlayerEntity player, Terrain terrain) : Entity
    {
        public PlayerEntity Player { get; private set; } = player;
        public Terrain Terrain { get; private set; } = terrain;

        public override void Draw()
        {
            EngineRenderer.Line(
                Pos.Mul(Preferences.TILE_SIZE).Change(Preferences.TILE_SIZE / 2),
                Player.Pos.Mul(Preferences.TILE_SIZE).Change(Preferences.TILE_SIZE / 2),
                new(230, 230, 240, 80)
            );

            EngineRenderer.Rectangle(nti
                new(Pos.Mul(Preferences.TILE_SIZE), Size.TileSize()),
                new(100, 255, 100, 140)
            );
        }

        public override void Update(float deltaTime)
        {
            UpdateAndClampPosition();

            if (MouseMap.ButtonDown("RMB"))
            {
                Terrain.Place(Pos.X, Pos.Y, Player.Inventory.GetSelection());
            }

            else if (MouseMap.ButtonDown("LMB"))
            {
                Terrain.Break(Pos.X, Pos.Y);
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
            int minX = Player.Pos.X - 5;
            int maxX = Player.Pos.X + 5;

            int minY = Player.Pos.Y - 5;
            int maxY = Player.Pos.Y + 5;

            Pos.X = Math.Clamp(mouseX, minX, maxX);
            Pos.Y = Math.Clamp(mouseY, minY, maxY);
        }
    }
}