using System.Drawing;

namespace Flinty.GameMath
{
    public class RectShape(Coordinates pos, Area size)
    {
        public Coordinates Pos { get; set; } = pos;
        public Area Size { get; set; } = size;

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