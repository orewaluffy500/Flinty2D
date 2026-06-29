-- EXAMPLE: Add new blocks

-- The path by default is `path/to/game/data/`
-- So don't worry about paths too much

-- You give it the block name, texture path (leave empty if no texture needed) and the fallback color
-- The block is rendered with the fallback coloring when the texture is empty or invalid

API.registry.register("my_mod:obsidian", "", 20, 20, 30)

-- Add a block WITH a texture
API.registry.register("my_mod:glowlight", "Textures/mymod/glowlight.png", 0, 0, 0) -- I left fallback color as black

-- Add a hidden block that the player can't use
-- Start block name with `hid ` to indicate its hidden
API.registry.register("hid my_mod:crying_obsidian", "", 30, 50, 30)

-- That's it for registering new blocks.