-- EXAMPLE: Terrain Module Stuff
-- Here you will be seeing many terrain stuff.

-- IMPORTANT NOTE: Don't call any functions besides API.registry, API.event and API.out OUTSIDE of functions to avoid errors.

-- Placing blocks
API.terrain.place(x + 1, y, name)

-- Forcefully placing blocks (replace existing)
API.terrain.place(x + 1, y, name, true)

-- Breaking blocks
API.terrain.destroy(x - 1, y)

-- Moving blocks
API.terrain.move(x, y, x, y + 1) -- first two are the current block, second two parameters are the destination

-- Forcefully moving blocks
API.terrain.move(x, y, x, y + 1, true)

-- Setting block metadata (Important)
API.terrain.block_meta(x, y, "mymod:some_meta", 10)

-- Getting block metadata (Important too)
local twenty = API.terrain.block_meta(x, y, "mymod:some_meta")

-- Getting block metadata but safe with default value
local twenty_ = API.terrain.block_opt_meta(x, y, "mymod:some_meta", 20) -- last value in default value