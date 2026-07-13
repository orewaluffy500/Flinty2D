--------------------------
---BLOCK METADATA
---Here you'll learn how to have data values per block and how to do some other stuff
---Metadata is just a way to store different values per block such as health, fuel, etc etc


-- Everything will take place in this function

local function block_placed(block_x, block_y)
    -- To handle metadata, First you need to get a block:

    local block_object = core.world.get_block(block_x, block_y)
    -- If a block does not exist or is invalid, It simply becomes an air block which does absolutely nothing

    -- These are the core values of a block object
    -- block_object.x
    -- block_object.y
    -- block_object.name: This is auto retrieved and is air when the block is air.

    -- You can handle metadata like this
    block_object:set_meta('my_mod:meta_value1', 30) -- Meta values can range from integers and strings to lists even object tables
    local meta_value1 = block_object:get_meta('my_mod:meta_value1') -- The name  of the output variable can be anything

    -- However if you get a meta value and it's not there, the game simply returns nil, So if you want a default value in-place of nil:
    local meta_value = block_object:opt_meta('my_mod:meta_value1', 20) -- 20 is the default value here


    -- That's all for meta, Now for the other features of the block object

    block_object:duplicate(4, 2, false) -- Duplicate this block to the destination
    -- The `false` is optional it's simply to specify whether to replace existing blocks
    -- Sidenote: Duplication only copies the type NOT the metadata or other values

    -- You can also move blocks
    block_object:move(3, 8, false) -- Move to 3, 8 (WITH METADATA)
    -- The `false` is optional it's simply to specify whether to replace existing blocks

    if block_object:same_as('soil') then -- Simply a helper over `block.name == target`
        core.logging.debug("My Mod", "Block is soil!")
    end

    block_object:destroy() -- Destroy the block





    -- If you don't want the hassle of needing an entire object just for destroying or getting the name
    -- Simply use the quick world module functions:

    core.world.get_name_of(block_x, block_y) -- Get name or air if empty/invalid
    core.world.place(block_x, block_y, "soil") -- Place a block
    core.world.destroy(block_x, block_y) -- Destroy a block

    -- Advanced place:
    core.world.place(block_x, block_y, 'soil', true, false)
    -- The last two are optional
    -- The first optional argument a.k.a `true` signifies whether we forcefully replace any existing block or not
    -- The second optional argument signifies whether we bypass the `block_updated` logic when placing this block so it doesn't alert nearby blocks

    -- The place and destroy functions also actually return a boolean informing whether the operation failed or succeeded
end

core.event.on_block_event('placed', '*', block_placed)