using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSystem : MonoBehaviour
{
    public class OnTickEventArgs : EventArgs
    {
        public int tick;
    }

    public static event EventHandler<OnTickEventArgs> OnTick;

    public static event EventHandler<OnTickEventArgs> OnTick_5;

    private const float TICK_TIMER_MAX = 1f; // 1 second

    private static int _tick;

    private float tickTimer;

    private void Awake()
    {
        _tick = 0;
        Debug.Log( "[TimeSystem]: Tick Active");
    }

    public static int GetCurrentTime()
    {
        return _tick;
    }
    
    // Update is called once per frame
    private void Update()
    {
        tickTimer += Time.deltaTime;
        if (!(tickTimer >= TICK_TIMER_MAX)) return;

        tickTimer -= TICK_TIMER_MAX;
        _tick++;
        OnTick?.Invoke( this, new OnTickEventArgs { tick = _tick });

        if (_tick % 5 == 0)
        {
            OnTick_5?.Invoke( this, new OnTickEventArgs { tick = _tick });
        }
    }
}
