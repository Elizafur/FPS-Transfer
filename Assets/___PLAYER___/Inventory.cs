using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    protected List<InventoryItem> _Items;

    public Inventory() : this(null) {}

    public Inventory(List<InventoryItem> gameObjects)  
    {
        _Items = new List<InventoryItem>();
        if (gameObjects == null) 
            return;
        else 
            foreach (InventoryItem g in gameObjects) 
                if (g != null) _Items.Add(g);
    }

    public bool HasItem(InventoryItem o)    
    {
        return _Items.Contains(o);
    }

    //ex: if (var Item = Inventory.GetItem(o)) ... Item.fuckNuts();
    public InventoryItem GetItem(InventoryItem o) 
    {
        if ( HasItem(o) )
            return _Items[_Items.IndexOf(o)];
        else
            return null;
    }

    public bool AddItem(InventoryItem o) {
        if (o != null)
            _Items.Add(o);
        else return false;

        return true;
    }

    public void printList() {
        foreach (InventoryItem i in _Items) {
            print(i);
        }
    }

    public int length() {
        return _Items.Count;
    }

    public List<InventoryItem> GetList()   
    {
        return _Items;
    }

}
