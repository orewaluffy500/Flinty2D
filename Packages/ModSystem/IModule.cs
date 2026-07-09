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
            RegisterFuncDirect($"{fullName}.{method.Name}", method, instance);
        }

        // Log.
        GameLogger.ModEngineLog("IModule", $"Registered {(static_ ? "static" : "instance")} object: {name}");
    }

    public void RegisterType(string typeName, Type type, object? instance = null)
    {
        foreach (var method in type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public))
        {
            GameLogger.DebugLog("IModule", $"Register function {ModEngine.GAME_API_PREFIX}.{typeName}:{method.Name}");
            RegisterFuncDirect($"{ModEngine.GAME_API_PREFIX}.{typeName}.{method.Name}", method, instance);
        }

        GameLogger.ModEngineLog("IModule", $"Registered type: {typeName}");
    }

    public void RegisterFunc(string name, string method)
    {
        Engine.Lua.RegisterFunction($"{ModuleName}.{name}", this, GetType().GetMethod(method));
    }

    public void RegisterFuncDirect(string path, MethodInfo? method, object? instance = null)
    {
        Engine.Lua.RegisterFunction(path, instance ?? this, method);
    }

    public void RegisterMethod(string name, string oname, Type type, string method, object? instance = null)
    {
        RegisterFuncDirect($"{oname}:{name}", type.GetMethod(method), instance);
    }
}