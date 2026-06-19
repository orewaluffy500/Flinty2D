using KeraLua;
using NLua;

namespace Flinty.ModSystem;


public class PlayerModule(APIBuilder builder, ModEngine engine) : IModule
{
    public APIBuilder Builder => builder;

    public ModEngine Engine => engine;

    public void Build()
    {
        Engine.Lua.NewTable("Player");

        var type = GetType();
        Engine.Lua.RegisterFunction("Player.Selected", this, type.GetMethod(nameof(SelectedBlock)));
        Engine.Lua.RegisterFunction("Player.X", this, type.GetMethod(nameof(PosX)));
        Engine.Lua.RegisterFunction("Player.Y", this, type.GetMethod(nameof(PosY)));
        Engine.Lua.RegisterFunction("Player.Teleport", this, type.GetMethod(nameof(Teleport)));
        Engine.Lua.RegisterFunction("Player.Move", this, type.GetMethod(nameof(Move)));

        // Vararg functions

        Engine.Lua.DoString(@"
        function Player.Pos()
            return Player.X(), Player.Y()
        end");
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

    public int PosX() => Engine.Player.Pos.X;
    public int PosY() => Engine.Player.Pos.Y;

    public void Teleport(int x, int y)
    {
        Engine.Player.Pos.X = x;
        Engine.Player.Pos.Y = y;
    }

    public void Move(int x, int y)
    {
        Engine.Player.Velocity.X += x;
        Engine.Player.Velocity.Y += y;
    }
    
}