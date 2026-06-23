using Flinty.Assets;
using Raylib_cs;

namespace Flinty.ModSystem;

public class RegistryModule : IModule
{
    public RegistryModule(string moduleName, APIBuilder builder, ModEngine engine) : base(moduleName, builder, engine)
    {
    }

    public override void Build()
    {
        base.Build();

        RegisterFunc("register", nameof(RegisterBlock));
        RegisterFunc("block_active", nameof(BlockActive));
        RegisterFunc("activate", nameof(Activate));
        RegisterFunc("deactivate", nameof(Deactivate));
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

    public bool BlockActive(string name, params object[] args)
    {
        if (args.Length > 0 && args[0] is bool b)
        {
            BlockRegistry.SetActive(name, b, true);
        }

        return BlockRegistry.IsActive(name);
    }

    public void Activate(string name)
    {
        BlockRegistry.SetActive(name, true, true);
    }

    public void Deactivate(string name)
    {
        BlockRegistry.SetActive(name, false, true);
    }
}