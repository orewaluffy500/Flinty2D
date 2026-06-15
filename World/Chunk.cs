using Flinty.GameMath;
using Flinty.GameSystem;
using Flinty.Globals;

namespace Flinty.World
{
    public class Chunk(Pos pos)
    {
        public Block?[,] Blocks { set; get; } = new Block[Preferences.CHUNK_SIZE, Preferences.CHUNK_SIZE];
        public Pos Pos { get; } = pos;

        public void SetBlock(int x, int y, Block? block)
        {
            Blocks[x, y] = block;
        }


        public void ClearBlock(int x, int y)
        {
            Blocks[x, y] = null;
        }

        public Block? GetBlock(int x, int y)
        {
            return Blocks[x, y];
        }


        // Packs two 32-bit chunk coordinates into a single 64-bit key for dictionary lookup.
        // X occupies the upper 32 bits; Y is cast to uint to prevent sign-extension in the lower 32 bits.
        public static long GetChunkKey(int x, int y)
        {
            return ((long)x << 32) | (uint)y;
        }
    }


    public class ChunkManager(Terrain t)
    {
        public Terrain Terrain { get; } = t;
        public Dictionary<long, Chunk> Chunks { get; } = [];

        public List<Chunk> VisibleChunks { get; private set; } = [];

        // Returns the chunk at (x, y), creating and registering it on first access ("safe" = never null)
        public Chunk GetChunkSafe(int x, int y)
        {
            long key = Chunk.GetChunkKey(x, y);

            if (!Chunks.TryGetValue(key, out Chunk? value))
            {
                value = new Chunk(new Pos(x, y));
                Chunks[key] = value;
            }

            return value;
        }

        public void RenderAndUpdateChunks()
        {
            UpdateVisibleChunks(Terrain.Player.Pos.X, Terrain.Player.Pos.Y);

            bool ticking = Terrain.Engine.Ticking;
            int index = Terrain.Engine.TickIndex;

            foreach (Chunk chunk in VisibleChunks)
            {
                // Optionally overlay chunk border rectangles for debugging
                if (Preferences.DRAW_CHUNK_DECORS)
                {
                    ChunkHelpers.DrawDecors(chunk, Terrain.Engine.Renderer);
                }

                foreach (Block? block in chunk.Blocks)
                {
                    if (block == null) continue;    // Sparse array — skip empty cells

                    if (ticking)
                    {
                        block.Tick(index, Terrain);
                    }

                    block.Draw(Terrain.Engine.Renderer);
                }
            }
        }

        public void UpdateVisibleChunks(int originX, int originY)
        {
            VisibleChunks.Clear();

            // Convert the world-space block origin to chunk-space coordinates
            Pos originChunkPos = ChunkHelpers.Block2Chunk(originX, originY);

            int drawDistance = Preferences.DRAW_DISTANCE;

            // Collect all chunks within a square radius of [drawDistance] chunks around the origin
            for (int y = originChunkPos.Y - drawDistance; y < originChunkPos.Y + drawDistance; y++)
            {
                for (int x = originChunkPos.X - drawDistance; x < originChunkPos.X + drawDistance; x++)
                {
                    VisibleChunks.Add(GetChunkSafe(x, y));
                }
            }
        }
    }


    public class ChunkHelpers
    {
        // Converts a block's world position to the chunk that contains it.
        // Floor division ensures negative coordinates map to the correct chunk (e.g. block -1 → chunk -1, not 0)
        public static Pos Block2Chunk(int x, int y)
        {
            return new(
                (int)Math.Floor((double)x / Preferences.CHUNK_SIZE),
                (int)Math.Floor((double)y / Preferences.CHUNK_SIZE)
            );
        }

        // Converts a block's world position to its local offset within its chunk (0 to CHUNK_SIZE-1).
        // Floor on modulo handles negative coordinates consistently with Block2Chunk
        public static Pos Block2Local(int x, int y)
        {
            int size = Preferences.CHUNK_SIZE;

            return new(
                ((x % size) + size) % size,
                ((y % size) + size) % size
            );
        }


        // Draws a faint grey border rectangle around the chunk boundary, scaled from chunk-space to pixel-space.
        // The low alpha value (5) makes it a subtle debug overlay rather than a solid outline
        public static void DrawDecors(Chunk chunk, EngineRenderer engineRenderer)
        {
            Pos worldPos = chunk.Pos.Mul(Preferences.CHUNK_SIZE * Preferences.TILE_SIZE);
            Size size = Size.ChunkSize().Mul(Preferences.TILE_SIZE).ToSize();

            engineRenderer.RectangleLines(new(worldPos, size), new(70, 70, 70, 50));
            engineRenderer.Text(worldPos.X, worldPos.Y, $"{chunk.Pos.X}, {chunk.Pos.Y}", new(80, 80, 80), 14);
        }
    }
}