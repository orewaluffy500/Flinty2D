# Setting Up & Making Your First Mod

Flinty is an indie modding engine built around a "no setup mods" philosophy — and it delivers on that promise. There's no installation, no configuration files, and no build steps. To add a mod, just drop a Lua file into `data/Scripts` and Flinty will pick it up automatically.

---

## How Mods Work

Unlike many games that expose modding through their own language or a proprietary scripting system, Flinty uses **Lua**. Lua is a lightweight, beginner-friendly language — and if you're brand new to programming, you might appreciate one of its quirks: arrays start at index `1` instead of `0`.

To create your first mod, paste the following into a new file (e.g. `mymod.lua`) and save it in `data/Scripts`:

```lua
API.out.info("Hello from my mod!")
```

That's it. No boilerplate, no imports, no entry points to define.

---

## Running Your Mod

Open Flinty, then move the game window aside — behind it you'll find a small command prompt window. This is where the `API.out` module sends its output.

If everything is working, you should see:

```
Hello from my mod!
```

(Or whatever message you passed into the function.)

That's the full mod loading pipeline: drop in a file, open the game, see your output.

---

## The Output API

All of Flinty's modding functionality lives under the `API` master module. One of its built-in submodules is `API.out`, which handles sending messages to the command prompt.

It exposes three functions:

- **`API.out.info(text)`** — Prints `text` as a standard informational message.
- **`API.out.warning(cause, text)`** — Prints a warning. The output is colored yellow for visibility.
- **`API.out.error(cause, text)`** — Prints an error. The output is colored red for visibility.

Here's an example using all three:

```lua
API.out.info("Hello!")
API.out.warning("Warning!", "This is a warning — shown in yellow.")
API.out.error("Error", "This is an error — shown in red.")
```

---

## Challenge

Ready to test what you've learned? Try writing a mod that:

1. Prints `"foo"` as an info message **5 times**
2. Then sends an error with a cause and message of your choosing

Give it a go before checking any hints — it's more satisfying that way!