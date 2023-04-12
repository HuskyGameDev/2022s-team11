using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject UI;
    private Managers.InputManager _im;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        _im = GameManager.Instance.Get<Managers.InputManager>();
    }


    // Update is called once per frame
    void Update()
    {
        if (_im.GetButtonDown("Cancel"))
        {
            if (GameManager.Instance.IsPaused)
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
        UI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Pause()
    {
        UI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadHub()
    {

            GameManager.Instance.Get<Managers.AudioManager>().Play("Level Entry");
            GameManager.Instance.StartCoroutine(GameManager.Instance.Get<Managers.LevelManager>().LoadScene("HubLevelScene"));
            GameManager.Instance.Resume();

    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadOptions()
    {
        SceneManager.LoadScene("Options");
    }


    /*void Start ()
        {
            pauseMenu.SetActive(false);
        }

    void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        Paused = false;
    }

    void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        Paused = true;
    }*/
}
