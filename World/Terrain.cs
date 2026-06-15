using Flinty.GameMath;
using Flinty.GameSystem;
using Raylib_cs;

namespace Flinty.World
{
    public class Terrain
    {
        public Engine Engine { get; set; }
        public ChunkManager ChunkManager { get; }

        public Player Player { get; }

        public Terrain(Engine eng)
        {
            Engine = eng;
            ChunkManager = new(this);
            Player = new(this);
        }

        public void Draw()
        {
            ChunkManager.RenderAndUpdateChunks();

            Player.Draw(Engine.Renderer);
            Engine.Renderer.Text(0, 0, Raylib.GetFPS().ToString(), Color.RayWhite);
        }

        public void Update(float deltaTime)
        {
            Player.Update(deltaTime);
        }

        public void Once()
        {
            
        }

        public void Final()
        {
            
        }

        
        public void Place(int x, int y, string typeName, Boolean replace = false)
        {
            GetBlockEx(x, y, out Pos localBlockPos, out Chunk chunk, out Block? block);

            // Exit if the block already exists and we can't replace it.
            if (block != null && !replace) return;

            // Set block
            chunk.SetBlock(localBlockPos.X, localBlockPos.Y, new Block(x, y, typeName));
        }

        public void Break(int x, int y)
        {
            GetBlockEx(x, y, out Pos localBlockPos, out Chunk chunk, out Block? block);

            if (block == null) return;

            chunk.ClearBlock(localBlockPos.X, localBlockPos.Y);                
        }

        private void GetBlockEx(int x, int y, out Pos localBlockPos, out Chunk chunk, out Block? block)
        {
            // Retrieve chunk position
            Pos chunkPos = ChunkHelpers.Block2Chunk(x, y);

            // Retrieve block position local to chunk
            localBlockPos = ChunkHelpers.Block2Local(x, y);

            // Get chunk (Lazy-loaded)
            chunk = ChunkManager.GetChunkSafe(chunkPos.X, chunkPos.Y);

            // Get nullable block
            block = chunk.GetBlock(localBlockPos.X, localBlockPos.Y);
        }



        public Block? GetBlock(int x, int y)
        {
            // Retrieve chunk position
            Pos chunkPos = ChunkHelpers.Block2Chunk(x, y);

            // Retrieve block position local to chunk
            Pos localBlockPos = ChunkHelpers.Block2Local(x, y);

            // Get chunk (Lazy-loaded)
            Chunk chunk = ChunkManager.GetChunkSafe(chunkPos.X, chunkPos.Y);

            // Get nullable block
            Block? block = chunk.GetBlock(localBlockPos.X, localBlockPos.Y);
            
            return block;
        }

        public bool IsBlock(int x, int y)
        {
            return GetBlock(x, y) != null;
        }
    }
}