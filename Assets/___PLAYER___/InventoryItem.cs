using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TYPE_INVENTORY_ITEM    {
    GUN,
    KEY,
    GRENADE
}

public class InventoryItem : MonoBehaviour
{
    [SerializeField]
    public GameObject   _Item;
    [SerializeField]
    public TYPE_INVENTORY_ITEM _Type;
    public string              _Name;

    [SerializeField]
    Sprite              _InventoryIcon;

    public InventoryItem(GameObject g, TYPE_INVENTORY_ITEM t) 
    {
        if (g != null) 
        {
            _Item = g;
            _Name = _Item.ToString();
            _Type = t;
        }
    }

    public TYPE_INVENTORY_ITEM _GetType()    
    {
        return _Type;
    }
}
