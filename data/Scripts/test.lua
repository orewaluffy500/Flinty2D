local GOBBLER = "gobbler:gobbler"
local MOVABLE_DISTANCE = 1

local WEAKNESS = "rock"

local function gobbler_ticked(ti, x, y)
	local ox = core.math.random(-MOVABLE_DISTANCE, MOVABLE_DISTANCE)
	local oy = core.math.random(-MOVABLE_DISTANCE, MOVABLE_DISTANCE)
	
	local block = core.world.get_block(x, y)
	local dest =  core.world.get_block(x + ox, y + oy)
	if dest:same_as(GOBBLER) or dest:same_as(WEAKNESS) then
		return
	end
	
	block:move(x + ox, y + oy, true)
end

core.registry.register(GOBBLER, "", {
	r = 80,
	g = 255,
	b = 80
}, false)

core.event.on_block_event(GOBBLER, "random_tick", gobbler_ticked)