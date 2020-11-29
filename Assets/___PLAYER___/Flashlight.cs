using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    PlayerInput playerInput;
    Light       flashlight;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        flashlight  = GetComponent<Light>();
        flashlight.enabled = false;
    }

    void Update()
    {
        if (playerInput.flashlight)
        {
            flashlight.enabled = !flashlight.enabled;
        }
    }
}
