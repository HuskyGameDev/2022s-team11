using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

namespace Managers
{
    /** Author: Nick Zimanski
    *   Version: 10/25/22
    */
    public class LevelManager : Manager
    {
        private SceneManager _sm;
        private GameManager _gm;
        private GameObject _loadingScreen;
        private GameObject _uiContainer;

        public Vector2 LevelOrigin { get; private set; }
        public bool IsSceneLoaded { get; private set; }
        public static event Action OnLevelExit;
        public static event Action OnLevelEnter;
        private Vector2 _lastCheckpoint;
        private int _checkpointNum;

        public void Update()
        {

            #region Scene Resetting
            if (_gm.Get<InputManager>().GetButtonDown("reset"))
            {
                ResetLevel();
            }
            #endregion
        }

        public LevelManager()
        {
            Initialize();
        }

        public new void Initialize()
        {
            base.Initialize();
            _gm = GameManager.Instance;
            _loadingScreen = _gm.Parameters.loadingScreen;
            _uiContainer = _gm.Parameters.uiContainer;

            OnLevelEnter = () => { };
            OnLevelExit = () => { };

            GameManager.updateCallback += Update;
        }

        public override Manager GetNewInstance()
        {
            return new LevelManager();
        }

        public override void Destroy()
        {
            GameManager.updateCallback -= Update;
        }

        public void ResetLevel()
        {
            _gm.FindPlayer().ResetPositionToCheckpoint();
            InteractiveManager.RespawnAll();
        }

        private void ReloadScene()
        {
            GameManager.Instance.StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex));
            GameManager.Instance.Initialize();
        }

        public IEnumerator LoadScene(int buildIndex)
        {
            if (buildIndex == 0)
            {
                Debug.Log($"Resetting");
                buildIndex = 1;
            }

            AsyncOperation ao;

            yield return null;

            StartLoadScreen();

            while (SceneManager.sceneCount > 1)
            {
                ao = SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(SceneManager.sceneCount - 1));

                while (!(ao.isDone))
                {
                    yield return null;
                }
            }

            ao = SceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Additive);
            ao.allowSceneActivation = true;

            while (!ao.isDone)
            {
                UpdateLoadProgress(ao.progress);
                yield return null;
            }

            EndLoadScreen(SceneManager.GetSceneAt(SceneManager.sceneCount - 1));
        }

        public IEnumerator LoadScene(string buildName)
        {
            if (buildName == "Base Scene")
            {
                Debug.Log($"Resetting");
                buildName = "HubScene";
            }

            AsyncOperation ao;

            yield return null;

            StartLoadScreen();

            while (SceneManager.sceneCount > 1)
            {
                ao = SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(SceneManager.sceneCount - 1));

                while (!(ao.isDone))
                {
                    yield return null;
                }
            }

            ao = SceneManager.LoadSceneAsync(buildName, LoadSceneMode.Additive);
            ao.allowSceneActivation = true;

            while (!ao.isDone)
            {
                UpdateLoadProgress(ao.progress);
                yield return null;
            }

            _checkpointNum = 0;

            EndLoadScreen(SceneManager.GetSceneAt(SceneManager.sceneCount - 1));
        }

        private void StartLoadScreen()
        {
            OnLevelExit();

            IsSceneLoaded = false;
            _loadingScreen.SetActive(true);
            _uiContainer.SetActive(true);
            _uiContainer.GetComponent<Camera>().enabled = true;
            //Lock Movement inputs
            _gm.Get<Managers.InputManager>().LockInputType("player");
        }

        private void UpdateLoadProgress(float progressPercentage)
        {

        }

        private void EndLoadScreen(Scene newScene)
        {
            OnLevelEnter();

            IsSceneLoaded = true;
            _loadingScreen.SetActive(false);
            _uiContainer.SetActive(false);
            _uiContainer.GetComponent<Camera>().enabled = false;
            //Unlock Movement inputs
            _gm.Get<Managers.InputManager>().UnlockInputType("player");
            SceneManager.SetActiveScene(newScene);
        }

        /// <summary>
        /// Sets the respawn point for the player
        /// </summary>
        /// <param name="pos">The checkpoint to set</param>
        /// <returns>Whether or not the change was successful</returns>
        public void SetCheckpoint(Vector2 pos, int checkpointNum)
        {
            Debug.Log("current _checkpointNum: " + _checkpointNum + ", given checkpointNum: " + checkpointNum);
            if (checkpointNum >= _checkpointNum) {
                Debug.Log("checkpoint changed");
                _checkpointNum = checkpointNum;
                _lastCheckpoint = pos;
            }
        }


        public Vector2 GetLastCheckpoint()
        {
            return _lastCheckpoint;
        }

        /// <summary>
        /// Returns true if the player's last checkpoint is the given checkpoint
        /// </summary>
        /// <param name="pos">The position to check</param>
        /// <returns>true if the position is the player's current checkpoint</returns>
        public bool IsAtCheckpoint(Vector2 pos)
        {
            
            return _lastCheckpoint == pos;
        }

        public void RegisterOrigin(GameObject go)
        {
            RegisterOrigin(go.transform);
        }

        public void RegisterOrigin(Transform tsf)
        {
            RegisterOrigin(tsf.position);
        }

        public void RegisterOrigin(Vector2 pos)
        {
            LevelOrigin = pos;
        }
    }
}