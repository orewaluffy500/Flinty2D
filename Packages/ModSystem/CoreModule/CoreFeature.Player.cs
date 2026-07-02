namespace Flinty.ModSystem;

public class CorePlayerModule : ICoreFeature
{
    public CorePlayerModule(CoreHelperModule coreHelperModule) : base(coreHelperModule)
    {
    }

    public override void Create()
    {
        string typeName = $"WorldPlayer";
        Engine.Lua.DoString(@$"
            {typeName} = {{}}
            {typeName}.__index = {typeName}
        ", "create_player_type");

        Engine.Lua.DoString(@$"
            function {ModEngine.GAME_CORE_MASTER_MODULE_NAME}.get_player()
                local o = setmetatable({{}}, WorldPlayer)
                return o
            end
        ", "create_player_constructor");

        CreatePlayerValues(typeName);
        CreatePlayerPositioning(typeName);
    }

    public void CreatePlayerValues(string typeName)
    {
        Engine.Lua.DoString(@$"
            function {typeName}:get_x()
                return NATIVE.player.x()
            end

            function {typeName}:get_y()
                return NATIVE.player.y()
            end

            function {typeName}:get_cursor_x()
                return NATIVE.player.cursor_x()
            end

            function {typeName}:get_cursor_y()
                return NATIVE.player.cursor_y()
            end

            function {typeName}:get_selected_block()
                return NATIVE.player.selected()
            end

            function {typeName}:set_selected_block(block)
                NATIVE.player.selected(block)
            end
        ", "create_player_values");
    }

    public void CreatePlayerPositioning(string typeName){
        Engine.Lua.DoString(@$"
            
            function {typeName}:teleport_cursor(x, y)
                NATIVE.player.cursor_pos(x, y)
            end

            function {typeName}:teleport(x, y)
                NATIVE.player.teleport(x, y)
            end

            function {typeName}:move(dx, dy)
                self:teleport(self:get_x() + dx, self:get_y() + dy)
            end

            function {typeName}:move_cursor(dx, dy)
                self:teleport_cursor(self:get_cursor_x() + dx, self:get_cursor_y() + dy)
            end
            
        ", "create_player_positioning");
    }
}