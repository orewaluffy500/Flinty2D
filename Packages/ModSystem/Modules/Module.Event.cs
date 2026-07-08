
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
            on($"game.{event_name}", function);
        }

        public void on_block_event(string block_name, string event_name, LuaFunction function)
        {
            on($"block.{block_name}.{event_name}", function);
        }
    }
}