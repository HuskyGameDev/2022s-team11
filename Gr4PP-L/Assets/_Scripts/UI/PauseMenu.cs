using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/** Author: Nick Zimanski
*   Author: River Dallas
*   Version: 4/13/23
*
*/
public class PauseMenu : MonoBehaviour
{
    private Managers.InputManager _im;
    [SerializeField]
    private Selectable _resumeButton;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        _im = GameManager.Instance.Get<Managers.InputManager>();
    }

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    private void OnEnable()
    {
        if (GameManager.Instance.Get<Managers.InputManager>().GetControlType() != "Keyboard&Mouse")
            _resumeButton.Select();
    }


    // Update is called once per frame
    void Update()
    {
    }

    public void LoadHub()
    {
        Resume();
        GameManager.Instance.StartCoroutine(GameManager.Instance.Get<Managers.LevelManager>().LoadScene("HubLevelScene"));
    }

    public void Resume()
    {
        GameManager.Instance.Resume();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadOptions()
    {
        GameManager.Instance.Get<Managers.UIManager>().NavigateToMenu("Options");
    }
}
