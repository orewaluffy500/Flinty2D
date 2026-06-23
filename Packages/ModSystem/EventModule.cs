using Flinty.Globals;
using NLua;

namespace Flinty.ModSystem;


public class EventModule : IModule
{
    public EventModule(string moduleName, APIBuilder builder, ModEngine engine) : base(moduleName, builder, engine)
    {
    }

    public override void Build()
    {
        base.Build();

        RegisterFunc("connect", nameof(Connect));
    }

    public void Connect(string id, LuaFunction function)
    {
        Engine.Callbacks.Connect(id, function);
    }
}