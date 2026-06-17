using NLua;

namespace Flinty.ModSystem;

public class OutputModule(APIBuilder builder, ModEngine engine) : IModule
{
    public APIBuilder Builder => builder;

    public ModEngine Engine => engine;

    public void Build()
    {
        Engine.Lua.NewTable("f_out");

        var type = GetType();
        Engine.Lua.RegisterFunction("f_out.info", this, type.GetMethod(nameof(Info)));
        Engine.Lua.RegisterFunction("f_out.error", this, type.GetMethod(nameof(Error)));
        Engine.Lua.RegisterFunction("f_out.warning", this, type.GetMethod(nameof(Warning)));
    }

    public void Info(string text)
    {
        Console.WriteLine("Mod Message: " + text);
    }

    public void Error(string cause, string text)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Mod Error [{cause}]: {text}");
        Console.ResetColor();
    }
    
    public void Warning(string cause, string text)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"Mod Warning [{cause}]: {text}");
        Console.ResetColor();
    }
}