using NLua;

namespace Flinty.ModSystem;


public class PlayerModule(APIBuilder builder, ModEngine engine) : IModule
{
    public APIBuilder Builder => builder;

    public ModEngine Engine => engine;

    public void Build()
    {
        Engine.Lua.NewTable("f_player");

        var type = GetType();
        Engine.Lua.RegisterFunction("f_player.selected", this, type.GetMethod(nameof(SelectedBlock)));
    }

    public string? SelectedBlock(string? block = null)
    {
        if (block != null)
        {
            Engine.Player.Inventory.SetSelection(block);
            return block;
        }

        return Engine.Player.Inventory.GetSelection();
    }
}