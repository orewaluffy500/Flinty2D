namespace Flinty.Globals
{
    public class Preferences
    {
        public static readonly int TILE_SIZE = 16; // Size of every tile
        public static readonly int CHUNK_SIZE = 16; // Size of every chunk

        public static readonly int DRAW_DISTANCE = 4; // Square area of drawing

        public static readonly bool DRAW_CHUNK_DECORS = true; // Chunk decorations such as borders.
        
        public static readonly float STEP_DELAY = 0.1f;

        public static readonly int TICK_RATE = 16;

        public static readonly int TOTAL_TICKS_CAP = 128_000;

        public static readonly int RANDOM_TICKS = 3;

        public static readonly float DEFAULT_TWEEN_SPEED = 0.1f; // Default tween speed for TweenedEntities
    }
}