# Events in Flinty

## What Are Events?

Logging messages is a good start, but how do you actually respond to things happening in the game — like when it starts, ends, or updates? That's where events come in.

Events let you run a function whenever something specific occurs in the engine. You connect them using `API.event`, a submodule of the master `API` module.

---

## A Simple Example

The following mod prints a greeting when the game starts and a farewell when it ends:

```lua
function game_start()
    API.out.info("Hello!")
end

function game_end()
    API.out.info("Bye!")
end

API.event.connect("game.start", game_start)
API.event.connect("game.end", game_end)
```

`API.event.connect` takes two arguments: the name of the event to listen for, and the function to call when it fires. The functions themselves can be named anything you like — only the event string (`"game.start"`, `"game.end"`) has to match exactly.

Run the game and you should see `Hello!` in the command prompt on startup. When you close it, `Bye!` will appear briefly. If you miss it, try launching the game directly from a command prompt window — that way the output stays visible even after the game exits.

---

## Ticks and `game.tick`

One of the most useful events is `game.tick`, which fires every game tick.

A **tick** is the engine's unit of per-frame logic. By default, the `TICK_RATE` is `16`, meaning each tick occurs every `1 / 16` seconds (roughly 62.5 times per second). Anything you connect to `game.tick` will run at that frequency, so keep in mind that logging inside it will produce a lot of output fast.