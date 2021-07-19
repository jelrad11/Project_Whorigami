using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class Audio_mix : MonoBehaviour
{
    public AudioMixer mixer;
//    public float volume;


    public void SetVolume(float vol) // -80 = silence; 0 = full sound; 0+ = louder than full
    {
        mixer.SetFloat("volume", vol);
    }
}
