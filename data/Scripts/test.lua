function _Tick()
    if API.terrain.is_empty(2, 2) then
        API.terrain.place(4, 4, "soil")
    else
        API.terrain.destroy(4, 4)
    end
end