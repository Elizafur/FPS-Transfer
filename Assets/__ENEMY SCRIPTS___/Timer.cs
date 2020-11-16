using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{

    [SerializeField]
    public float timerLimit;
    public float timeRemaining;

    public bool isFinished = false;


    // Start is called before the first frame update
    void Start()
    {
        if (!timerLimit.Equals(null))
            timeRemaining = timerLimit;
        else
            timeRemaining = 2f;//default
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFinished)
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

    }

    public void reset()
    {
        timeRemaining = timerLimit;
        isFinished = false;
    }
}
