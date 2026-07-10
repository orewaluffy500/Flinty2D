using Flinty.World;
using NLua;

namespace Flinty.ModSystem.Modules;

public class PlayerModule : INativeModule
{
    public PlayerModule(string moduleName, APIBuilder builder, ModEngine engine) : base(moduleName, builder, engine)
    {
    }

    public override void Initialize()
    {
        RegisterObject(ModuleName, typeof(InnerModule), new InnerModule(Terrain));
    }



    public class InnerModule(Terrain terrain)
    {
        // POSITION
        public int get_x() => terrain.Player.Pos.X;
        public int get_y() => terrain.Player.Pos.Y;

        
        public void set_x(int x) => terrain.Player.Pos.X = x;
        public void set_y(int y) => terrain.Player.Pos.Y = y;

        // VELOCITY
        public void move_by(int mx, int my)
        {
            terrain.Player.Velocity.Change(mx, my);
        }

        // TELEPORT
        public void move_to(int dx, int dy)
        {
            terrain.Player.Pos.Set(dx, dy);
        }

        // SELECTED BLOCK
        public string get_selected() => terrain.Player.Inventory.GetSelection();
        public void set_selected(string n) => terrain.Player.Inventory.SetSelection(n);

        // HELPERS
        public bool is_at(int x, int y) => get_x() == x && get_y() == y;

        // METADATA
        public object? get_meta(string name) => terrain.Player.Metadata.Get(name);
        public object opt_meta(string name, object def) => terrain.Player.Metadata.OptGet(name, def);
        public object? set_meta(string key, object val) => terrain.Player.Metadata.Set(key, val);
    }
}