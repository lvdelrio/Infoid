using UnityEngine;

[CreateAssetMenu(fileName = "New Passive Item", menuName = "Inventory/Passive Item")]
public class PassiveItemData : ScriptableObject
{
    public string itemName;
    public string description;
    public Sprite icon;
    public StatBonus statBonus;
}

[System.Serializable]
public class StatBonus
{
    public float moveSpeedBonus;
    public int maxHealthBonus;
    public int damageBonus;
    public int luckBonus;
}