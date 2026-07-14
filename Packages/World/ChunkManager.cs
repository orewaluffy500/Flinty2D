using Flinty.Assets;
using Flinty.GameMath;
using Flinty.Globals;

namespace Flinty.World;

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
            value = new Chunk(new Point(x, y));
            Chunks[key] = value;
        }

        return value;
    }

    public void RenderAndUpdateChunks()
    {
        UpdateVisibleChunks(Terrain.Player.Pos.X, Terrain.Player.Pos.Y);

        bool ticking = Terrain.Engine.Clock.IsTicking;
        int index = Terrain.Engine.Clock.TickIndex;

        foreach (Chunk chunk in VisibleChunks)
        {
            // Optionally overlay chunk border rectangles for debugging
            if (Preferences.DRAW_CHUNK_DECORS)
            {
                ChunkHelpers.DrawDecors(chunk);
            }

            if (ticking && !Terrain.TicksFrozen)
            {
                chunk.DoRandomTick(Terrain);
            }

            foreach (Block? block in chunk.Blocks)
            {
                if (block == null) continue;    // Sparse array — skip empty cells

                if (Terrain.CanBlocksTick())
                {
                    Terrain.Engine.FireBlockEvent("tick", block.Type, Terrain.Engine.Clock.TickIndex, block.Pos.X, block.Pos.Y);
                }

                block.Draw();
            }
        }
    }

    public void UpdateVisibleChunks(int originX, int originY)
    {
        VisibleChunks.Clear();

        // Convert the world-space block origin to chunk-space coordinates
        Point originChunkPos = ChunkHelpers.Block2Chunk(originX, originY);

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