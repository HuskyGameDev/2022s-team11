using System.Security.AccessControl;
using System;
using UnityEngine.Audio;
using UnityEngine;

namespace Audio {
    [System.Serializable]
    public class Sound {
        /*
        * A unique identifier for a sound clip
        */
        public String name;

        public AudioClip clip;

        [Range(0f, 1f)]
        public float volume = 1f;

        [Range(.1f, 3f)]
        public float pitch = 1f;

        public SoundType type;

        public bool loop;
        

        [HideInInspector]
        public AudioSource source;
    }
}