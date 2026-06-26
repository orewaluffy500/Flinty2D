using Flinty.GameMath;
using Flinty.GameSystem;
using Flinty.Globals;

namespace Flinty.World
{
    public class Chunk(Pos pos)
    {
        public Block?[] Blocks { set; get; } = new Block[Preferences.CHUNK_SIZE * Preferences.CHUNK_SIZE];
        public Pos Pos { get; } = pos;

        public int BlockIndex(int x, int y)
        {
            return x + (y * Preferences.CHUNK_SIZE);
        }
        public void SetBlock(int x, int y, Block? block)
        {
            Blocks[BlockIndex(x, y)] = block;
        }

        public void SetBlockIfAbsent(int x, int y, Block? block)
        {
            if (Blocks[BlockIndex(x, y)] == null)
            {
                SetBlock(x, y, block);
            }
        }


        public void ClearBlock(int x, int y)
        {
            Blocks[BlockIndex(x, y)] = null;
        }

        public Block? GetBlock(int x, int y)
        {
            return Blocks[BlockIndex(x, y)];
        }

        public void MoveBlock(int x, int y, int dx, int dy, bool force = false)
        {
            if (Blocks[BlockIndex(x, y)] == null) return;

            if (Blocks[BlockIndex(dx, dy)] != null && !force) return;

            Blocks[BlockIndex(dx, dy)] = Blocks[BlockIndex(x, y)];
            Blocks[BlockIndex(x, y)] = null;
        }


        // Packs two 32-bit chunk coordinates into a single 64-bit key for dictionary lookup.
        // X occupies the upper 32 bits; Y is cast to uint to prevent sign-extension in the lower 32 bits.
        public static long GetChunkKey(int x, int y)
        {
            return ((long)x << 32) | (uint)y;
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
        public static void DrawDecors(Chunk chunk)
        {
            Pos worldPos = chunk.Pos.Mul(Preferences.CHUNK_SIZE * Preferences.TILE_SIZE);
            Size size = Size.ChunkSize().Mul(Preferences.TILE_SIZE).ToSize();

            EngineRenderer.RectangleLines(new(worldPos, size), new(70, 70, 70, 50));
            EngineRenderer.Text(worldPos.X, worldPos.Y, $"{chunk.Pos.X}, {chunk.Pos.Y}", new(80, 80, 80), 14);
        }
    }
}