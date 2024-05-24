using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveItem : MonoBehaviour
{
    // Start is called before the first frame update
    public string itemName;
    public string itemDescription;
    public int itemID;

    public int itemCooldown;
    private bool _isOnCooldown;

    public void ActivateItem()
    {
        if (!_isOnCooldown)
        {
            Debug.Log("Item activated");
            StartCoroutine(Cooldown());
        }
    }

    private IEnumerator Cooldown()
    {
        _isOnCooldown = true;
        yield return new WaitForSeconds(itemCooldown);
        _isOnCooldown = false;
    }
    
}
