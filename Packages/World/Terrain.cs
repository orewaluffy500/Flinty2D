using Flinty.Assets;
using Flinty.GameMath;
using Flinty.GameSystem;
using Flinty.Player;
using Raylib_cs;

namespace Flinty.World
{
    public class Terrain
    {
        public Engine Engine { get; set; }
        public ChunkManager ChunkManager { get; }

        public PlayerEntity Player { get; }

        public bool TicksFrozen { get; set; } = false;

        public Terrain(Engine eng)
        {
            Engine = eng;
            ChunkManager = new(this);
            Player = new(this);
        }

        public void Draw()
        {
            Raylib.BeginMode2D(Player.Camera);
            ChunkManager.RenderAndUpdateChunks();

            Player.Draw(Engine.Renderer);
            Raylib.EndMode2D();

            DrawHUD();
            Engine.Renderer.Text(0, 0, Raylib.GetFPS().ToString(), Color.RayWhite);
        }

        public void DrawHUD()
        {
            Engine.Renderer.TextOrigined(100, 0, new(1, 0), $"Blocks Frozen: {TicksFrozen}", TicksFrozen ? PreColors.CalmGreen : PreColors.CalmRed);
            Player.DrawHUD(Engine.Renderer);
        }

        public void Update(float deltaTime)
        {
            Player.Update(deltaTime);
        }

        public void Once()
        {
            Player.Update(1); // Head-tick
        }

        public void Final()
        {
            
        }


        public void Move(int x, int y, int dx, int dy, bool force = false)
        {
            GetBlockEx(x, y, out Pos localBlockPos1, out Chunk chunk1, out Block? block1);
            GetBlockEx(dx, dy, out Pos localBlockPos2, out Chunk chunk2, out Block? block2);

            if (block1 == null) return;
            if (block2 != null && !force) return;
            
            block1?.Pos.Set(dx, dy);

            chunk2.SetBlock(localBlockPos2.X, localBlockPos2.Y, block1);
            chunk1.ClearBlock(localBlockPos1.X, localBlockPos1.Y);
        }

        
        public void Place(int x, int y, string typeName, bool replace = false)
        {
            if (!BlockRegistry.IsRegistered(typeName)) return;

            GetBlockEx(x, y, out Pos localBlockPos, out Chunk chunk, out Block? block);

            // Exit if the block already exists and we can't replace it.
            if (block != null && !replace) return;

            // Set block
            chunk.SetBlock(localBlockPos.X, localBlockPos.Y, new Block(x, y, typeName, this));

            // Call script event
            Engine.ModEngine.Callback_BlockPlaced(x, y, typeName);
        }

        public void Break(int x, int y)
        {
            GetBlockEx(x, y, out Pos localBlockPos, out Chunk chunk, out Block? block);

            if (block == null) return;

            // Handle script event
            bool continue_ = Engine.ModEngine.Callback_BlockBreaking(x, y, block.Type);
            
            if (continue_ == false) return; // Must use explicit check to ensure both nil and true mean continue (for sake of simplicity    )

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

        public string GetBlockName(int x, int y)
        {
            return GetBlock(x, y)?.Type ?? "air";
        }

        public bool IsBlock(int x, int y)
        {
            return GetBlockName(x, y) != "air";
        }

        public bool IsSolidBlock(int x, int y)
        {
            var block = GetBlock(x, y);
            if (block is null) return false;
            
            return block.CanCollide;
        }
    }
}