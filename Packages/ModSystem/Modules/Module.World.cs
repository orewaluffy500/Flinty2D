using System.Reflection;
using Flinty.World;

namespace Flinty.ModSystem.Modules;


public class WorldModule : INativeModule
{
    public WorldModule(string moduleName, APIBuilder builder, ModEngine engine) : base(moduleName, builder, engine)
    {
    }

    public override void Initialize()
    {
        RegisterObject(ModuleName, typeof(InnerModule), new InnerModule(Terrain));
    }



    public class InnerModule(Terrain terrain)
    {
        public bool destroy(int x, int y)
        {
            return terrain.Break(x, y);
        }

        public bool place(int x, int y, string name, bool replace = false, bool bypassNeighbours = false)
        {
            return terrain.Place(x, y, name, replace, bypassNeighbours);
        }

        public string get_name_of(int x, int y)
        {
            return terrain.GetBlockName(x, y);
        }
    }
}