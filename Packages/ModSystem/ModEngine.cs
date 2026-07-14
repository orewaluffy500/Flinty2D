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
    public PlayerNode Player { get; } = engine.Terrain.Player;

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

    // BASE CALLBACK FIRERS
    public List<object> FireCallback(string id, params object[] args) => Callbacks.Fire(id, args);

}