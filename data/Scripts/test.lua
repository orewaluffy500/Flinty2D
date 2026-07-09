core.event.on_game_event("tick", function ()
    local pos = core.player.get_pos()
    core.logging.debug("Mod", pos.x.." "..pos.y)
end)