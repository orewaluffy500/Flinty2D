-----------------------------------
---PLAYER STUFF
---Here you'll be seeing how to control the player.

local function player_moved(old_x, old_y, new_x, new_y)
    -- Isn't this self-explanatory?
end

local function player_reselected(old_block, new_block)
    -- Fires when the player selects a new block type (Not select a new world block to be clear.)
end

-- Connecting these events
core.event.on_player_event('moved', player_moved)
core.event.on_player_event('reselected', player_reselected)


-- This function is where you'll be seeing all the stuff you can do with the player
local function main()
    -- Moving the player
    core.player.move_by(1, 1) -- Changes the player velocity meaning the player moves by this the next the frame.
    core.player.move_to(4, 2) -- Immediately teleports the player to the desired location.

    -- Handling the player's X and Y
    local player_x = core.player.get_x()
    local player_y = core.player.get_y()

    core.player.set_x(4)
    core.player.set_y(2) -- Pretty easy to understand eh?

    -- Handling the player's meta-data
    core.player.set_meta('my_mod:some_meta', false)
    
    local probably_nil_meta = core.player.get_meta('my_mod:some_meta') -- Will return null if the value isn't found
    local definitely_not_nil_meta = core.player.opt_meta('my_mod:some_meta', false) -- Takes a default value so it's safer (You can still pass null as a default value)

    -- Some helpers for the player
    if core.player.is_at(2, 2) then
        core.logging.debug("My Mod", "Player is at 2, 2")
    end

    -- Player's block selection
    local selected = core.player.get_selected()
    core.player.set_selected('soil')
end