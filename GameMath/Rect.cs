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
    }
}