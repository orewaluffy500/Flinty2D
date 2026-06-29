using Flinty.Globals;
using Flinty.World;

namespace Flinty.ModSystem;


public class TerrainModule : IModule
{
    public TerrainModule(string moduleName, APIBuilder builder, ModEngine engine) : base(moduleName, builder, engine)
    {
    }

    public override void Build()
    {
        base.Build();

        RegisterFunc("place", nameof(Place));
        RegisterFunc("fill", nameof(Fill));
        RegisterFunc("move", nameof(MoveBlock));
        RegisterFunc("destroy", nameof(Break));
        RegisterFunc("get_block", nameof(GetBlock));
        RegisterFunc("is_empty", nameof(IsEmpty));
        RegisterFunc("block_meta", nameof(BlockMeta));
        RegisterFunc("block_opt_meta", nameof(OptBlockMeta));
    }


    public bool Place(int x, int y, string name, bool replace = false, bool raw = false)
    {
        return Terrain.Place(x, y, name, replace, raw);
    }

    public bool Break(int x, int y)
    {
        return Terrain.Break(x, y);
    }

    public void Fill(int sx, int sy, int ex, int ey, string name, bool replace = false)
    {
        Terrain.Fill(sx, sy, ex, ey, name, replace);
    }

    public string GetBlock(int x, int y)
    {
        return Terrain.GetBlockName(x, y); // return air if theres nothing there
    }

    public object? BlockMeta(int x, int y, string name, params object[] args)
    {
        Block? block = Terrain.GetBlock(x, y);
        if (block == null)
        {
            Logging.Error("ModSystem.TerrainModule", $"Could not find valid block at {x} , {y}");
            return null;
        }


        if (args.Length > 0)
        {
            block.Metadata.Set(name, args[0]);
            return args[0];    
        }

        return block.Metadata.Get(name);
    }

    public object? OptBlockMeta(int x, int y, string name, object def)
    {
        Block? block = Terrain.GetBlock(x, y);
        if (block == null)
        {
            Logging.Error("ModSystem.TerrainModule", $"Could not find valid block at {x} , {y}");
            return null;
        }

        return block.Metadata.OptGet(name, def);
    }

    public bool MoveBlock(int x, int y, int dx, int dy, bool force = false)
    {
        return Terrain.Move(x, y, dx, dy, force);
    }

    public bool IsEmpty(int x, int y) => !Terrain.IsBlock(x, y);
}