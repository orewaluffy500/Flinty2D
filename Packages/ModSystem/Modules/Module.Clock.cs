using Flinty.GameSystem;
using Flinty.Globals;

namespace Flinty.ModSystem.Modules;


public class ClockModule : INativeModule
{
    public ClockModule(string moduleName, APIBuilder builder, ModEngine engine) : base(moduleName, builder, engine)
    {
    }

    public override void Initialize()
    {
        RegisterObject(ModuleName, typeof(InnerModule), new InnerModule(GameEngine.Clock));
    }



    public class InnerModule(Clock clock)
    {
        public int total_ticks()
        {
            return clock.TicksSinceStart;
        }
        
        public int total_seconds()
        {
            return clock.TicksSinceStart / tick_rate();
        }
        
        public int total_minutes()
        {
            return total_seconds() / 60;
        }

        public int current_tick()
        {
            return clock.TickIndex;
        }

        public int tick_rate()
        {
            return Preferences.TICK_RATE;
        }
    }
}