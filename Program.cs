using Flinty.Assets;
using Flinty.GameSystem;
using Flinty.Globals;
using NLua.Exceptions;

try {
    Engine engine = new("Flinty Beta", 1080, 720);

    engine.Start();

    Console.WriteLine("Press any key to terminate...");
    Console.ReadKey();
} catch (LuaException e)
{
    GameLogger.ErrorLog("Lua", $"{e.Message} {e.InnerException?.Message}");
} catch (Exception e)
{
    GameLogger.ErrorLog("Game", $"{e.GetType().Name}: {e.Message} {e.InnerException?.Message} at {e.Source} `{e.TargetSite}`");
}