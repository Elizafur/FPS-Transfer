using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UTIL;

public class LightFlicker : MonoBehaviour
{

    [SerializeField]
    public float timeBetweenFlickers = 1f;

    Light l;
    bool disabled = false;
    Timer2 t;

    void Start()
    {
        l = GetComponent<Light>();
        t = new Timer2(timeBetweenFlickers);
    }

    void Update()
    {
        t.tick();

        if (t.isFinished)   {
            disabled = !disabled;
            l.intensity = (disabled) ? 0f : 687023.3f; //value = 22EV;
            t.reset();
        }
    }




}
