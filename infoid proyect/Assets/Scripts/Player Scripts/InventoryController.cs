using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    public PlayerController playerController;
    public List<ActiveItemData> activeItems = new List<ActiveItemData>();
    public List<PassiveItemData> passiveItems = new List<PassiveItemData>();

    public int maxActiveItems = 6;
    public int itemsPerRow = 2;
    public float itemSpacing = 10f;
    public GameObject passiveItemUIPrefab;
    public Transform passiveItemUIParent;

    private Dictionary<string, GameObject> passiveItemUIDictionary = new Dictionary<string, GameObject>();
    private Dictionary<string, int> passiveItemCountDictionary = new Dictionary<string, int>();

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
        UpdatePassiveItemUI(item, 1);
        Debug.Log("Added passive item: " + item.itemName);
    }

    private void UpdatePassiveItemUI(PassiveItemData item, int change)
    {
        if (passiveItemCountDictionary.TryGetValue(item.itemName, out int currentCount))
        {
            currentCount += change;
            passiveItemCountDictionary[item.itemName] = currentCount;
            Text countText = passiveItemUIDictionary[item.itemName].transform.GetChild(1).GetComponent<Text>();
            countText.text = "x" + currentCount.ToString();
        }
        else
        {
            passiveItemCountDictionary[item.itemName] = 1;
            GameObject newItemUI = Instantiate(passiveItemUIPrefab, passiveItemUIParent);
            newItemUI.transform.GetChild(0).GetComponent<Image>().sprite = item.icon;
            newItemUI.transform.GetChild(1).GetComponent<Text>().text = "x1";

            passiveItemUIDictionary.Add(item.itemName, newItemUI);
            RearrangeGrid(); 
        }
    }

    private void RearrangeGrid()
    {
        int index = 0;
        foreach (var itemUI in passiveItemUIDictionary.Values)
        {
            int row = index / itemsPerRow;
            int column = index % itemsPerRow;
            itemUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(column * (itemSpacing + itemUI.GetComponent<RectTransform>().sizeDelta.x), -row * (itemSpacing + itemUI.GetComponent<RectTransform>().sizeDelta.y));
            index++;
        }
    }
}
