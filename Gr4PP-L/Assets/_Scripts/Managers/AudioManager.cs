using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Audio;

namespace Managers
{
    /**
    * Author: Nick Zimanski
    * Version: 11/29/22
    */
    public class AudioManager : Manager
    {
        private Audio.Sound[] _sounds;
        private Dictionary<string, Sound> _soundSrc;
        private const float DEFAULT_VOLUME_VARIANCE = 0.1f;
        private const float DEFAULT_PITCH_VARIANCE = 0.1f;

        public new void Initialize()
        {
            _sounds = GameManager.Instance.Parameters.sounds;
            _soundSrc = new Dictionary<string, Sound>(_sounds.Length);
            GameObject go = GameManager.Instance.gameObject;

            foreach (var s in _sounds)
            {
                s.source = go.AddComponent<AudioSource>();

                s.source.clip = s.clip;
                s.source.loop = s.loop;
                s.source.volume = s.volume;
                s.source.pitch = s.pitch;

                _soundSrc.Add(s.name, s);
            }
        }

        public AudioManager()
        {
            Initialize();
        }

        public override Manager GetNewInstance()
        {
            return new AudioManager();
        }

        public override void Destroy()
        {
        }

        /// <summary>
        /// Plays a sound identified by the given name
        /// </summary>
        /// <param name="soundName">The sound to play</param>
        public void Play(string soundName)
        {
            Sound s;
            if (_soundSrc.TryGetValue(soundName, out s))
            {
                Play(s, s.volume, s.pitch);
                return;
            }

            Debug.LogWarning("Trying to play sound (" + soundName + ") but it can't be found!");
        }

        public void Play(string soundName, float volume, float pitch)
        {
            Sound s;
            if (_soundSrc.TryGetValue(soundName, out s))
            {
                Play(s, volume, pitch);
                return;
            }

            Debug.LogWarning("Trying to play sound (" + soundName + ") but it can't be found!");
        }

        public void Play(Sound sound, float volume, float pitch)
        {
            //Adjust the volume for this sound based on the player's audio settings
            sound.source.volume = volume * getVolumeAdjustment(sound.type);
            sound.source.pitch = pitch;

            sound.source.Play();
        }

        public void PlayVariant(string soundName)
        {
            PlayVariant(soundName, DEFAULT_VOLUME_VARIANCE, DEFAULT_PITCH_VARIANCE);
        }
        public void PlayVariant(string soundName, float volumeVariance, float pitchVariance)
        {
            Sound s;
            if (_soundSrc.TryGetValue(soundName, out s))
            {
                float vv = (float)GameManager.Random.NextDouble() * volumeVariance * 2;
                float pv = (float)GameManager.Random.NextDouble() * pitchVariance * 2;

                Play(s, s.volume - volumeVariance + vv, s.pitch - pitchVariance + pv);
                return;
            }

            Debug.LogWarning("Trying to play sound (" + soundName + ") but it can't be found!");
        }


        public void PlayVariantVolume(string soundName)
        {
            PlayVariantVolume(soundName, DEFAULT_VOLUME_VARIANCE);
        }
        public void PlayVariantVolume(string soundName, float volumeVariance)
        {
            PlayVariant(soundName, volumeVariance, 0f);
        }


        public void PlayVariantPitch(string soundName)
        {
            PlayVariantPitch(soundName, DEFAULT_PITCH_VARIANCE);
        }
        public void PlayVariantPitch(string soundName, float pitchVariance)
        {
            PlayVariant(soundName, 0f, pitchVariance);
        }

        /// <summary>
        /// Returns a number between 0 and 1 representing the player's volume setting for the given SoundType
        /// </summary>
        /// <param name="type">The SoundType to fetch data for</param>
        /// <returns>The player's volume setting</returns>
        private float getVolumeAdjustment(SoundType type)
        {
            float val;
            switch (type)
            {
                case SoundType.SFX:
                    val = PlayerPrefs.GetFloat("SFXVolume");
                    break;
                case SoundType.MUSIC:
                    val = PlayerPrefs.GetFloat("MusicVolume");
                    break;
                default:
                    return PlayerPrefs.GetFloat("MainVolume");
            }
            return val * PlayerPrefs.GetFloat("MainVolume");
        }

        /// <summary>
        /// Stops playing a sound that's currently playing.
        /// </summary>
        /// <param name="soundName">The sound to stop playing</param>
        /// <returns>Whether or not the given sound was successfully stopped</returns>
        public bool Stop(string soundName)
        {
            Sound s;
            if (_soundSrc.TryGetValue(soundName, out s))
            {
                bool isPlaying = s.source.isPlaying;
                s.source.Stop();
                return isPlaying;
            }

            Debug.LogWarning("Trying to stop sound (" + soundName + ") but it can't be found!");
            return false;
        }
    }
}
