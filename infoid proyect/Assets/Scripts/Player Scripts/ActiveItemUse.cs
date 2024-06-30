using UnityEngine;

public class ActiveItemUse : MonoBehaviour
{
    public Transform shootingPoint;
    public float shootingForce = 10f;

    [Header("Audio")]
    public AudioClip useItemSound;
    private AudioSource audioSource;

    private PlayerController playerController;
    private InventoryController inventoryController;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        inventoryController = GetComponent<InventoryController>();

        // Get or add an AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
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

        PlayUseItemSound();
    }

    private void PlayUseItemSound()
    {
        if (useItemSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(useItemSound);
        }
        else
        {
            Debug.LogWarning("Use item sound or AudioSource is missing!");
        }
    }
}