using System.Runtime.CompilerServices;
using Flinty.GameSystem;
using Flinty.Globals;
using Flinty.Player;
using Flinty.World;
using KeraLua;
using NLua.Exceptions;

namespace Flinty.ModSystem;


public class ModEngine(Engine engine)
{
    public Engine Engine { get; } = engine;
    public Terrain Terrain { get; } = engine.Terrain;
    public Clock Clock { get; } = engine.Clock;
    public PlayerEntity Player { get; } = engine.Terrain.Player;

    public NLua.Lua Lua { get; } = new();

    public Callbacks Callbacks { get; } = new();

    public List<ScriptMod> Mods { get; } = [];
    public List<string> QueuedMods { get; } = [];

    public static readonly string GAME_API_PREFIX = "core";

    private int ModIndex { get; set; } = 0;
    public void InitializeSystem()
    {

        GameLogger.ModEngineLog("ModSystem", Lua["_VERSION"] is string s ? s : "N/A");

        Lua.DoString(@"
        function ___run_mod(filename, env)
            local file, err = loadfile(filename, 't', env)
            assert(file, err)
            return file()
        end
        ");
    }

    private void RunMod(string filename)
    {
        try {
            ModIndex++;

            string envName = $"env{ModIndex}";

            Lua.NewTable(envName);
            Lua.DoString($"setmetatable({envName}, {{ __index = _G }} )");

            Lua.GetFunction("___run_mod")
            .Call(filename, Lua[envName]);

            Mods.Add(new(this, envName));
        } catch (LuaException e)
        {
            GameLogger.ErrorLog($"Lua Error: {filename}", $"{e.Message}, {e.InnerException?.Message}");
        }
    }

    public void InitializeModules()
    {
        new APIBuilder(this).BuildModules();
    }
    public void LoadScript(string filename)
    {
        QueuedMods.Add(filename);
        GameLogger.ModEngineLog("ModEngine", $"Succesfully loaded `{filename}`");
    }

    public void RunQueuedMods()
    {
        foreach (string filename in QueuedMods)
        {
            RunMod(filename);
        }

        GameLogger.ModEngineLog("ModEngine", $"Ran {QueuedMods.Count} queued mods");
    }



    public void Callback_Start()
    {
        Callbacks.Fire("game.start");
    }

    public void Callback_Tick()
    {
        Callbacks.Fire("game.tick");
    }

    public void Callback_End()
    {
        Callbacks.Fire("game.end");
    }

    public void Callback_PlayerMove(int ox, int oy, int nx, int ny)
    {
        Callbacks.Fire("player.moved", ox, oy, nx, ny);
    }

    public void Callback_BlockPlaced(int x, int y, string name)
    {
        Callback_Block("placed", name, x, y, name);
    }

    public void Callback_BlockUpdated(int mx, int my, string mname, int x, int y, string name) // Called when a block is placed in a 3x3 area of another block, mx and stuff are the block that triggered it and the normal ones are the block that got triggered
    {
        Callback_Block("updated", name, mx, my, mname, x, y, name);
    }

    public void Callback_BlockTick(int x, int y, string name)
    {
        Callback_Block("tick", name, x, y, name);
    }

    public void Callback_BlockRandomTick(int x, int y, string name)
    {
        Callback_Block("random_tick", name, x, y, name);
    }

    public bool Callback_BlockBreaking(int x, int y, string name)
    {
        var answers = Callback_Block("breaking", name, x, y, name);

        foreach (var v in answers)
        {
            if (v is bool b && !b)
            {
                return false;
            }
        }

        return true; // prioritize denied
    }

    private List<object> Callback_Block(string id, string name, params object[] args)
    {
        var result = Callbacks.Fire($"block.any.{id}", args);
        var result2 = Callbacks.Fire($"block.{name}.{id}", args);

        result.AddRange(result2);
        return result;
    }
}