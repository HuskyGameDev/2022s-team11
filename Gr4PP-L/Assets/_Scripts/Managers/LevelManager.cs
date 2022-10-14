using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers {
    /** Author: Nick Zimanski
    * Version: 10/13/22
    */
    public class LevelManager : Manager
    {
        private SceneManager _sm;
        private GameManager _gm;

        public Vector2 LevelOrigin {get; private set;}

        [SerializeField]
        private Vector2 _lastCheckpoint;

        public void Update()
        {

            #region Scene Resetting
            if (_gm.Get<InputManager>().GetButton("Level Reset")) {
                _gm.FindPlayer().ResetPosition();
                LevelManager.ResetScene();
            }
            #endregion
        }

        public LevelManager() {
            base.Initialize();
            _gm = GameManager.Instance;

            GameManager.updateCallback += Update;
        }

        public override void Destroy()
        {
            GameManager.updateCallback -= Update;
        }

        public static void ResetScene() {
            LoadScene(SceneManager.GetActiveScene().buildIndex);
            GameManager.Instance.Initialize();
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
            RegisterOrigin(go.transform);
        } 

        public void RegisterOrigin(Transform tsf) {
            RegisterOrigin(tsf.position);
        } 

        public void RegisterOrigin(Vector2 pos) {
            LevelOrigin = pos;
        } 
    }
}