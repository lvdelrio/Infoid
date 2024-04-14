using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{
    public GameObject player;
    public GameController gameController; 
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
        
        //distanceText.text = distance.ToString("0.00")+" m";
        // int  distance = Mathf.FloorToInt(player.distance);
        distanceText.text = Mathf.FloorToInt(gameController.GetDistance())+" m";
        
    }
}
