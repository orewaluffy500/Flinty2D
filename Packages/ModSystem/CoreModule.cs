namespace Flinty.ModSystem;

public class CoreHelperModule : IModule
{
    private readonly string MODULE_NAME = ModEngine.GAME_CORE_MASTER_MODULE_NAME;

    public CoreHelperModule(string moduleName, APIBuilder builder, ModEngine engine) : base(moduleName, builder, engine)
    {
        CreateConnectMethods(); // Create helpers over `API.event.connect`
        CreateTerrainMethods("world"); // Create helper sover `API.terrain.place` and some other terrain functions.
        
        CoreBlockType.CreateBlockType(this); // Create table class for blocks
        CreateBlockFunctions("world"); // Create stuff like `get_block_at`
    }

    private void CreateConnectMethods()
    {
        Engine.Lua.DoString(@$"
        function {MODULE_NAME}.connect(event_group, event_name, func)
            API.event.connect(event_group .. '.' .. event_name, func)
        end

        function {MODULE_NAME}.connect_block_event(block_name, event_name, func)
            {MODULE_NAME}.connect('block.' .. block_name, event_name, func)
        end
        ", $"{MODULE_NAME}_helper_connections");
    }



    public void CreateTerrainMethods(string table_name)
    {
        Engine.Lua.DoString(@$"
        {MODULE_NAME}.{table_name} = {{}}

        function {MODULE_NAME}.{table_name}.place(x, y, name, replace)
            local replace_safe = replace or false

            API.terrain.place(x, y, name, replace_safe)
        end

        function {MODULE_NAME}.{table_name}.destroy(x, y)
            API.terrain.destroy(x, y) 
        end

        function {MODULE_NAME}.{table_name}.is_air(x, y)
            return API.terrain.get_block(x,y) == 'air'
        end

        function {MODULE_NAME}.{table_name}.is_block(x, y, name)
            return API.terrain.get_block(x,y) == name
        end

        function {MODULE_NAME}.{table_name}.get_block(x, y)
            return API.terrain.get_block(x,y)
        end
        ", $"{MODULE_NAME}_helper_{table_name}");
    }

    public void CreateBlockFunctions(string worldTableName)
    {
        Engine.Lua.DoString(@$"
        function {MODULE_NAME}.{worldTableName}.get_block_at(x, y)
            local o = setmetatable({{}}, WorldBlock)
            o.x = x
            o.y = y
            o.name = API.terrain.get_block(x, y)
            return o
        end
        ", "create_block_constructor");
    }
}



class CoreBlockType
{
    public static void CreateBlockType(CoreHelperModule module)
    {
        module.Engine.Lua.DoString(@"WorldBlock = {}
        WorldBlock.__index = WorldBlock
        ", "create_block_type");

        CreateMetadata(module);
        CreatePositioning(module);
        CreateChecking(module);
    }

    private static void CreatePositioning(CoreHelperModule module)
    {
        module.Engine.Lua.DoString(@"
        function WorldBlock:move_to(dest_x, dest_y, force)
            if API.terrain.move(self.x, self.y, dest_x, dest_y, force or false) then
                self.x = dest_x
                self.y = dest_y
            end
        end

        function WorldBlock:set_type(type_name)
            API.terrain.set_block(self.x, self.y, type_name)
            self.name = typename
        end

        function WorldBlock:destroy()
            API.terrain.destroy(self.x, self.y)
        end

        function WorldBlock:destroy_if(needed_name)
            if needed_name == self.name then self:destroy() end
        end

        function WorldBlock:destroy_unless(needed_name)
            if needed_name ~= self.name then self:destroy() end
        end
        ", "create_block_positioning");
    }

    private static void CreateChecking(CoreHelperModule module)
    {
        module.Engine.Lua.DoString(@"
        function WorldBlock:is_air()
            return self.name == 'air'
        end

        function WorldBlock:is_in_group(group)
            return string.sub(self.name, 1, #(group .. ':')) == group
        end
        ", "create_block_checks");
    }

    private static void CreateMetadata(CoreHelperModule module)
    {
        module.Engine.Lua.DoString(@"
        function WorldBlock:set_meta(key, value)
            API.terrain.block_meta(self.x, self.y, key, value)
        end

        function WorldBlock:get_meta(key)
            return API.terrain.block_meta(self.x, self.y, key)
        end

        function WorldBlock:has_meta(key)
            return API.terrain.block_has_meta(self.x, self.y, key)
        end

        function WorldBlock:get_meta_or_add(key, value)
            if not API.terrain.block_has_meta(self.x, self.y, key) then
                self:set_meta(key, value)
            end

            return self:get_meta(key)
        end

        function WorldBlock:get_meta_or_default(key, def)
            return API.terrain.block_opt_meta(self.x, self.y, key, def)
        end
        ", "create_block_metadata");
    }
}