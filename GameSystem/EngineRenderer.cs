using System.Drawing;
using Flinty.GameMath;
using Raylib_cs;

namespace Flinty.GameSystem
{
    public class EngineRenderer
    {
        public Engine Parent { get; }

        public EngineRenderer(Engine parent_)
        {
            Parent = parent_;
        }

        public void Rectangle(RectShape rect, Raylib_cs.Color color)
        {
            var pos = rect.Pos;
            var size = rect.Size;

            Raylib.DrawRectangle(pos.X, pos.Y, size.X, size.Y, color);
        }

        public void RectangleLines(RectShape rect, Raylib_cs.Color color)
        {
            var pos = rect.Pos;
            var size = rect.Size;

            Raylib.DrawRectangleLines(pos.X, pos.Y, size.X, size.Y, color);
        }

        public void Text(int x, int y, String text, Raylib_cs.Color color, int fontSize = 24)
        {
            Raylib.DrawText(text, x, y, fontSize, color);
        }


        public void Line(Pos start, Pos end, Raylib_cs.Color color)
        {
            Raylib.DrawLineV(start.ToVector(), end.ToVector(), color);
        }
    }
}