using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OptionData
{
    public float subtitles;
    public float audio;

    public OptionData(float pSubtitles, float pAudio){
        subtitles = pSubtitles;
        audio = pAudio;
    }
}
