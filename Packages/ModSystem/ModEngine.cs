using Flinty.GameSystem;
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

    public void InitializeModules()
    {
        new APIBuilder(this).BuildModules();
    }
    public void LoadScript(string filename)
    {
        try
        {
            Lua.DoFile(filename);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.StackTrace);
            Console.WriteLine(e.Message);
        }
    }
}