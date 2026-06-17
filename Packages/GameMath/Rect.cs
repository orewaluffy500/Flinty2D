using System.Drawing;

namespace Flinty.GameMath
{
    public class RectShape(Pos pos, Size size)
    {
        public Pos Pos { get; set; } = pos;
        public Size Size { get; set; } = size;

        public static RectShape from(int x, int y, int w, int h)
        {
            return new(new(x, y), new(w, h));
        }

        public Raylib_cs.Rectangle toRaylib()
        {
            return new(Pos.X, Pos.Y, Size.X, Size.Y);
        }
    }
}