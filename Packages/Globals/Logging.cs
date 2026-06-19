namespace Flinty.Globals;


public class Logging
{
    public static void Message(string path, string msg)
    {
        Console.WriteLine($"Message from {path} : {msg}");
    }
}