using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace Utility
{
    public class SettingsContainer : MonoBehaviour
    {
        [SerializeField]
        private Slider _mainVolumeSlider;
        public void UpdatePrefs()
        {
            PlayerPrefs.SetFloat("MainVolumeSetting", _mainVolumeSlider.value);
        }
    }
}
