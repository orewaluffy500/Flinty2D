namespace Flinty.Globals;


public class GameLogger
{

    public static void LeveledLog(string level, string label, string msg, ConsoleColor color = ConsoleColor.Green){
        Console.ForegroundColor = color;
        Console.WriteLine($"[SYSTEM] {level.ToUpper()}: {label}: {msg}");
        Console.ResetColor();
    }

    public static void InfoLog(string label, string msg)
    {
        LeveledLog("info", label, msg);
    }

    public static void ModEngineLog(string label, string msg)
    {
        LeveledLog("info", label, msg, ConsoleColor.Cyan);
    }

    public static void ErrorLog(string label, string msg)
    {
        LeveledLog("error", label, msg, ConsoleColor.Red);
    }

    public static void WarningLog(string label, string msg)
    {
        LeveledLog("warning", label, msg, ConsoleColor.Yellow);
    }

    public static void LuaError(string cause, string msg)
    {
        Console.WriteLine($"Lua exception [{cause}] : {msg}");
    }
}