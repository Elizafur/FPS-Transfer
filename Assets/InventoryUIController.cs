using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUIController : MonoBehaviour
{

    [SerializeField]
    private PlayerController player;

    [SerializeField]
    private PlayerInventoryHandler inventory;
    
    private const int rows = 4;
    private const int columns = 3;

    GameObject KeyBlue;
    GameObject KeyRed;
    GameObject KeyYellow;
    UnityEngine.UI.Image Background;

    void Start()
    {
        KeyBlue   = GameObject.Find("KeyBlue");
        KeyRed    = GameObject.Find("KeyRed");
        KeyYellow = GameObject.Find("KeyYellow");
        Background= GetComponent<UnityEngine.UI.Image>();
        KeyBlue.SetActive(false);
        KeyRed.SetActive(false);
        KeyYellow.SetActive(false);
        Background.enabled = false;
    }

    void Update()
    {
        if (player.gamePaused)
        {

            Inventory i = inventory.GetInventory();
            int count = inventory.GetInventory().length();
            int rowCount = 0;
            int columnCount = 0;
            foreach (InventoryItem x in i.GetList())  
            {
                if (rowCount < 5)
                {
                    ++rowCount;
                }
                else 
                {
                    rowCount = 0;
                    ++columnCount;
                }
            }

            Background.enabled = true;
            foreach (InventoryItem x in i.GetList())   {
                if (x._Name == "Blue Keycard")
                    KeyBlue.SetActive(true);
            }

            
        }
        else    {
            Background.enabled = false;
            KeyBlue.SetActive(false);
            KeyRed.SetActive(false);
            KeyYellow.SetActive(false);
        }
    }
}
