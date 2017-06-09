using System;
using System.Collections.Generic;
using UnityEngine;


public class TimerInfo
{
    public float lastDoTime;
    public float interval;
}



public class Timer
{

    protected TimerInfo info = new TimerInfo();
    protected bool start = false;
    public string Name { get; set; }

    public Timer()
    {
        
    }

    public Timer(float interval)
    {
        SetTimer(interval,false);
    }

    public Timer(float interval, bool readyAtStart)
    {
        SetTimer(interval, readyAtStart);
    }

    public void SetTimer(float interval, bool readyAtStart)
    {
        if (readyAtStart)
        {
            info.lastDoTime = -9999;
        }
        else
        {
            info.lastDoTime = Time.time;
        }
        info.interval = interval;
        start = true;
    }

    public void Do()
    {
        info.lastDoTime = Time.time;
        start = true;
    }

    public bool Ready()
    {
        if (start && Time.time - info.lastDoTime > info.interval)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //直到下次do， 暂停timer；
    public void Pause()
    {
        info.lastDoTime = Time.time;
        start = false;
    }

    //【gyf】
    //需要作为逻辑判断的最后一个，例如
    //if (other.gameObject.layer == PhysicsLayer.PLAYER && laserTimer.Ready2())
    public bool Ready2()
    {
        if (start)
        {
            bool result = Time.time - info.lastDoTime > info.interval;
            
            if (result == true)
            {
                Do();
            }
            return result;
        }
        else
        {
            return false;
        }
    }
	
	public float GetInterval()
	{
		return info.interval;
	}
	
	public float GetTimeSpan()
	{
		if(start)
		{
			return Mathf.Min(Time.time - info.lastDoTime,info.interval);
		}
		return 0;
	}

}
