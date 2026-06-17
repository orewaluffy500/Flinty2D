using Raylib_cs;

namespace Flinty.Assets
{
    public class TextureRegistry
    {
        public static Dictionary<string, Raylib_cs.Texture2D> textures { get; } = new();

        public static void LoadNew(string id, string path)
        {
            if (textures.ContainsKey(id))
            {
                if (Raylib.IsTextureValid(textures[id])) return;
            }

            textures[id] = Raylib.LoadTexture(path);
        }


        public static Raylib_cs.Texture2D? GetTexture(string id)
        {
            if (!textures.ContainsKey(id))
            {
                return null;
            }

            Texture2D texture2D = textures[id];
            return Raylib.IsTextureValid(texture2D) ? texture2D : null;
        }

        public static void UnloadAll()
        {
            foreach (KeyValuePair<string, Texture2D> pair in textures)
            {
                Raylib.UnloadTexture(pair.Value);
            }
        }
    }
}