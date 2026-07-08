using Flinty.GameMath;
using NLua;

namespace Flinty.ModSystem.Modules;

public class MathModule : INativeModule
{
    public MathModule(string moduleName, APIBuilder builder, ModEngine engine) : base(moduleName, builder, engine)
    {
    }

    public override void Initialize()
    {
        RegisterObject(ModuleName, typeof(InnerModule));
    }


    public class InnerModule
    {
        public static int random_offset(int v, bool non_zero = false)
        {
            return non_zero ? Randomizer.GetNonZeroOffset(v) : Randomizer.GetOffset(v);
        }

        public static int random(int min, int max)
        {
            return Randomizer.Random.Next(min, max);
        }
    }
}