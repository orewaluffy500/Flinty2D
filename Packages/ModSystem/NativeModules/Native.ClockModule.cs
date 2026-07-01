using Flinty.Globals;
namespace Flinty.ModSystem;


public class NativeClockModule : INativeModule
{
    public NativeClockModule(string moduleName, APIBuilder builder, ModEngine engine) : base(moduleName, builder, engine)
    {
    }

    public override void Build()
    {
        base.Build();

        RegisterFunc("get_total_ticks", nameof(GetTicksSinceStart));
        RegisterFunc("get_current_tick", nameof(GetCurrentTick));
        RegisterFunc("get_total_seconds", nameof(GetSecondsSinceStart));
    }

    public int GetTicksSinceStart()
    {
        return Engine.Engine.Clock.TicksSinceStart;
    }

    public int GetCurrentTick()
    {
        return Engine.Engine.Clock.TickIndex;
    }

    public int GetSecondsSinceStart()
    {
        return GetTicksSinceStart() / Preferences.TICK_RATE;
    }

}
