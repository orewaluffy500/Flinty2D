namespace Flinty.ModSystem;

class CoreBlockType : ICoreFeature
{
    public CoreBlockType(CoreHelperModule coreHelperModule) : base(coreHelperModule)
    {
    }

    public override void Create()
    {
        Engine.Lua.DoString(@"WorldBlock = {}
        WorldBlock.__index = WorldBlock
        ", "create_block_type");

        CreateMetadata();
        CreatePositioning();
        CreateChecking();
    }

    private void CreatePositioning()
    {
        Engine.Lua.DoString(@"
        function WorldBlock:move_to(dest_x, dest_y, force)
            if NATIVE.terrain.move(self.x, self.y, dest_x, dest_y, force or false) then
                self.x = dest_x
                self.y = dest_y
                return true
            end
            return false
        end

        function WorldBlock:move_by(change_x, change_y, force)
            return self:move_to(self.x + change_x, self.y + change_y, force)
        end

        function WorldBlock:set_type(type_name)
            NATIVE.terrain.set_block(self.x, self.y, type_name)
            self.name = typename
        end

        function WorldBlock:destroy()
            NATIVE.terrain.destroy(self.x, self.y)
        end

        function WorldBlock:destroy_if(needed_name)
            if needed_name == self.name then self:destroy() end
        end

        function WorldBlock:destroy_unless(needed_name)
            if needed_name ~= self.name then self:destroy() end
        end
        ", "create_block_positioning");
    }

    private void CreateChecking()
    {
        Engine.Lua.DoString(@"
        function WorldBlock:is_air()
            return self.name == 'air'
        end

        function WorldBlock:is_in_group(group)
            return string.sub(self.name, 1, #(group .. ':')) == group
        end
        ", "create_block_checks");
    }

    private void CreateMetadata()
    {
        Engine.Lua.DoString(@"
        function WorldBlock:set_meta(key, value)
            NATIVE.terrain.block_meta(self.x, self.y, key, value)
        end

        function WorldBlock:get_meta(key)
            return NATIVE.terrain.block_meta(self.x, self.y, key)
        end

        function WorldBlock:has_meta(key)
            return NATIVE.terrain.block_has_meta(self.x, self.y, key)
        end

        function WorldBlock:get_meta_or_add(key, value)
            if not NATIVE.terrain.block_has_meta(self.x, self.y, key) then
                self:set_meta(key, value)
            end

            return self:get_meta(key)
        end

        function WorldBlock:get_meta_or_default(key, def)
            return NATIVE.terrain.block_opt_meta(self.x, self.y, key, def)
        end
        ", "create_block_metadata");
    }
}