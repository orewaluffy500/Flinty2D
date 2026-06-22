using Flinty.GameSystem;
using Flinty.Globals;
using NLua;
using NLua.Exceptions;

namespace Flinty.ModSystem;


public class ScriptMod
{
    public ModEngine Engine { get; }
    public string EnvName { get; }

    public static readonly string GAME_EVENT_NAME = "Event";

    public ScriptMod(ModEngine engine, string envName)
    {
        Engine = engine;
        EnvName = envName;
    }


    public void Callback_Start() => RunCallback("start");

    public void Callback_Tick() => RunCallback("tick");

    public void Callback_BlockTick(int x, int y, string name) => RunCallback("block_tick", x, y, name);

    public void Callback_End() => RunCallback("exit");

    public void Callback_BlockPlaced(int x, int y, string name) => RunCallback("block_placed", x, y, name);

    public bool Callback_BlockBreaking(int x, int y, string name)
    {
        object? result = RunCallback("block_breaking", x, y, name);

        if (result is object[] values &&
            values.Length > 0 &&
            values[0] is bool b)
        {
            return b;
        }

        // no boolean returned -> allow breaking
        return true;
    }

    public LuaFunction? GetCallback(string name)
    {
        var o = Engine.Lua[EnvName + "." + $"{GAME_EVENT_NAME}.{name}"];

        if (o is LuaFunction f)
        {
            return f;
        }

        return null;
    }


    public object? RunCallback(string name, params object[] args)
    {
        try
        {
            if (GetCallback(name) is LuaFunction f)
            {
                return f.Call(args);
            }
        }
        catch (LuaException e)
        {
            Logging.LuaError(name, $"{e.Message} {e.InnerException?.Message}");
        }

        return null;
    }
    
}