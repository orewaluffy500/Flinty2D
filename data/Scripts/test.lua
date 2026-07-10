core.event.on_clock_event("tick", function (index)
    core.logging.debug("Mod", "tick "..index)
end)

core.event.on_clock_event("second", function (index)
    core.logging.debug("Mod", "second "..index)
end)