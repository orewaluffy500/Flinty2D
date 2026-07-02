namespace Flinty.ModSystem;

public class CoreHelperModule : INativeModule
{
    private readonly string MODULE_NAME = ModEngine.GAME_CORE_MASTER_MODULE_NAME;

    public CoreHelperModule(string moduleName, APIBuilder builder, ModEngine engine) : base(moduleName, builder, engine)
    {
        CreateConnectMethods(); // Create helpers over `NATIVE.event.connect`
        CreateTerrainMethods("world"); // Create helper sover `NATIVE.terrain.place` and some other terrain functions.
        CreateBlockFunctions("world"); // Create stuff like `get_block_at`
        
        new CoreBlockType(this).Create(); // Create table class for blocks
        new CoreLoggingModule(this).Create(); // Create aliases over NATIVE.out
        new CoreClockModule(this).Create(); // Create clock functions like total_ticks
        new CorePlayerModule(this).Create(); // Create player methods.
    }

    private void CreateConnectMethods()
    {
        Engine.Lua.DoString(@$"
        function {MODULE_NAME}.connect(event_group, event_name, func)
            NATIVE.event.connect(event_group .. '.' .. event_name, func)
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

            NATIVE.terrain.place(x, y, name, replace_safe)
        end

        function {MODULE_NAME}.{table_name}.destroy(x, y)
            NATIVE.terrain.destroy(x, y) 
        end

        function {MODULE_NAME}.{table_name}.is_air(x, y)
            return NATIVE.terrain.get_block(x,y) == 'air'
        end

        function {MODULE_NAME}.{table_name}.is_block(x, y, name)
            return NATIVE.terrain.get_block(x,y) == name
        end

        function {MODULE_NAME}.{table_name}.get_block_name(x, y)
            return NATIVE.terrain.get_block(x,y)
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
            o.name = NATIVE.terrain.get_block(x, y)
            return o
        end
        ", "create_block_constructor");
    }
}
