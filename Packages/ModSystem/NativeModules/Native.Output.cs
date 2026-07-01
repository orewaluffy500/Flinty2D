using NLua;

namespace Flinty.ModSystem;

public class NativeOutputModule(string moduleName, APIBuilder builder, ModEngine engine) : INativeModule(moduleName, builder, engine)
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
        if (text.ToString() != null)
        {
            Console.WriteLine("Mod Message: " + text.ToString());
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