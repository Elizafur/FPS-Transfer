using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryHandler : MonoBehaviour
{
    [SerializeField]
    protected Inventory         _Inventory;
    [SerializeField]
    protected GunController     _GunController; //Contains gun inventory.
    [SerializeField]
    protected PlayerController  _Player;
    [SerializeField]
    protected PlayerInput       _PI;

    protected float             pickupRange = 5;

    void Start()
    {
        if (_Inventory == null)
            //init empty if left null
            _Inventory = new Inventory(new List<InventoryItem>());

        if (_Player == null)
            throw new System.ArgumentNullException("Player cannot be null");

        //_GunController.gunInventory;

    }

    public Inventory GetInventory()   {
        return _Inventory;
    }

    public bool AddItem(InventoryItem item) 
    {
        return _Inventory.AddItem(item);
    }

    void Update()   
    {
        if (_PI.interact)
        {
            RaycastHit rh;
            LayerMask l = LayerMask.GetMask("InventoryItem");
            if (Physics.Raycast(_Player.transform.position, _Player.transform.forward, out rh, pickupRange, l)) {
                GameObject g = rh.collider.gameObject;
                InventoryItem i = g.GetComponent<InventoryItem>();
                AddItem(i);
                rh.collider.gameObject.SetActive(false);
                GetInventory().printList();
            }
        }
    }



}
