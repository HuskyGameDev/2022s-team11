using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Audio;

namespace Managers
{
    public class AudioManager : Manager
    {
        private Audio.Sound[] _sounds;
        private Dictionary<string, Sound> _soundSrc;

        public new void Initialize()
        {
            _soundSrc = new Dictionary<string, Sound>();
            GameObject go = GameManager.Instance.gameObject;
            _sounds = GameManager.Instance.Parameters.sounds;

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

        public AudioManager() {
            Initialize();
        }

        public override Manager GetNewInstance()
        {
            return new AudioManager();
        }

        public override void Destroy()
        {
        }

        public void Play(string soundName) {
            Sound s;
            if (_soundSrc.TryGetValue(soundName, out s)) {

                //Adjust the volume for this sound based on the player's audio settings
                s.source.volume = s.volume;

                s.source.Play();
                return;
            }
            
            Debug.LogWarning("Trying to play sound (" + soundName + ") but it can't be found!");
        }
    }
}
