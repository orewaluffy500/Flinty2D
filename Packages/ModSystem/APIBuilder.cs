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
        ModEngine.Lua.NewTable(ModEngine.GAME_CORE_MASTER_MODULE_NAME);

        new CoreHelperModule(ModEngine.GAME_CORE_MASTER_MODULE_NAME, this, ModEngine).Build();
    }
}