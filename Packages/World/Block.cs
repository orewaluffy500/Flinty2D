
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
        public Terrain Terrain { get; }

        public bool CanCollide { get; set; } = true;

        private Metadata? _metadata;
        public Metadata Metadata => _metadata ??= new Metadata(); // lazy loaded metadata for optimization

        public static readonly Size TileSize = Size.TileSize();

        public Block(int x, int y, string type, Terrain terrain)
        {
            Terrain = terrain;
            Pos = new Pos(x, y);
            Type = type;
            BlockEntry = BlockRegistry.GetBlockEntry(Type);

            if (BlockEntry is BlockEntry b)
            {
                CanCollide = b.CanCollide;
            }
        }


        public override void Draw()
        {

            // Validate color
            Color finalColor = Color.Black;

            if (BlockEntry != null)
            {
                finalColor = BlockEntry.FallbackColor;
            }

            // Get texture
            Texture2D? tex = null;

            if (BlockEntry != null)
            {
                tex = TextureRegistry.GetTexture(BlockEntry.TextureId);
            }


            // Draw
            if (tex == null)
            {
                EngineRenderer.Rectangle(
                    new(Pos.Mul(Preferences.TILE_SIZE), TileSize),
                    finalColor
                );

                return;
            }


            EngineRenderer.Texture(
                (Texture2D)tex, new(Pos.Zero(), Size.TileSize()), new(Pos.Mul(Preferences.TILE_SIZE), Size.TileSize()), 0
            );
            
        }

        public override void Tick(int index, Terrain terrain)
        {
        }
    }
}