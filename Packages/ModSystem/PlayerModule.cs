using KeraLua;
using NLua;

namespace Flinty.ModSystem;


public class PlayerModule(string moduleName, APIBuilder builder, ModEngine engine) : IModule(moduleName, builder, engine)
{

    public override void Build()
    {
        base.Build();

        RegisterFunc("selected", nameof(SelectedBlock));
        RegisterFunc("x", nameof(PosX));
        RegisterFunc("y", nameof(PosY));
        RegisterFunc("teleport", nameof(Teleport));
        RegisterFunc("move", nameof(Move));

        // Vararg functions

        Engine.Lua.DoString(@$"
        function {ModuleName}.pos()
            return {ModuleName}.x(), {ModuleName}.y()
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