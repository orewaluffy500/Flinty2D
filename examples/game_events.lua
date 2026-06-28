-- EXAMPLE: Basic game event

local function game_starts()
    API.out.info("This block of code runs when the game begins")
end

local function game_tick()
    -- 1 game tick is a 16th of a second (Can be changed depending on the tick rate which is 16 by default)

    API.out.info("Game just ticked")
end

local function game_end()
    API.out.info("This block of code runs when the game ends")
end

-- Connect out start function to the game start event
API.event.connect("game.start", game_starts)
API.event.connect("game.tick", game_tick)
API.event.connect("game.end", game_end)

-- Look in the command line behind the game for output