using Flinty.GameMath;
using Raylib_cs;

namespace Flinty.GameSystem
{
    public static class EngineRenderer
    {
        public static Font SystemFont { get; private set; }

        public static void Init()
        {
            SystemFont = Raylib.LoadFont("data/Fonts/PixelifySans-Regular.ttf");
        }

        public static void Rectangle(RectShape rect, Raylib_cs.Color color)
        {
            var pos = rect.Pos;
            var size = rect.Size;

            Raylib.DrawRectangle(pos.X, pos.Y, size.X, size.Y, color);
        }

        public static void RectangleLines(RectShape rect, Raylib_cs.Color color)
        {
            var pos = rect.Pos;
            var size = rect.Size;

            Raylib.DrawRectangleLines(pos.X, pos.Y, size.X, size.Y, color);
        }

        //                                  Region from texture  Destination rect
        public static void Texture(Texture2D texture, RectShape source, RectShape dest, int rotation)
        {
            Raylib.DrawTexturePro(
                texture, source.toRaylib(), dest.toRaylib(), Point.Zero().ToVector(), rotation, Color.White
            );
        }

        // Just draw text but use 0-100 resizable scale instead of raw 0-width scale.
        public static void TextScale(float x, float y, string text, Color color, int fontSize = 24)
        {
            float scaleX = x / 100;
            float scaleY = y / 100;

            Raylib.DrawTextEx(SystemFont, text, new(scaleX * Raylib.GetScreenWidth(), scaleY * Raylib.GetScreenHeight()), fontSize, 2, color);
        }

        public static void Text(int x, int y, string text, Color color, int fontSize = 24)
        {
            Raylib.DrawTextEx(SystemFont, text, new(x, y), fontSize, 2, color);
        }

        public static void TextOrigined(float x, float y, Point origin, string text, Color color, int fontSize = 24)
        {
            if (origin.IsZero())
            {
                TextScale(x, y, text, color, fontSize);
                return; // perfomance lentlemen
            }

            var size = Raylib.MeasureTextEx(SystemFont, text, fontSize, 2);

            float offsetX = origin.X * (size.X / Raylib.GetScreenWidth() * 100); // Get offset based on origin
            float offsetY = origin.Y * (size.Y / Raylib.GetScreenHeight() * 100); // Same as above


            TextScale(x - offsetX, y - offsetY, text, color, fontSize);
        }


        public static void Line(Point start, Point end, Raylib_cs.Color color)
        {
            Raylib.DrawLineV(start.ToVector(), end.ToVector(), color);
        }
    }



    public class PreColors
    {
        public static readonly Color CalmRed = new(255, 110, 110);
        public static readonly Color CalmGreen = new(110, 255, 110);
        public static readonly Color CalmBlue = new(110, 110, 255);

    }
}