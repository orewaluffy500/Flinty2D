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
        public int get_x() => terrain.Player.Pos.X;
        public int get_y() => terrain.Player.Pos.Y;

        
        public void set_x(int x) => terrain.Player.Pos.X = x;
        public void set_y(int y) => terrain.Player.Pos.Y = y;

        public void move_by(int mx, int my)
        {
            terrain.Player.Velocity.Change(mx, my);
        }

        public void move_to(int dx, int dy)
        {
            terrain.Player.Pos.Set(dx, dy);
        }

        public string get_selected() => terrain.Player.Inventory.GetSelection();
        public void set_selected(string n) => terrain.Player.Inventory.SetSelection(n);

        public bool is_at(int x, int y) => get_x() == x && get_y() == y;
    }
}