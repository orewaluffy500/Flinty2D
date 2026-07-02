namespace Flinty.ModSystem;

public class CoreClockModule : ICoreFeature
{
    public CoreClockModule(CoreHelperModule coreHelperModule) : base(coreHelperModule)
    {
    }


    public override void Create()
    {
        string moduleName = $"{ModEngine.GAME_CORE_MASTER_MODULE_NAME}.clock";
        Engine.Lua.DoString(@$"{moduleName} = {{}}
            function {moduleName}.total_ticks()
                return NATIVE.clock.get_total_ticks()
            end

            function {moduleName}.total_seconds()
                return NATIVE.clock.get_total_seconds()
            end

            function {moduleName}.tick_index()
                return NATIVE.clock.get_current_tick()
            end

            function {moduleName}.total_minutes()
                return {moduleName}.total_seconds() // 60
            end

            function {moduleName}.total_hours()
                return {moduleName}.total_minutes() // 60
            end
        ");
    }

}
