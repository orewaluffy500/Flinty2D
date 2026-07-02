local function block_placed(x, y, name)
    local player = core.get_player()

    player:move_cursor(2, 2)
end

core.connect_block_event("any", "placed", block_placed)