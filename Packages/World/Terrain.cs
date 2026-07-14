using Flinty.Assets;
using Flinty.GameMath;
using Flinty.GameSystem;
using Flinty.Globals;
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

            Player.Draw();
            Raylib.EndMode2D();

            DrawHUD();
            EngineRenderer.Text(0, 0, Raylib.GetFPS().ToString(), Color.RayWhite);
        }

        public void DrawHUD()
        {
            EngineRenderer.TextOrigined(100, 0, new(1, 0), $"Blocks Frozen: {TicksFrozen}", TicksFrozen ? PreColors.CalmGreen : PreColors.CalmRed);
            Player.DrawHUD();
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


        public bool Move(int x, int y, int dx, int dy, bool force = false)
        {
            // Get the two blocks
            GetBlockEx(x, y, out Point localBlockPos1, out Chunk chunk1, out Block? block1);
            GetBlockEx(dx, dy, out Point localBlockPos2, out Chunk chunk2, out Block? block2);

            if (block1 == null) return false; // Abort if target is null
            if (block2 != null && !force) return false; // Abort if destination exists and force isn't applied

            block1?.Pos.Set(dx, dy); // Obviously not null but just for the sake of perfectionism we use ?.

            chunk2.SetBlock(localBlockPos2.X, localBlockPos2.Y, block1); // Move the target to the destination
            chunk1.ClearBlock(localBlockPos1.X, localBlockPos1.Y); // Clear the old location

            return true;
        }


        public bool Place(int x, int y, string typeName, bool replace = false, bool raw = false)
        {
            if (!BlockRegistry.IsRegistered(typeName)) return false;

            GetBlockEx(x, y, out Point localBlockPos, out Chunk chunk, out Block? block);

            // Exit if the block already exists and we can't replace it.
            if (block != null && !replace) return false;

            // Set block
            chunk.SetBlock(localBlockPos.X, localBlockPos.Y, new Block(x, y, typeName, this));

            // Call script event
            Engine.FireBlockEvent("placed", typeName, x, y);

            if (!raw) UpdateBlocks(x, y, typeName);
            
            return true;
        }

        private void UpdateBlocks(int x, int y, string typeName)
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0) continue;

                    GetBlockEx(x + j, y + i, out Point _pos, out Chunk _c, out Block? tmp);
                    if (tmp != null)
                    {
                        Engine.FireBlockEvent("updated", tmp.Type, x, y, x + j, y + i);
                    }
                }
            }
        }

        public bool Break(int x, int y)
        {
            GetBlockEx(x, y, out Point localBlockPos, out Chunk chunk, out Block? block);

            if (block == null) return false;

            // Handle script event
            var answers = Engine.FireBlockEvent("can_break", block.Type);

            foreach (var o in answers)
            {
                GameLogger.DebugLog("Breaking block " + block.Type, o?.ToString() ?? "n/a");
            }

            if (answers.Contains(false)) return false;

            chunk.ClearBlock(localBlockPos.X, localBlockPos.Y);
            UpdateBlocks(x, y, block.Type);

            return true;
        }

        public void Fill(int x1, int y1, int x2, int y2, string name, bool replace = false)
        {
            int incX = x1 <= x2 ? 1 : -1;
            int incY = y1 <= y2 ? 1 : -1;

            for (int y = y1; y != y2 + incY; y += incY)
            {
                for (int x = x1; x != x2 + incX; x += incX)
                {
                    Place(x, y, name, replace, false);
                }
            }
        }

        private void GetBlockEx(int x, int y, out Point localBlockPos, out Chunk chunk, out Block? block)
        {
            // Retrieve chunk position
            Point chunkPos = ChunkHelpers.Block2Chunk(x, y);

            // Retrieve block position local to chunk
            localBlockPos = ChunkHelpers.Block2Local(x, y);

            // Get chunk (Lazy-loaded)
            chunk = ChunkManager.GetChunkSafe(chunkPos.X, chunkPos.Y);

            // Get nullable block
            block = chunk.GetBlock(localBlockPos.X, localBlockPos.Y);
        }

        public void SetBlock(int x, int y, string name)
        {
            GetBlockEx(x, y, out Point localBlockPos, out Chunk chunk, out Block? alreadyThere);

            if (alreadyThere != null) {
                alreadyThere.Type = name;
                alreadyThere.BlockEntry = BlockRegistry.GetBlockEntry(name);
            }
            else
            {
                chunk.SetBlock(x, y, new Block(x, y, name, this));
            }
        }

        public Block? GetBlock(int x, int y)
        {
            // Retrieve chunk position
            Point chunkPos = ChunkHelpers.Block2Chunk(x, y);

            // Retrieve block position local to chunk
            Point localBlockPos = ChunkHelpers.Block2Local(x, y);

            // Get chunk (Lazy-loaded)
            Chunk chunk = ChunkManager.GetChunkSafe(chunkPos.X, chunkPos.Y);

            // Get nullable block
            Block? block = chunk.GetBlock(localBlockPos.X, localBlockPos.Y);

            return block;
        }

        public string GetBlockName(int x, int y)
        {
            return GetBlock(x, y)?.Type ?? "air"; // Return block type or air
        }

        public bool IsBlock(int x, int y)
        {
            return GetBlockName(x, y) != "air"; // Return if said block is not air
        }

        public bool IsSolidBlock(int x, int y)
        {
            // Return if said block isn't air and is solid (can collide.)
            var block = GetBlock(x, y);
            if (block is null) return false;

            return block.CanCollide;
        }


        public bool CanBlocksTick()
        {
            return Engine.Clock.IsTicking && !TicksFrozen;
        }
    }
}