using Flinty.GameSystem;
using Flinty.Globals;
using Flinty.Player;
using Flinty.World;
using NLua;
using NLua.Exceptions;

namespace Flinty.ModSystem;


public class ModEngine(Engine engine)
{
    public Engine Engine { get; } = engine;
    public Terrain Terrain { get; } = engine.Terrain;
    public Clock Clock { get; } = engine.Clock;
    public PlayerEntity Player { get; } = engine.Terrain.Player;

    public Lua Lua { get; } = new();

    public Callbacks Callbacks { get; } = new();

    public List<ScriptMod> Mods { get; } = [];
    public List<string> QueuedMods { get; } = [];



    private static readonly HashSet<string> SandboxWhitelist = new()
    {
        "assert", "error", "ipairs", "pairs", "next",
        "pcall", "xpcall", "print", "select", "tonumber", "tostring",
        "type", "setmetatable", "getmetatable", "rawget", "rawset",
        "rawequal", "rawlen", "unpack",
        "string", "table", "math", "core", "___run_mod"
    };

    public static readonly string GAME_API_PREFIX = "core";

    private int ModIndex { get; set; } = 0;
    public void InitializeSystem()
    {
        GameLogger.ModEngineLog("ModSystem", Lua["_VERSION"] is string s ? s : "N/A");

        Lua.DoString(@"
        do
            local real_loadfile = loadfile
            function ___run_mod(filename, env)
                local file, err = real_loadfile(filename, 't', env)
                assert(file, err)
                return file()
            end
        end
        ");
    }


    public void HandleWhitelist()
    {
        var globals = (LuaTable)Lua["_G"];
        var keysToRemove = new List<string>();

        // Use Lua-side iteration instead of NLua's DictionaryEntry enumerator
        Lua.DoString(@"
        ___globalKeys = {}
        for k, _ in pairs(_G) do
            table.insert(___globalKeys, k)
        end
        ");

        var keysTable = (LuaTable)Lua["___globalKeys"];
        foreach (var keyObj in keysTable.Values)
        {
            if (keyObj is string key &&
                !SandboxWhitelist.Contains(key) &&
                key != "_G" && key != "_VERSION")
            {
                keysToRemove.Add(key);
            }
        }

        foreach (var key in keysToRemove)
        {
            globals[key] = null;
        }

        Lua.DoString("___globalKeys = nil");
    }

    private void RunMod(string filename)
    {
        try
        {
            ModIndex++;

            string envName = $"ModEnv_{ModIndex}";

            Lua.NewTable(envName);
            Lua.DoString($"setmetatable({envName}, {{ __index = _G }} )");

            Lua.GetFunction("___run_mod")
            .Call(filename, Lua[envName]);

            Mods.Add(new(this, envName));
        }
        catch (LuaException e)
        {
            GameLogger.ErrorLog($"Lua Error", $"{e}");
        }
    }

    public void InitializeModules()
    {
        new APIBuilder(this).BuildModules();

        HandleWhitelist();
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

        return !answers.Contains(false);
    }

    private List<object> Callback_Block(string id, string name, params object[] args)
    {
        var result = Callbacks.Fire($"block.any.{id}", args);
        var result2 = Callbacks.Fire($"block.{name}.{id}", args);

        result.AddRange(result2);
        return result;
    }
}