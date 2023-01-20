using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerTime
{
    public float[] times = new float[3];

    public PlayerTime(PlayerTime oldPlayerTimes, Timer timer)
    {
        //if there are no previous times set all times to infinity
        if(oldPlayerTimes == null)
        {
            for(int time = 0; time < times.Length; time++)
            {
                times[time] = Mathf.Infinity;
            }
            //set top score as recived score
            times[0] = timer.TotalTime;
            return;
        }
        else
        {
            for (int time = 0; time < times.Length; time++)
            {
                times[time] = oldPlayerTimes.times[time];
            }
        }
        CompareAndOverwriteTimes(timer.TotalTime);
    }

    private void CompareAndOverwriteTimes(float timer)
    {
        for (int time = 0; time < times.Length; time++)
        {
            if(timer < times[time])
            {
                //shift elements down one spot from the end until it reaches insert spot 
                for(int reverseTime = times.Length - 1; reverseTime > time; reverseTime--)
                {
                    times[reverseTime] = times[reverseTime - 1 > 0 ? reverseTime - 1 : 0];
                }

                //inset better time
                times[time] = timer;
                break;
            }
        }
    }
}
