using Raylib_cs;

namespace Flinty.Globals
{
    public class MouseMap
    {
        public static Dictionary<string, MouseButton> Dict { get; } = new()
        {
            ["LMB"] = MouseButton.Left,
            ["RMB"] = MouseButton.Right,
            ["MMB"] = MouseButton.Middle,
            ["SMB"] = MouseButton.Side,
            ["EMB"] = MouseButton.Extra  
        };

        public static bool ButtonDown(string btn)
        {
            return Dict.ContainsKey(btn) ? Raylib.IsMouseButtonDown(Dict[btn]) : false;
        }

        public static bool ButtonUp(string btn)
        {
            return Dict.ContainsKey(btn) ? Raylib.IsMouseButtonUp(Dict[btn]) : false;
        }
    }
}