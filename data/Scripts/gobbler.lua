
local function set_tick_till_move(x, y, v)
    API.terrain.block_meta(x, y, "tick_till_move", v)
end

local function reset_tick_til_move(x, y)
    set_tick_till_move(x, y, 4)
end

local function block_placed(x, y, name)
    API.out.info("AAAAAA")
    reset_tick_til_move(x, y)
end

local function block_tick(x, y, name)
    local tickTilMove = API.terrain.block_meta(x, y, "tick_till_move")

    if tickTilMove > 0 then
        set_tick_till_move(x, y, tickTilMove - 1)
        return
    end

    local bx = 1 - math.random(0, 1)
    local by = 1 - math.random(0, 1)

    local block_near = API.terrain.get_block(x + bx, y + by)
    if block_near ~= "air" and block_near ~= "gobbler" and block_near ~= "rock" then
        if math.random(1, 4) == 1 then
            API.terrain.move(x, y, x+bx, y+by)
            reset_tick_til_move(x+bx, y+by)
            return
        end
    end

    local dx = math.random(-1, 1)
    local dy = math.random(-1, 1)

    local ex = x + dx
    local ey = y + dy

    if dx ~= 0 or dy ~= 0 then
        local name = API.terrain.get_block(ex, ey)
        if name ~= "rock" and name ~= "gobbler" then
            API.terrain.move(x, y, ex, ey, true)
        end
    end

    reset_tick_til_move(ex, ey)
end


API.registry.register("gobbler", "gobbler.png", 20, 100, 20, false)

API.registry.activate("gobbler")

API.event.connect("block.gobbler.placed", block_placed)
API.event.connect("block.gobbler.tick", block_tick)
