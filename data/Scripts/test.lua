local function placed(x, y, name)
    local block = core.world.get_block_at(x, y)

    if block.name == "soil" then
        block:set_type("rock")
    elseif block.name == 'rock' then
        block:set_type('soil')
    end
end

core.connect_block_event('any', 'placed', placed)