using NLua;

namespace Flinty.ModSystem;


public class Callbacks
{
    public Dictionary<string, List<LuaFunction>> callbacks { get; } = [];

    public Callbacks()
    {
    }


    public void Connect(string id, LuaFunction callback)
    {
        if (!callbacks.ContainsKey(id))
        {
            callbacks[id] = [];
        }

        callbacks[id].Add(callback);
    }

    public List<object> Fire(string id, params object[] args)
    {
        List<object> results = [];
        
        foreach (var callback in callbacks.TryGetValue(id, out var v) ? v : [])
        {
            if (callback is null) continue;
            
            var tmp_result = callback.Call(args);

            if (tmp_result is object[] arr)
            {
                results.AddRange(arr);
            } else
            {
                results.Add(tmp_result);
            }
        }

        return results;
    }
}