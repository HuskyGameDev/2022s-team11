using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Audio;

namespace Managers
{
    public class AudioManager : Manager
    {
        public Audio.Sound[] sounds;
        private Dictionary<string, Sound> _soundSrc;

        public AudioManager()
        {
            _soundSrc = new Dictionary<string, Sound>();
            GameObject go = GameManager.Instance.gameObject;

            foreach (var s in sounds)
            {
                s.source = go.AddComponent<AudioSource>();

                s.source.clip = s.clip;
                s.source.loop = s.loop;
                s.source.volume = s.volume;
                s.source.pitch = s.pitch;

                _soundSrc.Add(s.name, s);
            }
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
