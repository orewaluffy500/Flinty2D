using Raylib_cs;

namespace Flinty.Assets
{
    public static class BlockRegistry
    {
        // Registry of blocks
        public static Dictionary<string, BlockEntry> Registry { get; } = new();
        public static List<string> VisibleRegistry { get; } = new(); // Registry of visible blocks

        // Only add new block
        public static void RegisterNew(string typeName, string path, Color fallbackColor)
        {
            if (Registry.ContainsKey(typeName)) return;

            TextureRegistry.LoadNew(typeName, "data/" + path);
            Registry[typeName] = new(fallbackColor, typeName);

            RefreshVisibleRegistry();
        }

        // (nullable) Get registered entry
        public static BlockEntry? GetBlockEntry(string typeName)
        {
            return Registry[typeName];
        }

        public static void RefreshVisibleRegistry()
        {
            VisibleRegistry.Clear();
            foreach (var pair in Registry)
            {
                if (!pair.Key.StartsWith("_"))
                {
                    VisibleRegistry.Add(pair.Key);
                }
            }
        }
    }



    public class BlockEntry(Color fallbackColor, string textureId)
    {
        public Color FallbackColor { get; set; } = fallbackColor;
        public string TextureId { get; private set; } = textureId;
    }
}