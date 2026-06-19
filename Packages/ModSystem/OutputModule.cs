using NLua;

namespace Flinty.ModSystem;

public class OutputModule(APIBuilder builder, ModEngine engine) : IModule
{
    public APIBuilder Builder => builder;

    public ModEngine Engine => engine;

    public void Build()
    {
        Engine.Lua.NewTable("Out");

        var type = GetType();
        Engine.Lua.RegisterFunction("Out.info", this, type.GetMethod(nameof(Info)));
        Engine.Lua.RegisterFunction("Out.error", this, type.GetMethod(nameof(Error)));
        Engine.Lua.RegisterFunction("Out.warning", this, type.GetMethod(nameof(Warning)));
    }

    public void Info(object text)
    {
        if (text is string t)
        {
            Console.WriteLine("Mod Message: " + text);
        }
    }

    public void Error(object cause, object text)
    {
        if (text is string t)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Mod Error [{cause}]: {text}");
            Console.ResetColor();
        }
    }

    public void Warning(object cause, object text)
    {
        if (text is string t)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Mod Warning [{cause}]: {text}");
            Console.ResetColor();
        }
    }
}