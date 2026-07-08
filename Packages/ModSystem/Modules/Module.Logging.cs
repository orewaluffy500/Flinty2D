namespace Flinty.ModSystem.Modules;



public class LoggingModule : INativeModule
{
    public LoggingModule(string moduleName, APIBuilder builder, ModEngine engine) : base(moduleName, builder, engine)
    {
    }

    public override void Initialize()
    {
        RegisterObject(ModuleName, typeof(InnerModule)); // Register as core.logging, Depends on what GAME_API_PREFIX, and ModuleName is.
    }


    public class InnerModule
    {
        public static void leveled(string level, string label, string text) => Console.WriteLine($"Mod [{level.ToUpper()}]: {label}: {text}");


        public static void log(string label, string text) => leveled("log", label, text);

        public static void error(string label, string text)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            leveled("error", label, text);
            Console.ResetColor();
        }

        public static void debug(string label, string text)
        {
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            leveled("debug", label, text);
            Console.ResetColor();
        }

        public static void warn(string label, string text)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            leveled("warn", label, text);
            Console.ResetColor();
        }
    }
}
