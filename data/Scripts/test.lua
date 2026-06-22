API.registry.register("gobbler", "gobbler.png", 20, 100, 20, false)

API.registry.activate("gobbler")

function Event.block_placed(x, y, name)
    if name ~= "gobbler" then return end

    reset_tick_til_move(x, y)
end

function Event.block_tick(x, y, name)
    if name ~= "gobbler" then return end

    local tickTilMove = API.terrain.block_meta(x, y, "tick_till_move")

    if tickTilMove > 0 then
        set_tick_till_move(x, y, tickTilMove - 1)
        return
    end

    local dx = math.random(-1, 1)
    local dy = math.random(-1, 1)

    API.terrain.move(x, y, x + dx, y + dy)
    reset_tick_til_move(x + dx, y + dy)
end

function set_tick_till_move(x, y, v)
    API.terrain.block_meta(x, y, "tick_till_move", v)
end

function reset_tick_til_move(x, y)
    set_tick_till_move(x, y, 4)
end