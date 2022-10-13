using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class setVolume : MonoBehaviour
{
   public AudioMixer mixer;

   public void SetLevel (float slidervalue)

   {
    mixer.SetFloat ("MusicVol", Mathf.Log10 (slidervalue) * 20);
   }

}
