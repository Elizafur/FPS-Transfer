using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestalInteraction : MonoBehaviour
{

    GameObject player;

    void Start()
    {
        player = GameObject.Find("_PLY_");
    }

    public void RotatePedestal()
    {
        Transform[] children = GetComponentsInChildren<Transform>();
        //transform.SetPositionAndRotation(transform.position, q);
        //rotate around CENTER instead of origin
        this.transform.RotateAround(children[2].GetComponent<Collider>().bounds.center, new Vector3(0,1,0), 90);
    }

    public bool Interactable()
    {
        RaycastHit rh;
        if (Physics.Raycast(transform.position, player.transform.position - transform.position, out rh, 2f))
        {
            if (rh.collider.tag == "Player")
            {
                return true;
            }
        }
        return false;
    }

    void Update()   {
        if (player.GetComponent<PlayerInput>().interact && Interactable()) {
            RotatePedestal();
        }
    }

}
