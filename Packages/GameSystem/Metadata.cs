using Flinty.Globals;

namespace Flinty.GameSystem;


public class Metadata
{
    public Dictionary<string, object> Data { get; private set; } = new();

    public void Set(string name, object? value)
    {
        if (value is null)
        {
            GameLogger.ErrorLog("GameSystem.Metadata", $"Cannot set {name} to null value.");  
            return;
        }

        Data[name] = value;
    }

    public object? Get(string name)
    {
        return Data.TryGetValue(name, out var v) ? v : null;
    }


    public object OptGet(string name, object def)
    {
        return Data.GetValueOrDefault(name, def);
    }
}