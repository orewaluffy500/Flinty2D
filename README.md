<img src="data/Textures/Icon.png" height="220">

# Flinty
2D sandbox with modding

Flinty is a 2D sandbox where the whole purpose is to experiment with mods, The flinty modding system requires no set-up whatso-ever not even any external apps besides a text-editor i guess.

The API is easy too, For example this is all you need to add a ruby block:
```lua
core.registry.register("ruby_mod:ruby", "Textures/ruby.png")
```

Boom! Just drop that into `data/Scripts` and you'll have a black ruby block, It'll be black since if you don't put `ruby.png` in `data/Textures` it won't find any texture and use the default color.

## Learning
I can't write readable or helpful documentation so instead I put examples with comments in `examples/` for people to learn from,
Note: Learn lua for you to understand the codes (It's pretty simple actually.)
