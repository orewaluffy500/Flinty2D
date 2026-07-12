-------------------------------
--- LEARNING THE BASIC
--- Hear you'll learn how to show debug logs and handle game events


local function game_started()
    core.logging.debug("My Mod", "Hello from my mod") -- Prints the message with the label `My Mod`
    core.logging.log("My Mod", "Regular Log")
    core.logging.error("My Mod", "Error!") -- Errors appear Red
    core.logging.warning("My Mod", "Warning") -- Warnings appear Yellow

    core.logging.leveled("Custom Level", "My Mod", "Custom Text") -- Show log with a custom level
end

local function game_ended()
    
end

local function game_ticked(tick)
    -- Tick is the index of the game-tick
    -- The game executes 16 game-ticks per second. a.k.a 16 hertz
end

-- Connect our game_started function to when the game starts
core.event.on_game_event('start', game_started)

-- Connect our game_ended to when the game ends
core.event.on_game_event('end', game_ended)

-- Connect our tick function to when the game ticks.
core.event.on_game_event('tick', game_ticked)

-- Run the game and see your logs appear in the console behind the game window.

-- Now go to `terrain_basics.lua`
