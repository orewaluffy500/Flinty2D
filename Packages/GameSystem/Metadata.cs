using Flinty.Globals;

namespace Flinty.GameSystem;


public class Metadata
{
    public Dictionary<string, object> Data { get; private set; } = new();

    public object? Set(string name, object value)
    {
        Data[name] = value;
        return value;
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