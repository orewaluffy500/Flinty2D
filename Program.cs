using Flinty.GameSystem;

try {
    Engine engine = new("Flinty Beta", 1080, 720);

    engine.Start();

    Console.WriteLine("Press any key to terminate...");
    Console.ReadKey();
} catch (Exception e)
{
    Console.WriteLine($"Error: {e.Source}: {e.Message} {e?.InnerException}");
}