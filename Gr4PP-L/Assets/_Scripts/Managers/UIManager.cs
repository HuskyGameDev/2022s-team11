using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

namespace Managers
{
    //Author: Nick Zimanski
    //Last Updated:  4/13/23
    public class UIManager : Manager
    {
        private GameManager _gm;
        private GameObject _uiContainer;
        private GameObject _pauseMenu;
        private GameObject _optionsMenu;
        private GameObject _currentMenu;

        public UIManager()
        {
            Initialize();
        }

        public new void Initialize()
        {
            _gm = GameManager.Instance;
            _uiContainer = _gm.Parameters.uiContainer;
            _pauseMenu = _gm.Parameters.pauseScreen;
            _optionsMenu = _gm.Parameters.optionsScreen;
        }
        public void Resume()
        {
            _uiContainer.SetActive(false);
            _currentMenu.SetActive(false);
        }
        public void Pause()
        {
            _uiContainer.SetActive(true);
            GoTo(_pauseMenu);
        }

        public void NavigateToMenu(String menu)
        {
            switch (menu)
            {
                case "Pause":
                    GoTo(_pauseMenu);
                    break;
                case "Options":
                    GoTo(_optionsMenu);
                    break;
                default:
                    break;
            }
        }

        private void GoTo(GameObject go)
        {
            if (go == _currentMenu)
            {
                _currentMenu.SetActive(true);
                return;
            }

            if (_currentMenu != null)
                _currentMenu.SetActive(false);

            _currentMenu = go;
            _currentMenu.SetActive(true);
        }
        public override void Destroy()
        {

        }

        public override Manager GetNewInstance()
        {
            return new UIManager();
        }
    }
}
