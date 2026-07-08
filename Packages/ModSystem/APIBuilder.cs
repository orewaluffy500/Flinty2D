using Flinty.ModSystem.Modules;

namespace Flinty.ModSystem;

public class APIBuilder(ModEngine engine)
{
    public ModEngine ModEngine { get; } = engine;

    public void BuildModules()
    {
        BuildOutputModule();
    }

    public void BuildOutputModule()
    {
        ModEngine.Lua.NewTable(ModEngine.GAME_API_PREFIX);

        INativeModule[] modules =
        {
            new LoggingModule("logging", this, ModEngine),
            new EventsModule("event", this, ModEngine),
            new BlockRegModule("registry", this, ModEngine),
            new WorldModule("world", this, ModEngine),
            new MathModule("math", this, ModEngine),
        };

        // Initialize the types AFTER declaring the module instances.
        foreach (INativeModule module in modules)
        {
            module.Initialize();
        }
    }
}