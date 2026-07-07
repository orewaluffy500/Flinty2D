-- local function gobbler_ticked(x, y, name)
--     local block = core.world.get_block_at(x, y)

--     local ticksTilMove = block:get_meta_or_add("gobbler:cool", 1)

--     if ticksTilMove > 0 then
--         block:set_meta("gobbler:cool", ticksTilMove - 1)
--         return
--     end

--     block:set_meta("gobbler:cool", 1)

--     local offset_x = math.random(0, 1) == 1 and 1 or -1
--     local offset_y = math.random(0, 1) == 1 and 1 or -1

--     local name = core.world.get_block_name(block.x + offset_x, block.y + offset_y)
--     if name == "gobbler" or name == "rock" then return end

--     block:move_by(offset_x, offset_y, true)
-- end


-- NATIVE.registry.register("gobbler", "", 50, 50, 255)
-- core.connect_block_event("gobbler", "random_tick", gobbler_ticked)