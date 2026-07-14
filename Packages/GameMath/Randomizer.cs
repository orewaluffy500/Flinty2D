namespace Flinty.GameMath;


public class Randomizer
{
    public static Random Random { get; } = new();

    public static int GetOffset(int v)
    {
        return Random.Next(-v, v);
    }

    public static int GetNonZeroOffset(int v)
    {
        int offset = GetOffset(v);
        return offset != 0 ? offset : (Random.Next(0, 1) == 0 ? -1 : 1);
    }

    public static Point GetRandomBlockPos(int sx, int sy, int ex, int ey)
    {
        return new(
            Random.Next(sx, ex),
            Random.Next(ex, ey)
        );
    }
}