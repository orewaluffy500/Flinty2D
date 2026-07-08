using System.Reflection;
using Flinty.GameSystem;
using Flinty.Globals;
using Flinty.World;

namespace Flinty.ModSystem;

public abstract class INativeModule
{
    public APIBuilder Builder { get; }
    public ModEngine Engine { get; }
    public Engine GameEngine { get; }
    public Terrain Terrain { get; }
    public string ModuleName { get; protected set; }

    public INativeModule(string moduleName, APIBuilder builder, ModEngine engine)
    {
        Builder = builder;
        ModuleName = moduleName;
        Engine = engine;
        GameEngine = engine.Engine;
        Terrain = engine.Terrain;

        GameLogger.ModEngineLog("IModule", $"Initialized new module: {ModuleName}.");
    }

    public abstract void Initialize();

    public void RegisterObject(string name, Type type, object? instance = null)
    {
        string fullName = $"{ModEngine.GAME_API_PREFIX}.{name}";
        Engine.Lua.NewTable(fullName); // Create table for that type

        bool static_ = instance is null; // Make it so no instance classes are auto-static

        var flags = BindingFlags.DeclaredOnly | BindingFlags.Public;

        if (static_){ flags |= BindingFlags.Static; } // add static flag when static
        else { flags |= BindingFlags.Instance; }

        // Get methods
        var methods = type.GetMethods(flags);

        foreach (var method in methods)
        {
            GameLogger.DebugLog("ModEngine", $"Register function {fullName}.{method.Name}");
            // Register each function
            Engine.Lua.RegisterFunction($"{fullName}.{method.Name}", instance ?? this, method);
        }

        // Log.
        GameLogger.ModEngineLog("IModule", $"Registered {(static_ ? "static" : "instance")} object: {name}");
    }

    public void RegisterFunc(string name, string method)
    {
        Engine.Lua.RegisterFunction($"{ModuleName}.{name}", this, GetType().GetMethod(method));
    }
}