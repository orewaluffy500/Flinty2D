using Flinty.Assets;
using NLua;

namespace Flinty.ModSystem.Modules;


public class BlockRegModule : INativeModule
{
    public BlockRegModule(string moduleName, APIBuilder builder, ModEngine engine) : base(moduleName, builder, engine)
    {
    }

    public override void Initialize()
    {
        RegisterObject(ModuleName, typeof(InnerModule));
    }




    public class InnerModule
    {
        public static void register(string id, string texName, LuaTable color, bool canCollide = true, bool hidden = false)
        {
            int red =   Convert.ToInt32(color["r"] ?? 0);
            int green = Convert.ToInt32(color["g"] ?? 0);
            int blue =  Convert.ToInt32(color["b"] ?? 0);
            int alpha = Convert.ToInt32(color["a"] ?? 255);

            BlockRegistry.RegisterNew(id, texName, new(red, green, blue, alpha), canCollide, hidden);
        }

        public static bool is_registered(string id)  => BlockRegistry.IsRegistered(id);
        public static bool is_hidden(string id)  => BlockRegistry.IsHidden(id);
        public static bool can_collide(string id)    => BlockRegistry.CanBlockCollide(id);
        public static string? get_texture(string id) => BlockRegistry.GetTextureNameOf(id);
    }
}
