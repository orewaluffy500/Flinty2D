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
 
    public List<ScriptMod> Mods { get; } = [];



    public static readonly string GAME_API_PREFIX = "API";

    private int ModIndex { get; set; } = 0;
    public void InitializeSystem()
    {
        Console.WriteLine(Lua["_VERSION"]);
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
        ModIndex++;

        string envName = $"env{ModIndex}";

        Lua.NewTable(envName);
        Lua.DoString($"setmetatable({envName}, {{ __index = _G }} )");

        Lua.GetFunction("___run_mod")
        .Call(filename, Lua[envName]);

        Mods.Add(new(this, envName));
    }

    public void InitializeModules()
    {
        Lua.DoString(@"
        function tostr(o)
            return '' .. o
        end");

        new APIBuilder(this).BuildModules();
    }
    public void LoadScript(string filename)
    {
        try
        {
            RunMod(filename);
            Logging.Message("ModSystem.ModEngine", $"Succesfully loaded `{filename}`");
        } catch (LuaException e)
        {
            Console.WriteLine(e.Message + " " + e.InnerException?.Message);
        }
    }




    public void Callback_Start()
    {
        foreach (ScriptMod mod in Mods)
        {
            mod.Callback_Start();
        }
    }

    public void Callback_Tick()
    {
        foreach (ScriptMod mod in Mods)
        {
            mod.Callback_Tick();
        }
    }

    public void Callback_Final()
    {
        foreach (ScriptMod mod in Mods)
        {
            mod.Callback_Final();
        }
    }
}