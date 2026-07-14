using System.Numerics;
using Flinty.GameMath;
using Flinty.GameSystem;
using Flinty.Globals;
using Raylib_cs;

namespace Flinty.World
{
    public class Entity
    {
        public Point Pos { get; protected set; } = Point.Zero();


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




    public class TweenedEntity : Entity
    {
        public PointF VisualPos { get; protected set; } = PointF.Zero();
        public float TweenSpeed = 1;
        public float TweenDistanceFactor = 0;

        public override void Update(float deltaTime)
        {
            if (!VisualPos.NearlyEqual(Pos))
            {
                TweenDistanceFactor += deltaTime * TweenSpeed;
                VisualPos.LerpInplace(Pos.X, Pos.Y, TweenDistanceFactor);
                return;
            }
            
            TweenDistanceFactor = 0;
        }
    }
}