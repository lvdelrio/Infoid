using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextScene : MonoBehaviour
{
    public int nextSceneIndex;

    public void LoadNextLevel()
    {
        Debug.Log("Cambie de pantalla");
        //SceneManager.LoadScene(nextSceneIndex);
    }
}
