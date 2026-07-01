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
        ModEngine.Lua.NewTable(ScriptMod.GAME_EVENT_NAME);

        new NativeOutputModule("out", this, ModEngine).Build();
        new NativePlayerModule("player", this, ModEngine).Build();
        new NativeTerrainModule("terrain", this, ModEngine).Build();
        new NativeRegistryModule("registry", this, ModEngine).Build();
        new NativeEventModule("event", this, ModEngine).Build();
        new NativeClockModule("clock", this, ModEngine).Build();

        new CoreHelperModule(ModEngine.GAME_CORE_MASTER_MODULE_NAME, this, ModEngine);
    }
}