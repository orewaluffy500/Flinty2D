
using Flinty.Globals;
using Flinty.World;
using NLua;

namespace Flinty.ModSystem.Modules;

public class EventsModule : INativeModule
{
    public EventsModule(string moduleName, APIBuilder builder, ModEngine engine) : base(moduleName, builder, engine)
    {
    }

    public override void Initialize()
    {
        InnerModule instance = new(Engine);
        RegisterObject(ModuleName, typeof(InnerModule), instance);
    }



    public class InnerModule(ModEngine engine)
    {
        public void on(string event_name, LuaFunction function)
        {
            engine.Callbacks.Connect(event_name, function);
        }

        public void on_game_event(string event_name, LuaFunction function)
        {
            on($"{EventCategories.GLOBAL_GAME_EVENTS}.{event_name}", function);
        }

        public void on_block_event(string block_name, string event_name, LuaFunction function)
        {
            on($"{EventCategories.BLOCK_EVENTS}.{block_name}.{event_name}", function);
        }

        public void on_player_event(string event_name, LuaFunction function)
        {
            on($"{EventCategories.PLAYER_EVENTS}.{event_name}", function);
        }

        public void on_clock_event(string event_name, LuaFunction function)
        {
            on($"{EventCategories.CLOCK_EVENTS}.{event_name}", function);
        }
    }
}