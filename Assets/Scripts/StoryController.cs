using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StoryController : MonoBehaviour
{
    public float maxSubtitleLength = 15f;
    public Text subtitleBox;
    public AudioSource audioSource;

    public List<string> storyLines;
    public List<bool> storyTriggers;
    public List<AudioClip> storyAudio;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    { 
        
    }

    private void callStory(int storyPoint){
        float storyPointLength = wordNumber(storyLines[storyPoint]);
        float subtitleSegments =  Mathf.Ceil(storyPointLength/maxSubtitleLength);

        float clipLength = storyAudio[storyPoint].length;
        float clipSegmentLength = clipLength/subtitleSegments;

        StartCoroutine(playStory(storyPoint, subtitleSegments, clipSegmentLength));
    }

    IEnumerator playStory(int storyPoint, float subtitleSegments, float clipSegmentLength){
        audioSource.clip = storyAudio[storyPoint];
        audioSource.Play();

        List<string> storyStrings = splitString(storyLines[storyPoint], subtitleSegments);

        for(int i = 0; i < subtitleSegments; i++){
            writeSubtitles(storyStrings[i]);
            yield return new WaitForSeconds(clipSegmentLength);
        }
    }
    private List<string> splitString(string startString, float subtitleSegments){
        List<string> returnStrings = new List<string>();
        string tmp = "";
        float ctr = 1f;

        while(ctr <= subtitleSegments){
            float wrdCtr = 0f;           
            for(int i = 0; i < startString.Length; i++){
                if(startString[i] == ' ') wrdCtr++;
                if(wrdCtr > (ctr-1f) * maxSubtitleLength && wrdCtr <= ctr * maxSubtitleLength) tmp += startString[i];
            }
            returnStrings.Add(tmp);
        }

        return returnStrings;
    }

    private void writeSubtitles(string subText){
        subtitleBox.text = subText;
    }
    private float wordNumber(string subText){
        float wordNumber = 0f;
        for(int i = 0; i < subText.Length; i++){
            if(subText[i] == ' ') wordNumber++;
        }
        return wordNumber;
    }
}
