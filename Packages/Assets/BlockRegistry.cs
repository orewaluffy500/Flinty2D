using System.Reflection.Metadata.Ecma335;
using Flinty.Globals;
using Raylib_cs;

namespace Flinty.Assets
{
    public static class BlockRegistry
    {
        // Registry of blocks
        public static Dictionary<string, BlockEntry> Registry { get; } = [];
        public static List<string> VisibleRegistry { get; } = []; // Registry of visible blocks


        // Only add new block
        public static void RegisterNew(string typeName, string path, Color fallbackColor, bool canCollide = true, bool hidden = false)
        {
            if (Registry.ContainsKey(typeName)) return;

            TextureRegistry.LoadNew(typeName, "data/" + path);

            var tex = TextureRegistry.GetTexture(typeName);
            if (tex is null || !Raylib.IsTextureValid((Texture2D)tex))
            {
                GameLogger.WarningLog("Block Registry", $"Texture `data/{(path == "" ? "?" : path)}` does not exist.");
            }

            Registry[typeName] = new(fallbackColor, typeName, canCollide, hidden);

            RefreshVisibleRegistry();

            GameLogger.InfoLog("Block Registry", $"Registered {(hidden ? "hidden block" : "block")}: {typeName}");
        }


        // (nullable) Get registered entry
        public static BlockEntry? GetBlockEntry(string typeName) => Registry.TryGetValue(typeName, out var v) ? v : null;
        public static string? GetTextureNameOf(string typeName) => GetBlockEntry(typeName)?.TextureId;
        public static bool CanBlockCollide(string typeName) => GetBlockEntry(typeName)?.CanCollide ?? false;
        public static bool IsHidden(string typeName) => Registry.ContainsKey(typeName) && !VisibleRegistry.Contains(typeName);


        public static void RefreshVisibleRegistry()
        {
            VisibleRegistry.Clear();

            foreach (var pair in Registry)
            {
                if (!pair.Value.Hidden)
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



    public class BlockEntry(Color fallbackColor, string textureId, bool canCollide = true, bool hidden = false)
    {
        public Color FallbackColor { get; set; } = fallbackColor;
        public string TextureId { get; private set; } = textureId;
        public bool CanCollide { get; } = canCollide;
        public bool Hidden { get; } = hidden;
    }
}