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

        new LoggingModule("logging", this, ModEngine).Initialize();
        new BlockRegModule("registry", this, ModEngine).Initialize();
    }
}