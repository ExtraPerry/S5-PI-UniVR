using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTickSystem : MonoBehaviour
{
    [SerializeField]
    private GameEvent tickUpdate;

    public const float TICK_TIMER_MAX = 0.2f; // 5 ticks per second.

    private int tick = 0;
    private float tickTimer = 0;

    private void Update()
    {
        tickTimer += Time.deltaTime;
        if (tickTimer >= TICK_TIMER_MAX)
        {
            tickTimer -= TICK_TIMER_MAX;
            tick++;

            if (tickUpdate is not null)
            {
                tickUpdate.Raise(this, tick);
            }    
        }
    }
}
