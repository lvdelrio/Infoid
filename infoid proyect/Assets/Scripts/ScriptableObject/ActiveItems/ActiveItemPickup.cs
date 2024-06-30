using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveItemPickup : MonoBehaviour
{
    public ActiveItemData itemData;
    public float deathTime;
    private float Timer;

    void FixedUpdate()
    {
        Timer += Time.fixedDeltaTime;
        if (Timer >= deathTime)
        {
            Destroy(this.gameObject);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
}