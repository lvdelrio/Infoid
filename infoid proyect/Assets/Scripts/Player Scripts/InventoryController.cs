using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerController playerController;
    public List<ActiveItemData> activeItems = new List<ActiveItemData>();
    public List<PassiveItemData> passiveItems = new List<PassiveItemData>();

    public int maxActiveItems = 2;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    public void AddActiveItem(ActiveItemData item)
    {
        if (activeItems.Count >= maxActiveItems)
        {
            activeItems.RemoveAt(0);
        }
        activeItems.Add(item);
    }

    public void AddPassiveItem(PassiveItemData item)
    {
        passiveItems.Add(item);
        playerController.IncreaseStats(item.statBonus);
        Debug.Log("Added passive item: " + item.itemName);
    }
}
