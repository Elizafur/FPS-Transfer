using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportHandler : MonoBehaviour
{
    Collider m_collider;

    void Start()    
    {
        m_collider = GetComponent<Collider>();

    }

    private void OnTriggerEnter(Collider other)   
    {
        if (other.tag == "Player")   
        {
            //SceneManager.LoadScene("Level02");
            //SceneManager.UnloadSceneAsync("SceneOne");
        }
    }
}
