// Encargado de controlar el estado global del juego

using TMPro;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript Instance { get; private set; }
    private TextMeshProUGUI textMesh;
    private float mettersFallen = 0f;
    public bool lost { get; set; } = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }



    void Start()
    {
        GameObject canvas = GameObject.Find("Canvas");
        textMesh = canvas.GetComponentInChildren<TextMeshProUGUI>();
    }


    void Update()
    {
        if (!lost)
        {
            mettersFallen += Config.FALL_SPEED * Time.deltaTime;
            textMesh.text = Mathf.Floor(mettersFallen).ToString();
        }
    }
}
