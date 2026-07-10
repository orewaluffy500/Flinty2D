using System.Reflection;
using Flinty.World;
using NLua;

namespace Flinty.ModSystem.Modules;


public class WorldModule : INativeModule
{
    public WorldModule(string moduleName, APIBuilder builder, ModEngine engine) : base(moduleName, builder, engine)
    {
    }

    public override void Initialize()
    {
        RegisterObject(ModuleName, typeof(InnerModule), new InnerModule(Terrain));

        Engine.Lua.DoString(@$"
        {ModEngine.GAME_API_PREFIX}.Block = {{}}
        {ModEngine.GAME_API_PREFIX}.Block.__index = {ModEngine.GAME_API_PREFIX}.Block

        function {ModEngine.GAME_API_PREFIX}.{ModuleName}.get_block(x, y)
            local o = setmetatable({{}}, {ModEngine.GAME_API_PREFIX}.Block)
            o.x = x
            o.y = y
            o.name = {ModEngine.GAME_API_PREFIX}.{ModuleName}.get_name_of(x, y)
            return o
        end
        ");


        Type type = typeof(InnerBlockType);
        InnerBlockType instance = new(Terrain);

        RegisterType("Block", type, instance);
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



    public class InnerBlockType(Terrain terrain)    
    {
        public bool same_as(LuaTable o, string other_name){
            return Name(o) == other_name;
        }

        public bool destroy(LuaTable o)
        {
            if (Invalid(o)) return false;

            return terrain.Break(X(o), Y(o));
        }

        public bool duplicate(LuaTable o, int ox, int oy, bool replace = false)
        {
            if (Invalid(o)) return false;

            return terrain.Place(X(o), Y(o), Name(o), replace);
        }

        public object? get_meta(LuaTable o, string key)
        {
            if (Invalid(o)) return null;

            var block = terrain.GetBlock(X(o), Y(o));
            return block?.Metadata.Get(key);
        }

        public object opt_meta(LuaTable o, string key, object def)
        {
            if (Invalid(o)) return def;
            
            var block = terrain.GetBlock(X(o), Y(o));
            return block is null ? def : block.Metadata.OptGet(key, def);
        }

        public object? set_meta(LuaTable o, string key, object value)
        {
            if (Invalid(o)) return null;
            
            var block = terrain.GetBlock(X(o), Y(o));
            block?.Metadata.Set(key, value);

            return block?.Metadata.Get(key);
        }


        private static int X(LuaTable o) => Convert.ToInt32(o["x"]);
        private static int Y(LuaTable o) => Convert.ToInt32(o["y"]);
        private static string Name(LuaTable o) => (string) o["name"] ?? "air";
        private static bool Invalid(LuaTable o) => o["name"] is null or (object)"air";
    }
}