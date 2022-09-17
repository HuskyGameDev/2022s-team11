using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

namespace Managers {
    //Author:        Ethan Hohman
    //Author: Nick Zimanski
    //Last Updated:  9/17/2022
    public class TimerManager : Manager
    {
        private Text[] _texts;
        private GameObject _timerCanvas;

        // Plan on using this so timer doesn't tick up during dialogue bits (if any) or room changes
        private bool _timerActive = true;

        public float CurrentTime = 0;
        public float BestTime = float.MaxValue; // Large Default value so new bestTime is set on level completion. THIS NUMBER SHOULD NEVER BEEN SEEN IN GAME
        
        void Start()
        {
            BestTime = PlayerPrefs.GetFloat(SceneManager.GetActiveScene().name, 9999999);
        }

        void Update()
        {
            if (_timerActive == true)
            {
                CurrentTime = CurrentTime + Time.deltaTime;
            }

            // Updates BestTime number for current scene
            if (CurrentTime < BestTime)
            {
                PlayerPrefs.SetFloat(SceneManager.GetActiveScene().name, CurrentTime);

            }
        }

        public override void OnSceneReset() {Start();}
    }
}
