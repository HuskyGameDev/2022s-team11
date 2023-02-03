using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void GameStart()
        {
        StartCoroutine(DelayedStart());
        }

     public void GameOptions()
            {
                   StartCoroutine(Controls());
            }

    public void MenuButton()
        {
                 StartCoroutine(Menu());
        }

    public void GameQuit()
        {
                Application.Quit(); //quits the game
        }

    IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(.1f);
        SceneManager.LoadScene("Base Scene"); //Loads the scene based off of button input
    }
    IEnumerator Controls()
        {
            yield return new WaitForSeconds(0.1f);
           SceneManager.LoadScene("Options"); //Loads the scene based off of button input
        }

    IEnumerator Menu()
    {
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene("Main Menu"); //Loads the scene based off of button input
    }
}
