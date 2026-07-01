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
        RegisterFunc("cursor_x", nameof(CursorX));
        RegisterFunc("cursor_y", nameof(CursorY));
        RegisterFunc("teleport", nameof(Teleport));
        RegisterFunc("move", nameof(Move));
        RegisterFunc("meta", nameof(Meta));
        RegisterFunc("opt_meta", nameof(OptMeta));
        RegisterFunc("has_meta", nameof(HasMeta));

        // Vararg functions

        Engine.Lua.DoString(@$"
        function {ModuleName}.pos()
            return {ModuleName}.x(), {ModuleName}.y()
        end");

        Engine.Lua.DoString(@$"
        function {ModuleName}.cursor_pos(x, y)
            if x ~= nil and y ~= nil then
                {ModuleName}.cursor_x(x)
                {ModuleName}.cursor_y(y)
            end

            return {ModuleName}.cursor_x(), {ModuleName}.cursor_y()
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
    
    public object? Meta(string name, params object[] args)
    {
        if (args.Length > 0)
        {
            Engine.Player.Metadata.Set(name, args[0]);
            return args[0];
        }

        return Engine.Player.Metadata.Get(name);
    }

    public object OptMeta(string name, object default_)
    {
        return Engine.Player.Metadata.OptGet(name, default_);
    }

    public bool HasMeta(string name)
    {
        return Engine.Player.Metadata.Data.ContainsKey(name);
    }

    public int CursorX(params object[] args)
    {
        var cursor = Engine.Player.Cursor;
        if (args.Length > 0){
            object v = args[0];
            if (v is long i){
                cursor.ClampCursor((int)i, cursor.Pos.Y);
            }
        }

        return cursor.Pos.X;
    }

    public int CursorY(params object[] args)
    {
        var cursor = Engine.Player.Cursor;
        if (args.Length > 0){
            object v = args[0];
            if (v is long i){
                cursor.ClampCursor(cursor.Pos.X, (int)i);
            }
        }
        return cursor.Pos.Y;
    }
}