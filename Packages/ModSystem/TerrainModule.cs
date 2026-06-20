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
        RegisterFunc("destroy", nameof(Break));
        RegisterFunc("get_block", nameof(GetBlock));
        RegisterFunc("is_empty", nameof(IsEmpty));
        RegisterFunc("block_meta", nameof(BlockMeta));
    }


    public void Place(int x, int y, string name, bool replace = false)
    {
        Terrain.Place(x, y, name, replace);
    }

    public void Break(int x, int y)
    {
        Terrain.Break(x, y);
    }

    public string GetBlock(int x, int y)
    {
        return Terrain.GetBlockName(x, y);
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

    public bool IsEmpty(int x, int y) => !Terrain.IsBlock(x, y);
}