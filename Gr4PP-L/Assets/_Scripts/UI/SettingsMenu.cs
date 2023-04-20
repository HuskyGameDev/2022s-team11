using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


public class SettingsMenu : MonoBehaviour
{
    [SerializeField]
    private Slider _mainVolumeSlider;
    [SerializeField]
    private Slider _sfxVolumeSlider;
    [SerializeField]
    private Slider _musicVolumeSlider;
    [SerializeField]
    private Toggle _reticleToggle;
    [SerializeField]
    private Selectable _cancelButton;

    void OnEnable()
    {
        _cancelButton.Select();
        _mainVolumeSlider.value = PlayerPrefs.GetFloat("MainVolume", 1f);
        _sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);
        _musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
    }
    public void UpdatePrefs()
    {
        PlayerPrefs.SetFloat("MainVolume", _mainVolumeSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", _sfxVolumeSlider.value);
        PlayerPrefs.SetFloat("MusicVolume", _musicVolumeSlider.value);

        PlayerPrefs.SetInt("ShowReticle", _reticleToggle.isOn ? 1 : 0);
        GameManager.Instance.FindPlayer().Reticle.SetActive(_reticleToggle.isOn);

        Cancel();
    }

    public void Cancel()
    {
        GameManager.Instance.Get<Managers.UIManager>().NavigateToMenu("Pause");
    }
}

