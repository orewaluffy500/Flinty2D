--------------------
---ADDING BLOCKS
---Here you'll learn how to add new blocks


-- Adding a simple block with no texture:
core.registry.register("my_mod:my_block", "", {
    r = 255, -- Red value
    g = 100, -- Green value
    b = 100, -- Blue value
    a = 255, -- Alpha value a.k.a transparency
}) -- All of those color values are 0-255 a.k.a unsigned integer 8's

-- Adding a block with no collision
core.registry.register("my_mod:my_ghost_block", "", {
    r = 255,
    g = 100,
    b = 100,
    a = 255,
}, false) -- Add false at the end to signify no collision

-- Adding a block with texture                 TEXTURE PATH
core.registry.register("my_mod:my_png_block", "path/to/texture", {
    r = 255,
    g = 100,
    b = 100,
    a = 255,
})

-- The path of the texture begins at `game directory/data`
-- So if you write `Textures/some_block.png` it's full path is effectively `game directory/data/Textures/some_block.png`

-- Now go to `block_features.lua`
