using Flinty.Globals;

namespace Flinty.GameSystem;

public class Clock
{
    public int TickIndex { get; protected set; } = 0;
    public float TickTimer { get; protected set; } = 1f / Preferences.TICK_RATE;
    public bool IsTicking { get; protected set; } = false;
    public int TicksSinceStart = 0;

    public void Update(float dt)
    {
        if (TickTimer > 0)
        {
            TickTimer -= dt;
            IsTicking = false;
            return;
        }

        TickIndex = (TickIndex + 1) % Preferences.TICK_RATE;
        TicksSinceStart++;
        if (TicksSinceStart > Preferences.TOTAL_TICKS_CAP) TicksSinceStart = 0;
        
        TickTimer = 1f / Preferences.TICK_RATE;
        IsTicking = true;
    }
}