using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject gun;
    public GunScript gunObj;

    private void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        pauseMenuUI.SetActive(false);
        gun.SetActive(true);
        Time.timeScale = 1f;
        StartCoroutine(gunObj.GunReload());
        GameIsPaused = false;
    }

    void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        pauseMenuUI.SetActive(true);
        gun.SetActive(false);
        Time.timeScale = 0f;
        StopCoroutine(gunObj.GunReload());
        GameIsPaused = true;
    }

    public void LoadMenu()
    {
        Debug.Log("Loading Game...");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
    }

}
