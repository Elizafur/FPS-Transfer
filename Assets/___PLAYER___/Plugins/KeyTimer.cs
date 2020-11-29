using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace myUtil{
public class KeyTimer : MonoBehaviour
{
    float startTime;
    float endTime;
    public float elapsedTime;
    [SerializeField]
    public KeyCode key;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(key))  {
            startTime = Time.time;
        }

        if (Input.GetKeyUp(key))    {
            endTime = Time.time;
            elapsedTime = endTime - startTime;
        }



    }


    public bool keyHeldForTime(float f)
    {
        return (elapsedTime >= f);
    }
}
}