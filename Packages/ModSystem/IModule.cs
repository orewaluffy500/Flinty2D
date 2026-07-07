using System.Reflection;
using Flinty.GameSystem;
using Flinty.World;

namespace Flinty.ModSystem;

public abstract class INativeModule(string moduleName, APIBuilder builder, ModEngine engine)
{
    public APIBuilder Builder { get; } = builder;
    public ModEngine Engine { get; } = engine;
    public Engine GameEngine { get; } = engine.Engine;
    public Terrain Terrain { get; } = engine.Terrain;
    public string ModuleName { get; protected set; } = moduleName;

    public abstract void Initialize();

    public void RegisterObject(string name, Type type, object? instance = null, bool static_ = true)
    {
        string fullName = $"{ModEngine.GAME_API_PREFIX}.{name}";
        Engine.Lua.NewTable(fullName);

        var methods = static_ ? type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.DeclaredOnly) : type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public);

        foreach (var method in methods)
        {
            Engine.Lua.RegisterFunction($"{fullName}.{method.Name}", instance ?? this, method);
        }
    }

    public void RegisterFunc(string name, string method)
    {
        Engine.Lua.RegisterFunction($"{ModuleName}.{name}", this, GetType().GetMethod(method));
    }
}