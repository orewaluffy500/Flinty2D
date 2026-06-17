using Flinty.GameMath;
using Flinty.GameSystem;

namespace Flinty.World
{
    public class Entity
    {
        public Pos Pos { get; protected set; } = Pos.Zero();


        public virtual void Update(float deltaTime)
        {
            
        }

        public virtual void Draw(EngineRenderer renderer)
        {
            
        }

        public virtual void Tick(int index, Terrain terrain)
        {
            
        }
    }
}