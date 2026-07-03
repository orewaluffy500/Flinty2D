namespace Flinty.ModSystem.CoreFeatures;



public class CoreFeatureLogging(ModEngine engine, string moduleName) : ICoreFeature(engine, moduleName)
{
    public override void Build()
    {
        CreateFeatureTable();

        var moduleLoggingType = typeof(ModuleLogging);

        foreach (var method in moduleLoggingType.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.DeclaredOnly))
        {
            RegisterFunction(method.Name, method);
        }
    }


    public static class ModuleLogging
    {
        public static void leveled(string label, string level, string text)
        {
            Console.WriteLine($"MOD [{level}]: {label}: {text}");
        }
        public static void info(string label, string text)
        {
            leveled(label, "INFO", text);
        }

        public static void error(string label, string text)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            leveled(label, "ERR", text);
            Console.ResetColor();
        }

        public static void warn(string label, string text)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            leveled(label, "WARN", text);
            Console.ResetColor();
        }
    }
}


