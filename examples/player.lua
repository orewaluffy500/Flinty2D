-- EXAMPLE: player stuff

local function start()
    ---------------------------------------------------
    
    -- Moving the player
    API.player.move(0, 2) -- This adds to the velocity so the player is moved next frame.

    -- Teleporting the player
    API.player.teleport(3, 5) -- This is done in the same frame

    -- Getting the x and y
    local x_ = API.player.x()
    local y_ = API.player.y()

    -- or
    local x, y = API.player.pos()


    ---------------------------------------------------

    -- Teleporting the hand of the player (a.k.a the cursor)

    API.player.cursor_pos(2, 4)

    -- or use as a getter
    local cursor_x, cursor_y = API.player.cursor_pos()

    ---------------------------------------------------
    
    -- Getting the selected block of the player
    local selected_block = API.player.selected()

    -- Changing the selected block
    API.player.selected("rock")

    
    ---------------------------------------------------
    
    -- Adding metadata to the player
    API.player.meta("is_goofy", true)

    -- Getting metadata from player
    local v1 = API.player.meta("is_goofy") -- will error if meta doesn't exist
    local v2 = API.player.opt_meta("is_goofy", true) -- you pass a default value to avoid an error
end

local function player_moved(old_x, old_y, new_x, new_y)
    -- This callback should be connected to `player.moved`
end

API.event.connect("player.moved", player_moved)