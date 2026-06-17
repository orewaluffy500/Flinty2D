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
        new OutputModule(this, ModEngine).Build();
        new PlayerModule(this, ModEngine).Build();
    }
}