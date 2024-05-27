using UnityEngine;

public class ActiveItemUse : MonoBehaviour
{
    public Transform shootingPoint;
    public float shootingForce = 10f;

    private PlayerController playerController;
    private InventoryController inventoryController;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        inventoryController = GetComponent<InventoryController>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && inventoryController.activeItems.Count > 0)
        {
            UseActiveItem();
        }
    }

    private void UseActiveItem()
    {
        ActiveItemData activeItemData = inventoryController.activeItems[0];
        GameObject activeItemObject = Instantiate(activeItemData.itemPrefab, shootingPoint.position, Quaternion.identity);
        Rigidbody2D rb = activeItemObject.GetComponent<Rigidbody2D>();
        rb.AddForce(Vector2.down * shootingForce, ForceMode2D.Impulse);
        inventoryController.activeItems.RemoveAt(0);
    }
}