local function placed(x, y, name)
    local block = core.Block.new(x, y, name)

    block:set_meta("ishij", true)
end


local function destroyed(x, y, name)
    local block = core.Block.new(x, y, name)

    core.logging.debug("Mod", block:get_meta("ishij"))
end

core.event.on_block_event("any", "placed", placed)
core.event.on_block_event("any", "breaking", destroyed)