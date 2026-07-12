-----------------------------------------
---CLOCK
---Here you'll learn clock stuff


-- Getting the total ticks, seconds and minutes ran since game start
local total_ticks = core.clock.total_ticks()
local total_seconds = core.clock.total_seconds()
local total_minutes = core.clock.total_minutes()

-- Getting the total tick rate
local total_tick_rate = core.clock.tick_rate() -- Probably 16

-- Getting the current tick index
local tick_index = core.clock.current_tick()