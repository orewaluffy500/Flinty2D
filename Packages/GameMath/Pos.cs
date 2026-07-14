using System.Numerics;
using Flinty.Globals;
using Raylib_cs;

namespace Flinty.GameMath
{
    public class Coordinates(int x, int y)
    {
        public int X { get; set; } = x;
        public int Y { get; set; } = y;

        public static Coordinates Zero()
        {
            return new Coordinates(0, 0);
        }

        public static Coordinates One()
        {
            return new Coordinates(1, 1);
        }

        public Vector2 ToVector()
        {
            return new Vector2(X, Y);
        }

        public static Coordinates From(int v)
        {
            return new Coordinates(v, v);
        }

        public static Coordinates FromGrid(int x, int y)
        {
            return new Coordinates(x, y).Mul(Preferences.TILE_SIZE);
        }

        public Coordinates Change(int v, bool inline = false)
        {
            return Change(v, v, inline);
        }

        public Coordinates Mul(int v, bool inline = false)
        {
            return Mul(v, v, inline);
        }

        public Coordinates Div(int v, bool inline = false)
        {
            return Div(v, v, inline);
        }


        public Coordinates Change(int v, int v2, bool inline = false)
        {
            if (inline)
            {
                X += v;
                Y += v2;
                return Zero();
            }

            return new Coordinates(X + v, Y + v2);
        }

        public Coordinates Mul(int v, int v2, bool inline = false)
        {
            if (inline)
            {
                X += v;
                Y += v2;
                return Zero();
            }

            return new Coordinates(X * v, Y * v2);
        }

        public Coordinates Div(int v, int v2, bool inline = false)
        {
            if (inline)
            {
                X += v;
                Y += v2;
                return Zero();
            }

            return new Coordinates(X / v, Y / v2);
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

        public void Set(Coordinates other)
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


    public class Area(int x, int y) : Coordinates(x, y)
    {
        public static Area TileSize()
        {
            return new Area(Preferences.TILE_SIZE, Preferences.TILE_SIZE);
        }

        public static Area ChunkSize()
        {
            return new Area(Preferences.CHUNK_SIZE, Preferences.CHUNK_SIZE);
        }

        

        public Coordinates AsCoordinate()
        {
            return new Coordinates(X, Y);
        }
    }
}