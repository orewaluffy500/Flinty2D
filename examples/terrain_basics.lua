---------------------------
--- TERRAIN BASICS
--- Here you'll how to see when block's are placed, updated and such


local function block_placed(block_x, block_y)
    core.logging.debug("My Mod", "Block was placed at " .. x .. ", " .. y)
    -- Example output: Block was placed at 4, 1
end

local function block_breaking()
    core.logging.debug("My Mod", "Block breaking at " .. x .. ", " .. y)
    -- Example output: Block breaking at 4, 1 

    return true
    -- You can return nothing or true to let the block be broken or return false to stop it from being broken
end

local function block_random_ticked(tick_index, block_x, block_y)
    -- For every loaded chunk, Several random blocks from that chunk are ticked
    -- This is to improve perfomance while also keeping the tick behavior

    core.logging.debug("My Mod", "Block was ticked at index " .. tick_index)
end

local function block_ticked(tick_index, block_x, block_y)
    -- Runs for every block every tick
    -- We advise you to not use this unless you absolutely have to
    -- That's because if hundreds of blocks are loaded at once then they will lag the game out

    core.logging.debug("My Mod", "Block was absolutely ticked at index " .. tick_index)
end

local function block_updated(updater_x, updater_y, target_x, target_y)
    -- Runs when a block is modified in a 3x3 cuboid of this block
    -- Updater is the block that was modified near this one
    -- Target is the current block

    -- Modifications include: placement, destroyment, moving, changing metadata, etc...
    -- Useful because it resembles minecraft's updation behavior
end


-- Connecting all these events?
core.event.on_block_event('placed', '*', block_placed)
core.event.on_block_event('random_tick', '*', block_random_ticked)
core.event.on_block_event('tick', '*', block_ticked)
core.event.on_block_event('breaking', '*', block_breaking)
core.event.on_block_event('updated', '*', block_breaking)

-- You can use a specific block name instead of '*' to only fire the event for that block
-- Or keep using '*' to fire that event for ALL blocks

-- Now go to `adding_blocks.lua`
