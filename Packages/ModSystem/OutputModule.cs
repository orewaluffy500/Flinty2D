using NLua;

namespace Flinty.ModSystem;

public class OutputModule(string moduleName, APIBuilder builder, ModEngine engine) : IModule(moduleName, builder, engine)
{

    public override void Build()
    {
        base.Build();

        RegisterFunc("info", nameof(Info));
        RegisterFunc("error", nameof(Error));
        RegisterFunc("warning", nameof(Warning));
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