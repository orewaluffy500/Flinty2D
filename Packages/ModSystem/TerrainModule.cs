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

    public bool IsEmpty(int x, int y) => !Terrain.IsBlock(x, y);
}