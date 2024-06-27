using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blockHandsController : MonoBehaviour
{
    public float speed = 10f;
    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(0, -speed * Time.deltaTime, 0);
    }
}
