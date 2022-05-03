using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoad : MonoBehaviour
{
    public void Leave()
    {
        Application.Quit();
    }


    public void LoadLevel(string name)
    {
        SceneManager.LoadScene(name);
        Time.timeScale = 1f;
    }

    
}
