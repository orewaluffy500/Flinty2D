using System.Numerics;
using Flinty.Globals;
using Raylib_cs;

namespace Flinty.GameMath
{
    public class Pos(int x, int y)
    {
        public int X { get; set; } = x;
        public int Y { get; set; } = y;

        public static Pos Zero()
        {
            return new Pos(0, 0);
        }

        public static Pos One()
        {
            return new Pos(1, 1);
        }

        public Vector2 ToVector()
        {
            return new Vector2(X, Y);
        }

        public static Pos From(int v)
        {
            return new Pos(v, v);
        }

        public static Pos FromGrid(int x, int y)
        {
            return new Pos(x, y).Mul(Preferences.TILE_SIZE);
        }

        public Pos Change(int v, bool inline = false)
        {
            return Change(v, v, inline);
        }

        public Pos Mul(int v, bool inline = false)
        {
            return Mul(v, v, inline);
        }

        public Pos Div(int v, bool inline = false)
        {
            return Div(v, v, inline);
        }


        public Pos Change(int v, int v2, bool inline = false)
        {
            if (inline)
            {
                X += v;
                Y += v2;
                return Zero();
            }

            return new Pos(X + v, Y + v2);
        }

        public Pos Mul(int v, int v2, bool inline = false)
        {
            if (inline)
            {
                X += v;
                Y += v2;
                return Zero();
            }

            return new Pos(X * v, Y * v2);
        }

        public Pos Div(int v, int v2, bool inline = false)
        {
            if (inline)
            {
                X += v;
                Y += v2;
                return Zero();
            }

            return new Pos(X / v, Y / v2);
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

        public void Set(Pos other)
        {
            Set(other.X, other.Y);
        }

        public void Set(Vector2 other)
        {
            Set((int) other.X, (int) other.Y);
        }

        public Size ToSize()
        {
            return new Size(X, Y);
        }
    }


    public class Size(int x, int y) : Pos(x, y)
    {
        public static Size TileSize()
        {
            return new Size(Preferences.TILE_SIZE, Preferences.TILE_SIZE);
        }

        public static Size ChunkSize()
        {
            return new Size(Preferences.CHUNK_SIZE, Preferences.CHUNK_SIZE);
        }

        

        public Pos ToPos()
        {
            return new Pos(X, Y);
        }
    }
}