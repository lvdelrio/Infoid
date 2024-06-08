using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KatanaController : MonoBehaviour
{
    private float deathTime = 5f;
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
        if (collision.CompareTag("Enemy"))
        {
            Destroy(this.gameObject);
        }
    }
}


    


