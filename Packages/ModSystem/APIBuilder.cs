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
        new OutputModule("out", this, ModEngine).Build();
        new PlayerModule("player", this, ModEngine).Build();
        new TerrainModule("terrain", this, ModEngine).Build();
    }
}