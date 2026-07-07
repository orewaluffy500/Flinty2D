using NLua;

namespace Flinty.ModSystem.CoreFeatures;


public class CoreFeatureEvents : ICoreFeature
{
    public CoreFeatureEvents(ModEngine engine, string featureName) : base(engine, featureName)
    {
    }

    public override void Build()
    {
        CreateFeatureTable();

        ModuleEvents instance = new(ModEngine);
        var type = typeof(ModuleEvents);

        foreach (var method in GetMethodsOf(type))
        {
            RegisterFunction(method.Name, method, instance);
        }
    }



    public class ModuleEvents(ModEngine engine)
    {
        public void on(string event_group, string event_name, LuaFunction function)
        {
            engine.Callbacks.Connect($"{event_group}.{event_name}", function);
        }   

        public void on_block_event(string block_name, string event_name, LuaFunction function)
        {
            on($"block.{block_name}", event_name, function);
        }
    }
}

