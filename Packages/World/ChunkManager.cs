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
            value = new Chunk(new Coordinates(x, y));
            Chunks[key] = value;
        }

        return value;
    }

    public void RenderAndUpdateChunks()
    {
        UpdateVisibleChunks(Terrain.Player.Coords.X, Terrain.Player.Coords.Y);

        bool ticking = Terrain.Engine.Clock.IsTicking;
        int index = Terrain.Engine.Clock.TickIndex;

        int randomTickStep = Preferences.RANDOM_TICK_STEP;

        foreach (Chunk chunk in VisibleChunks)
        {
            // Optionally overlay chunk border rectangles for debugging
            if (Preferences.DRAW_CHUNK_DECORS)
            {
                ChunkHelpers.DrawDecors(chunk);
            }

            if (ticking && !Terrain.TicksFrozen && index % randomTickStep == 0)
            {
                chunk.DoRandomTick(Terrain);
            }

            foreach (TileNode? block in chunk.Blocks)
            {
                if (block == null) continue;    // Sparse array — skip empty cells

                if (Terrain.CanBlocksTick())
                {
                    Terrain.Engine.FireBlockEvent("tick", block.BlockId, Terrain.Engine.Clock.TickIndex, block.Coords.X, block.Coords.Y);
                }

                block.Draw();
            }
        }
    }

    public void UpdateVisibleChunks(int originX, int originY)
    {
        VisibleChunks.Clear();

        // Convert the world-space block origin to chunk-space coordinates
        Coordinates originChunkPos = ChunkHelpers.Node2ChunkCoord(originX, originY);

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