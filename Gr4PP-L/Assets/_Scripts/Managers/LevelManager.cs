using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utility;

namespace Managers {
    /** Author: Nick Zimanski
    * Version: 9/19/22
    */
    public class LevelManager : Manager
    {
        private SceneManager _sm;

        public Vector2 LevelOrigin {get; private set;}

        [SerializeField]
        private Vector2 _lastCheckpoint;


        public static void ResetScene() {
            LoadScene(SceneManager.GetActiveScene().buildIndex);
            GameManager.Instance.OnSceneReset();
        }

        public static void LoadScene(int buildIndex) {
            SceneManager.LoadScene(buildIndex);
        }

        /// <summary>
        /// Sets the respawn point for the player
        /// </summary>
        /// <param name="pos">The checkpoint to set</param>
        /// <returns>Whether or not the change was successful</returns>
        public void SetCheckpoint(Vector2 pos) {
            _lastCheckpoint = pos;
        }


        public Vector2 GetCheckpoint() {
            return _lastCheckpoint;
        }

        /// <summary>
        /// Returns true if the player's last checkpoint is the given checkpoint
        /// </summary>
        /// <param name="pos">The position to check</param>
        /// <returns>true if the position is the player's current checkpoint</returns>
        public bool IsAtCheckpoint(Vector2 pos) {
            return _lastCheckpoint == pos;
        }

        public void RegisterOrigin(GameObject go) {
            LevelOrigin = go.transform.position;
        } 

        // Start is called before the first frame update
        void Start()
        {

            LevelOrigin = transform.position;
            SetCheckpoint(LevelOrigin);
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public override void OnSceneReset() {Start();}

    }
}