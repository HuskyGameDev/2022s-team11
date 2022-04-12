using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;


//Author:        Ethan Hohman
//Last Updated:  3/24/2022
public class TimerManager : MonoBehaviour
{
    public Text currentTime;
    public Text bestTime;

    // Plan on using this so timer doesn't tick up during dialogue bits (if any) or room changes
    bool timerActive = true;

    float currentTimeF;
    float bestTimeF = 9999999; // Large Default value so new bestTime is set on level completion. THIS NUMBER SHOULD NEVER BEEN SEEN IN GAME

    void Start()
    {
        bestTimeF = PlayerPrefs.GetFloat(SceneManager.GetActiveScene().name, 9999999);
        currentTime.text = "Current Time: " + currentTimeF.ToString();
        bestTime.text = "Best Time: " + bestTimeF.ToString();
    }

    void Update()
    {
        if (timerActive == true)
        {
            currentTimeF = currentTimeF + Time.deltaTime;
        }
        TimeSpan time = TimeSpan.FromSeconds(currentTimeF);
        currentTime.text = "Current Time: " + time.ToString(@"mm\:ss\:fff");

        // If the Level has not yet been completed, sets the BestTime value to be the same as CurrentTime
        if (bestTimeF == 9999999)
        {
            bestTime.text = "Best Time: " + time.ToString(@"mm\:ss\:fff");
        }

        // Updates BestTime number for current scene
        if (bestTimeF < currentTimeF)
        {
            PlayerPrefs.SetFloat(SceneManager.GetActiveScene().name, currentTimeF);

        }
    }
}
