# Blocks (Part 1)

## What Are Blocks?

Flinty is a voxel world, so blocks are pretty much everything. Players build and destroy them freely, and by default the game only comes with two: `rock` and `soil`. But you can add more — that's where things get fun.

---

## Adding a Custom Block

New blocks are registered using the `API.registry` submodule. Here's an example that adds an ice block:

```lua
API.registry.register("ice", "", 130, 130, 255)
```

The three integers at the end are the block's **Red, Green, and Blue** color values. In this case, moderate red and green with full blue gives a nice light blue — fitting for ice.

Once registered, your new block will appear when you cycle through blocks in-game. Simple as that!

---

## Adding a Texture

Solid colors are fine, but textures make blocks really come alive. And it's super simple to set up.

Drop a `.png` file into `data/Textures/` (the filename can be whatever you like), then replace the empty string `""` in your `register` call with the texture path:

```lua
API.registry.register("ice", "Textures/your_texture.png", 130, 130, 255)
```

Note that the path starts inside `data/`, so you don't need to include that part. Boom — your ice block now has a texture!