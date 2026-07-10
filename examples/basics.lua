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


-- Connect our game_started function to when the game starts
core.event.on_game_event('start', game_started)

-- Run the game and see your logs appear in the console behind the game window.

-- More game events include:
-- `end()`: Runs when game ends
-- `tick(index)`: Runs 16 times a second, Basically your go-to for cooldowns or timers.
-- The tick callback also gives you the index of the tick, So if the function was called at tick 12 you'll get 12 as the index.

-- Now go to `terrain_basics.lua`
