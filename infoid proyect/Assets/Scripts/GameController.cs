using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    public GameObject levelGenerator;
    public GameObject camera;
    public GameObject enemySpawner;
    public GameObject eventSystem;
    public GameObject blockHandPreFab;

    public int score;
    public int currentLevel;
    private float _distance;
    private float _currentDistance;
    private float _totalDistance;
    private float _loopDistance;


    void Start()
    {
        score = 0;
        currentLevel = 0;
        _totalDistance = GameStats.TotalDistance;
        _loopDistance = 0;
    }

    // Update is called once per frame
    void Update()
    {
        _currentDistance = Vector2.Distance(player.transform.position, new Vector2(0,0));
    if (_currentDistance > _distance)
    {
        _distance = _currentDistance;
    }

    // Update loop distance
    if (_distance > _loopDistance)
    {
        // Add the difference to total distance
        _totalDistance += _distance - _loopDistance;
        _loopDistance = _distance;
    }

    }


    public void FinishGame()
    {
        GameStats.TotalDistance = _totalDistance;
        //isntanciar al bloque que te persigue y termina el juego
        Instantiate<GameObject>(blockHandPreFab, player.transform.position + new Vector3(0,30,0), Quaternion.identity);
        //me lo hizo el bot pero funciona
        /* #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif */
    }
    public void finish(){
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    public void EndLevel()
    {
        // if (currentLevel == 0){
        //     eventSystem.GetComponent<LoadNextScene>().LoadNextLevel();
        // }
        GameStats.TotalDistance = _totalDistance;
        _loopDistance = 0;
        //currentLevel++;
        player.GetComponent<PlayerController>().ResetPlayerPosition();
        camera.GetComponent<CameraController>().ResetCameraPosition();
        levelGenerator.GetComponent<LevelGeneratorController>().ResetLevelGenerator();
        enemySpawner.GetComponent<SimpleSpawnerController>().DestroyAllEnemies();

    }

    

    public bool RollLuck(int minNumber, int maxNumber)
    {
        int roll;
        int luck = player.GetComponent<PlayerController>().luck;
        for (int i = 0; i < luck; i++)
        {
            roll = Random.Range(0, maxNumber);
            if (roll > minNumber)
            {
                return true;
            }
        }
        return false;
    }
    public void IncreaseScore(int value)
    {
        score += value;
    }

    public int GetScore()
    {
        return score;
    }

    public float GetDistance()
    {
        return _distance;
    }

    public float GetTotalDistance()
    {
        return _totalDistance;
    }
    
}
