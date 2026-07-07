using Flinty.Assets;
using Flinty.World;

namespace Flinty.ModSystem.CoreFeatures;


public class CoreFeatureBlockRegistry : ICoreFeature
{
    public CoreFeatureBlockRegistry(ModEngine engine, string featureName) : base(engine, featureName)
    {
    }

    public override void Build()
    {
        CreateFeatureTable();

        var type = typeof(ModuleRegistry);

        foreach (var method in GetStaticMethodsOf(type))
        {
            RegisterFunction(method.Name, method);
        }
    }






    public static class ModuleRegistry
    {
        public static void create(string block_name, string texture_name, int r = 0, int g = 0, int b = 0, bool canCollide = true)
        {
            BlockRegistry.RegisterNew(block_name, texture_name, new(r, g, b), canCollide);
        }

        public static bool is_registered(string block_name)
        {
            return BlockRegistry.IsRegistered(block_name);
        }

        public static bool can_collide(string block_name)
        {
            BlockEntry? entry = BlockRegistry.GetBlockEntry(block_name);
            return entry != null && entry.CanCollide;
        }

        public static string? get_texture_name(string block_name)
        {
            BlockEntry? entry = BlockRegistry.GetBlockEntry(block_name);
            return entry?.TextureId;
        }
    }
}