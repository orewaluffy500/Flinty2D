using System.Numerics;
using Flinty.Globals;

namespace Flinty.GameMath
{
    public class Point(int x, int y)
    {
        public int X { get; set; } = x;
        public int Y { get; set; } = y;

        public static Point Zero()
        {
            return new Point(0, 0);
        }

        public static Point One()
        {
            return new Point(1, 1);
        }

        public Vector2 ToVector()
        {
            return new Vector2(X, Y);
        }

        public static Point From(int v)
        {
            return new Point(v, v);
        }

        public static Point FromGrid(int x, int y)
        {
            return new Point(x, y).Mul(Preferences.TILE_SIZE);
        }

        public Point Change(int v, bool inline = false)
        {
            return Change(v, v, inline);
        }

        public Point Mul(int v, bool inline = false)
        {
            return Mul(v, v, inline);
        }

        public Point Div(int v, bool inline = false)
        {
            return Div(v, v, inline);
        }


        public Point Change(int v, int v2, bool inline = false)
        {
            if (inline)
            {
                X += v;
                Y += v2;
                return Zero();
            }

            return new Point(X + v, Y + v2);
        }

        public Point Mul(int v, int v2, bool inline = false)
        {
            if (inline)
            {
                X += v;
                Y += v2;
                return Zero();
            }

            return new Point(X * v, Y * v2);
        }

        public Point Div(int v, int v2, bool inline = false)
        {
            if (inline)
            {
                X += v;
                Y += v2;
                return Zero();
            }

            return new Point(X / v, Y / v2);
        }


        public bool Equals(int v)
        {
            return X == v && Y == v;
        }

        public bool Equals(int v, int v2)
        {
            return Sum() == v + v2;
        }

        public bool IsZero(){
            return Equals(0);
        }

        public void SetZero()
        {
            X = 0;
            Y = 0;
        }

        public int Sum()
        {
            return X + Y;
        }

        public void Set(int a, int b)
        {
            X = a;
            Y = b;
        }

        public void Set(Point other)
        {
            Set(other.X, other.Y);
        }

        public void Set(Vector2 other)
        {
            Set((int) other.X, (int) other.Y);
        }

        public Area AsArea()
        {
            return new Area(X, Y);
        }
    }


    public class PointF(float x, float y) : Point((int)x, (int)y)
    {
        public new float X { get; set; }
        public new float Y { get; set; }
    }


    public class Area(int x, int y) : Point(x, y)
    {
        public static Area TileSize()
        {
            return new Area(Preferences.TILE_SIZE, Preferences.TILE_SIZE);
        }

        public static Area ChunkSize()
        {
            return new Area(Preferences.CHUNK_SIZE, Preferences.CHUNK_SIZE);
        }

        

        public Point AsPoint()
        {
            return new Point(X, Y);
        }
    }
}