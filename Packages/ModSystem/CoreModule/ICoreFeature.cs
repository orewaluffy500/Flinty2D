using Flinty.ModSystem;

public abstract class ICoreFeature(CoreHelperModule coreHelperModule)
{
    public ModEngine Engine { get; } = coreHelperModule.Engine;
    public CoreHelperModule Core { get; } = coreHelperModule;

    public abstract void Create();
}