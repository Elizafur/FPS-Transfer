namespace UTIL{

using UnityEngine;

public class Timer2
{
    public bool isFinished;
    private float timeRemaining;
    private float timeLimit;
    
    //protected float curTime;

    public Timer2(float f)
    {
        timeLimit = f;
        timeRemaining = timeLimit;
    }


    public void tick()  //Call every Update();
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
        }
        else
        {
            isFinished = true;
        }
    }

    public void reset() 
    {
        timeRemaining = timeLimit;
        isFinished = false;
    }
}
}