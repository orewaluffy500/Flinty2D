using Flinty.Assets;
using Raylib_cs;

namespace Flinty.ModSystem;

public class NativeRegistryModule : INativeModule
{
    public NativeRegistryModule(string moduleName, APIBuilder builder, ModEngine engine) : base(moduleName, builder, engine)
    {
    }

    public override void Build()
    {
        base.Build();

        RegisterFunc("register", nameof(RegisterBlock));
    }


    public void RegisterBlock(string name, string texName, int r, int g, int b, params object[] args)
    {
        if (name.Contains(' ') || texName.Contains(' '))
        {
            Globals.Logging.LuaError("Invalid block or texture name", "Cannot use spaces in block or texture names during registry");
            return;   
        }

        Color color = new(r, g, b);

        bool canCollide = true;
        if (args.Length > 0 && args[0] is bool cc)
        {
            canCollide = cc;
        }

        BlockRegistry.RegisterNew(name, texName, color, canCollide);
    }

}