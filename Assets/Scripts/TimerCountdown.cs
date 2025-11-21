using UnityEngine;
using System;

public class TimerCountdown
{
    private float intialTime;
    private bool isRunning;
    
    private float Time { get; set; }

    public bool IsRunning { get => isRunning; set => isRunning = value; }


    public TimerCountdown(float intialTime)
    {
        this.intialTime = intialTime;
        IsRunning = false;
    }
    
    public void countdown(float deltaTime)
    {
        if (IsRunning && Time > 0)
        {
            Time -= deltaTime;
        }

        if (IsRunning && Time <= 0)
        {
            Stop();
        }
    }
    
    // Can delegate actions to be preformed on starting and stopping on timer;
    public Action OnTimerStart = delegate { };
    public Action OnTimerStop = delegate { };

    public void Start()
    {
        Time = intialTime;
        if (!IsRunning)
        {
            IsRunning = true;
            OnTimerStart.Invoke();
        }
    }

    public void Stop()
    {
        if (IsRunning)
        {
            IsRunning = false;
            OnTimerStop.Invoke();
        }
    }

}
