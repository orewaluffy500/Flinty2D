using Raylib_cs;

namespace Flinty.Globals
{
    public class KeyMap
    {
        public static Dictionary<string, KeyboardKey> Dict { get; } = new Dictionary<string, KeyboardKey>
        {
            ["A"] = KeyboardKey.A,
            ["B"] = KeyboardKey.B,
            ["C"] = KeyboardKey.C,
            ["D"] = KeyboardKey.D,
            ["E"] = KeyboardKey.E,
            ["F"] = KeyboardKey.F,
            ["G"] = KeyboardKey.G,
            ["H"] = KeyboardKey.H,
            ["J"] = KeyboardKey.J,
            ["K"] = KeyboardKey.K,
            ["L"] = KeyboardKey.L,
            ["M"] = KeyboardKey.M,
            ["N"] = KeyboardKey.N,
            ["O"] = KeyboardKey.O,
            ["P"] = KeyboardKey.P,
            ["Q"] = KeyboardKey.Q,
            ["R"] = KeyboardKey.R,
            ["S"] = KeyboardKey.S,
            ["T"] = KeyboardKey.T,
            ["U"] = KeyboardKey.U,
            ["V"] = KeyboardKey.V,
            ["W"] = KeyboardKey.W,
            ["X"] = KeyboardKey.X,
            ["Y"] = KeyboardKey.Y,
            ["Z"] = KeyboardKey.Z,
            ["Enter"] = KeyboardKey.Enter,
            ["Escape"] = KeyboardKey.Escape,
            ["LeftControl"] = KeyboardKey.LeftControl,
            ["RightControl"] = KeyboardKey.RightControl,


            ["MoveUp"] = KeyboardKey.W,
            ["MoveDown"] = KeyboardKey.S,
            ["MoveLeft"] = KeyboardKey.A,
            ["MoveRight"] = KeyboardKey.D,
            ["CycleBlocks"] = KeyboardKey.Tab,
        };


        public static bool KeyDown(string key)
        {
            return Raylib.IsKeyDown(Dict.GetValueOrDefault(key, KeyboardKey.Null));
        }

        public static bool KeyUp(string key)
        {
            return Raylib.IsKeyUp(Dict.GetValueOrDefault(key, KeyboardKey.Null));
        }

        public static bool KeyPressed(string key)
        {
            return Raylib.IsKeyPressed(Dict.GetValueOrDefault(key, KeyboardKey.Null));
        }

    }
}