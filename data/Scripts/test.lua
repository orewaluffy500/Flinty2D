local function player_moved(tx, ty, tn, x, y, n)
    API.out.info(n)
end

API.registry.activate("soil")
API.event.connect("block.any.updated", player_moved)