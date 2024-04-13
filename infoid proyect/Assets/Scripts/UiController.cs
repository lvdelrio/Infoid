using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{
    public GameObject player;
    private float currentDistance;

    public float distance;    
    // PlayerController player;
    Text distanceText;
    private void Awake() {
        // player = GameObject.Find("Player").GetComponent<PlayerController>();
        distanceText = GameObject.Find("UiDist").GetComponent<Text>();
    }

    void Start()
    {
        
    }
    void Update()
    {
        currentDistance = Vector2.Distance(player.transform.position, new Vector2(0,0));
        if (currentDistance > distance)
        {
            distance = currentDistance;
        }
        distanceText.text = distance.ToString("0.00")+" m";
        // int  distance = Mathf.FloorToInt(player.distance);
        // distanceText.text = distance+" m";
        
    }
}
