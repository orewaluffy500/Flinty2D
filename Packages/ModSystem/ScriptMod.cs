using Flinty.GameSystem;
using Flinty.Globals;
using NLua;
using NLua.Exceptions;

namespace Flinty.ModSystem;


public class ScriptMod
{
    public ModEngine Engine { get; }
    public string EnvName { get; }

    public ScriptMod(ModEngine engine, string envName)
    {
        Engine = engine;
        EnvName = envName;
    }


    public void Callback_Start()
    {
        RunCallback("Start");
    }

    public void Callback_Tick()
    {
        RunCallback("_Tick");
    }

    public void Callback_Final()
    {
        RunCallback("Final");
    }


    public LuaFunction? GetCallback(string name)
    {
        var o = Engine.Lua[EnvName + "." + name];

        if (o is LuaFunction f)
        {
            return f;
        }

        return null;
    }


    public void RunCallback(string name, params object[] args)
    {
        try {
        if (GetCallback(name) is LuaFunction f)
        {
            f.Call(args);
        }
        }
        catch (LuaException e)
        {
            Logging.LuaError(name, $"{e.Message} {e.InnerException?.Message}");
        }
    }
}