using System.Reflection;
using Flinty.GameSystem;
using Flinty.World;

namespace Flinty.ModSystem;

public class INativeModule(string moduleName, APIBuilder builder, ModEngine engine)
{
    public APIBuilder Builder { get; } = builder;
    public ModEngine Engine { get; } = engine;
    public Engine GameEngine { get; } = engine.Engine;
    public Terrain Terrain { get; } = engine.Terrain;
    public string ModuleName { get; protected set; } = moduleName;

    public virtual void Build()
    {
        ModuleName = ModEngine.GAME_API_PREFIX + "." + ModuleName;
        Engine.Lua.NewTable(ModuleName);
    }

    public void RegisterFunc(string name, string method)
    {
        Engine.Lua.RegisterFunction($"{ModuleName}.{name}", this, GetType().GetMethod(method));
    }
}