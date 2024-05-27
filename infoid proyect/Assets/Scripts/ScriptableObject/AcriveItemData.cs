using UnityEngine;

[CreateAssetMenu(fileName = "New Active Item", menuName = "Inventory/Active Item")]
public class ActiveItemData : ScriptableObject
{
    public string itemName;
    public string description;
    public GameObject itemPrefab;
}