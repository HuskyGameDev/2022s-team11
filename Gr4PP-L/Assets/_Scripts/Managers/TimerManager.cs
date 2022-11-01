using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

namespace Managers {
    //Author: than Hohman
    //Author: Nick Zimanski
    //Last Updated:  10/31/2022
    public class TimerManager : Manager
    {
        private Text[] _texts;
        private GameObject _timerCanvas;

        // Plan on using this so timer doesn't tick up during dialogue bits (if any) or room changes
        private bool _timerActive;

        public float CurrentTime = 0;
        public bool BestTimeFollowsCurrent = false;
        public float BestTime = float.MaxValue; // Large Default value so new bestTime is set on level completion. THIS NUMBER SHOULD NEVER BEEN SEEN IN GAME


        void Update()
        {
            if (!_timerActive) return;
            
            CurrentTime = CurrentTime + Time.deltaTime;
            if (BestTimeFollowsCurrent) BestTime = CurrentTime;


        }

        public void Pause() {
            _timerActive = false;
        }

        public void Resume() {
            _timerActive = true;
        }

        public void LevelExit() {
            // Updates BestTime number for current scene
            if (CurrentTime < BestTime)
            {
                PlayerPrefs.SetFloat(SceneManager.GetActiveScene().name, CurrentTime);
            }

            Pause();
        }

        public void LevelEnter() {
            CurrentTime = 0;
            Resume();
        }

        public new void Initialize() {
            base.Initialize();

            CurrentTime = 0;
            Pause();

            //TODO: THIS IS ONLY FOR EDITOR TESTING. REMOVE BEFORE FINAL RELEASE
            PlayerPrefs.DeleteKey(SceneManager.GetActiveScene().name);

            BestTime = PlayerPrefs.GetFloat(SceneManager.GetActiveScene().name, 9999999f);
            BestTimeFollowsCurrent = (BestTime == 9999999f);

            GameManager.updateCallback += Update;
            LevelManager.OnLevelExit += LevelExit;
            LevelManager.OnLevelEnter += LevelEnter;

        }

        public override Manager GetNewInstance()
        {
            return new TimerManager();
        }

        public TimerManager() {
            Initialize();
        }

        public override void Destroy() {
            GameManager.updateCallback -= Update;
            LevelManager.OnLevelExit -= LevelExit;
            LevelManager.OnLevelEnter -= LevelEnter;
        }
    }
}
