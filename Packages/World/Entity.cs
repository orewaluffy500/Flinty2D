using System.Numerics;
using Flinty.GameMath;
using Flinty.GameSystem;
using Raylib_cs;

namespace Flinty.World
{
    public class Entity
    {
        public Pos Pos { get; protected set; } = Pos.Zero();


        public virtual void Update(float deltaTime)
        {
            
        }

        public virtual void Draw()
        {
            
        }

        public virtual void DrawHUD()
        {
            
        }

        public virtual void Tick(int index, Terrain terrain)
        {
            
        }
    }


}