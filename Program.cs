using Flinty.GameSystem;

try {
    Engine engine = new("Flinty v0", 1080, 576);

    engine.Start();

    Console.WriteLine("Press any key to terminate...");
    Console.ReadKey();
} catch (Exception e)
{
    Console.WriteLine($"Error: {e.Message} {e?.InnerException}");
}