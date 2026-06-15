using Raylib_cs;

namespace Flinty.Assets
{
    public static class BlockRegistry
    {
        // Registry of blocks
        static Dictionary<string, BlockEntry> registry { get; } = new();

        // Only add new block
        public static void RegisterNew(string typeName, Color fallbackColor)
        {
            if (registry.ContainsKey(typeName)) return;

            registry[typeName] = new(fallbackColor);
        }

        // (nullable) Get registered entry
        public static BlockEntry? GetBlockEntry(string typeName)
        {
            return registry[typeName];
        }
    }



    public class BlockEntry(Color fallbackColor)
    {
        public Color FallbackColor { get; set; } = fallbackColor;
    }
}