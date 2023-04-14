using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

namespace Managers
{
    //Author: Ethan Hohman
    //Author: Nick Zimanski
    //Last Updated:  11/30/2022
    public class TimerManager : Manager
    {
        private Text[] _texts;
        private GameObject _timerCanvas;

        // Plan on using this so timer doesn't tick up during dialogue bits (if any) or room changes
        private bool _timerActive;

        /// <summary>
        /// The time, in seconds, since the beginning of the level
        /// </summary>
        public float CurrentTimeSeconds = 0;
        public bool BestTimeFollowsCurrent = false;
        public float BestTime = float.MaxValue; // Large Default value so new bestTime is set on level completion. THIS NUMBER SHOULD NEVER BEEN SEEN IN GAME


        void Update()
        {
            if (!_timerActive) return;

            CurrentTimeSeconds = CurrentTimeSeconds + Time.deltaTime;
            if (BestTimeFollowsCurrent) BestTime = CurrentTimeSeconds;


        }

        public void Pause()
        {
            _timerActive = false;
        }

        public void Resume()
        {
            _timerActive = true;
        }

        public void LevelExit()
        {
            // Updates BestTime number for current scene
            if (CurrentTimeSeconds <= BestTime)
            {
                PlayerPrefs.SetFloat(SceneManager.GetActiveScene().name, CurrentTimeSeconds);
            }

            Pause();
        }

        public void LevelEnter()
        {
            CurrentTimeSeconds = 0;
            Resume();
        }

        public new void Initialize()
        {
            base.Initialize();

            CurrentTimeSeconds = 0;
            Pause();

            //TODO: THIS IS ONLY FOR EDITOR TESTING. REMOVE BEFORE FINAL RELEASE
            //PlayerPrefs.DeleteKey(SceneManager.GetActiveScene().name);

            string sceneName = SceneManager.GetActiveScene().name;
            if (sceneName == "Base Scene") { sceneName = "Tutorial"; }

            BestTime = PlayerPrefs.GetFloat(sceneName, 9999999f);
            Debug.Log(BestTime);
            Debug.Log(SceneManager.GetActiveScene().name);
            BestTimeFollowsCurrent = (BestTime == 9999999f);

            GameManager.updateCallback += Update;
            LevelManager.OnLevelExit += LevelExit;
            LevelManager.OnLevelEnter += LevelEnter;

        }

        public override Manager GetNewInstance()
        {
            return new TimerManager();
        }

        public TimerManager()
        {
            Initialize();
        }

        public override void Destroy()
        {
            GameManager.updateCallback -= Update;
            LevelManager.OnLevelExit -= LevelExit;
            LevelManager.OnLevelEnter -= LevelEnter;
        }
    }
}
