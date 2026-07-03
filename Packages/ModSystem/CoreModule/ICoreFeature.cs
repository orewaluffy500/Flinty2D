using System.Reflection;
using Flinty.GameSystem;
using Flinty.World;

namespace Flinty.ModSystem;

public abstract class ICoreFeature {
    public ModEngine ModEngine { get; }
    public Engine GameEngine { get; }
    public Terrain Terrain { get; }
    
    public string FeatureName { get; }
    public string FullFeaturePath { get; }
    
    public ICoreFeature(ModEngine engine, string featureName){
        ModEngine = engine;
        GameEngine = ModEngine.Engine;
        Terrain = GameEngine.Terrain;

        FeatureName = featureName;
        FullFeaturePath = $"{ModEngine.GAME_CORE_MASTER_MODULE_NAME}.{FeatureName}";
    }

    public void CreateFeatureTable()
    {
        ModEngine.Lua.NewTable(FullFeaturePath);
    }

    public MethodInfo[] GetStaticMethodsOf(Type type)
    {
        return type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.DeclaredOnly);
    }

    public MethodInfo[] GetMethodsOf(Type type)
    {
        return type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
    }
    

    public void RegisterFunction(string name, MethodInfo func, object? inst = null)
    {
        ModEngine.Lua.RegisterFunction($"{FullFeaturePath}.{name}", inst ?? this, func);
    }

    public abstract void Build();
}