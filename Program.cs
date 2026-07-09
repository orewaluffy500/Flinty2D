using Flinty.GameSystem;
using Flinty.Globals;
using NLua.Exceptions;

// Default window size
int Width = 1080;
int Height = 640;

// Default caption
string Caption = "Flinty Beta";

try {
    // Check if custom width and height are passed
    if (args.Length > 1)
    {
        if (int.TryParse(args[0], out int width)) Width = width;
        if (int.TryParse(args[1], out int height)) Height = height;
    }

    Engine engine = new(Caption, Width, Height);

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