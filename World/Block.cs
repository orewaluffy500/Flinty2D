
using Flinty.Assets;
using Flinty.GameMath;
using Flinty.GameSystem;
using Flinty.Globals;
using Raylib_cs;
using Size = Flinty.GameMath.Size;

namespace Flinty.World
{
    public class Block : Entity
    {
        public string Type { get; }
        public BlockEntry? BlockEntry;

        public static readonly Size TileSize = Size.TileSize();

        public Block(int x, int y, string type)
        {
            Pos = new Pos(x, y);
            Type = type;
            BlockEntry = BlockRegistry.GetBlockEntry(Type);
        }


        public override void Draw(EngineRenderer renderer)
        {
            // Validate color
            Color finalColor = Color.Blue;

            if (BlockEntry != null)
            {
                finalColor = BlockEntry.FallbackColor;
            }

            // Draw
            renderer.Rectangle(
                new(Pos.Mul(Preferences.TILE_SIZE), TileSize),
                finalColor
            );
        }

        public override void Tick(int index, Terrain terrain)
        {
            
        }
    }
}