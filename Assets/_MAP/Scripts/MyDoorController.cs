using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyDoorController : MonoBehaviour
{
    [SerializeField]
    PlayerInventoryHandler playerInventory;
    bool open = false;

    [SerializeField]
    InventoryItem KEY;

    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }


    public void doOpenClose()  {

        if (KEY == null)
        {
            open=!open;
            anim.SetBool("open", open);
            return;
        }
        Inventory i = playerInventory.GetInventory();

        if (!i.HasItem(KEY)) {
            GetComponent<AudioSource>().Play();
            return;
        }
        open=!open;
        anim.SetBool("open", open);

    }

}
