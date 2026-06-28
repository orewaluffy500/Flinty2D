-- EXAMPLE: Block events

local function placed(block_x, block_y, block_name)
    API.out.info("placed block " .. block_name .. " at " .. block_x .. ", " .. block_y)
    -- Log that we placed S block at x and y
end

local function ticked(block_x, block_y, block_name)
    -- Runs every time this block is ticked
end

local function breaking(block_x, block_y, block_name)
    API.out.info("breaking block " .. block_name .. " at " .. block_x .. ", " .. block_y)
    -- Log that we are breaking S block at x and y

    -- Return false to stop the breaking and keep the block, Or dont return anything or true to continue

    -- By the way, the game checks if any mod stopped the breaking process for this block, if even 1 mod did then that is proritized over multiple continues.
    return true
end

local function updated(updater_x, updater_y, updater_name, block_x, block_y, block_name)
    -- Runs when a block (updater) is placed in a 3x3 area of this block (block)
end


-- Connecting the events
-- For all blocks you use `block.any` otherwise for one single type of block you use `block.<block_name>`

API.event.connect("block.any.placed", placed)
API.event.connect("block.any.tick", ticked)
API.event.connect("block.any.updated", updated)
API.event.connect("block.any.breaking", breaking)