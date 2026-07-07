using System.Reflection.Metadata.Ecma335;
using Raylib_cs;

namespace Flinty.Assets
{
    public static class BlockRegistry
    {
        // Registry of blocks
        public static Dictionary<string, BlockEntry> Registry { get; } = [];
        public static List<string> VisibleRegistry { get; } = []; // Registry of visible blocks


        // Only add new block
        public static void RegisterNew(string typeName, string path, Color fallbackColor, bool canCollide = true)
        {
            if (Registry.ContainsKey(typeName)) return;

            TextureRegistry.LoadNew(typeName, "data/" + path);
            Registry[typeName] = new(fallbackColor, typeName, canCollide);

            RefreshVisibleRegistry();
        }


        // (nullable) Get registered entry
        public static BlockEntry? GetBlockEntry(string typeName)
        {
            return Registry.TryGetValue(typeName, out var v) ? v : null;
        }

        public static string? GetTextureNameOf(string typeName)
        {
            return GetBlockEntry(typeName)?.TextureId;
        }

        public static bool CanBlockCollide(string typeName)
        {
            return GetBlockEntry(typeName)?.CanCollide ?? false;
        }

        public static void RefreshVisibleRegistry()
        {
            VisibleRegistry.Clear();

            foreach (var pair in Registry)
            {
                if (!pair.Key.StartsWith("hid "))
                {
                    VisibleRegistry.Add(pair.Key);
                }
            }
        }

        public static bool IsRegistered(string name)
        {
            return Registry.ContainsKey(name);
        }
    }



    public class BlockEntry(Color fallbackColor, string textureId, bool canCollide = true)
    {
        public Color FallbackColor { get; set; } = fallbackColor;
        public string TextureId { get; private set; } = textureId;
        public bool CanCollide { get; } = canCollide;
    }
}