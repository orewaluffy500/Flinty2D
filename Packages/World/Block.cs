
using Flinty.Assets;
using Flinty.GameMath;
using Flinty.GameSystem;
using Flinty.Globals;
using Raylib_cs;
using Area = Flinty.GameMath.Area;

namespace Flinty.World
{
    public class TileNode : BaseNode
    {
        public string BlockId { get; set; }
        public BlockEntry? BlockRegistryEntry;
        public Terrain Terrain { get; }

        public bool CanCollide { get; set; } = true;
        private Metadata? _metadata;
        public Metadata Metadata
        {
            get => _metadata ??= new Metadata();
            set => _metadata = value;
        }
        public static readonly Area TileSize = Area.TileSize();

        public TileNode(int x, int y, string type, Terrain terrain)
        {
            Terrain = terrain;
            Coords = new Coordinates(x, y);
            BlockId = type;
            BlockRegistryEntry = BlockRegistry.GetBlockEntry(BlockId);
            CanCollide = BlockRegistryEntry?.CanCollide ?? true;
        }


        public override void Draw()
        {

            // Validate color
            Color finalColor = BlockRegistryEntry?.FallbackColor ?? Color.Black;

            // Get texture
            Texture2D? tex = TextureRegistry.GetTexture(BlockId);


            // Draw
            if (tex == null)
            {
                EngineRenderer.Rectangle(
                    new(Coords.Mul(Preferences.TILE_SIZE), TileSize),
                    finalColor
                );

                return;
            }


            EngineRenderer.Texture(
                (Texture2D)tex, new(Coordinates.Zero(), Area.TileSize()), new(Coords.Mul(Preferences.TILE_SIZE), Area.TileSize()), 0
            );
            
        }

        public override void Tick(int index, Terrain terrain, Engine engine)
        {
            engine.FireBlockEvent("tick", BlockId, index, Coords.X, Coords.Y);
        }
    }
}