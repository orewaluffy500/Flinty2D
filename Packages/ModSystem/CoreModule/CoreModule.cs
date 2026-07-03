using Flinty.ModSystem.CoreFeatures;

namespace Flinty.ModSystem;

public class CoreHelperModule : INativeModule
{
    private readonly string MODULE_NAME = ModEngine.GAME_CORE_MASTER_MODULE_NAME;

    public CoreHelperModule(string moduleName, APIBuilder builder, ModEngine engine) : base(moduleName, builder, engine)
    {
    }

    public override void Build()
    {
        new CoreFeatureLogging(Engine, "logging").Build();
        new CoreFeatureEvents(Engine, "event").Build();
        new CoreFeatureBlockRegistry(Engine, "block_registry").Build();
    }
}
