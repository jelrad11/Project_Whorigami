using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StoryController : MonoBehaviour
{
    public float maxSubtitleLength = 15f;
    public AudioSource audioSource;
    public TMP_Text subtitleBox;
    public List<string> storyLines;
    public List<bool> storyTriggers;
    public List<AudioClip> storyAudio;
    public List<float> storyLineLength;

    public bool audioAvailable = false;

    private bool currentlyTalking = false;
    private List<int> storyPointQueue;
    private void Start()
    {
        addStoryLines();
    }
    private void Update()
    {
        if(!currentlyTalking && storyPointQueue.Count > 0){
            callStory(storyPointQueue[0]);
            storyPointQueue.RemoveAt(0);
        }
    }

    private void addStoryLines(){
        storyLines = new List<string>();
        storyLines.Add("There once was a not-so-ordinary piece of paper named Dinaa. In fact she was quite the extraordinary piece of paper all things considered for Dinaa could think and therefore, she was sure, she is. She existed!");
        storyLines.Add("In fact, she had already existed for a while now, and in that time she had existed she had come to the conclusion that she was meant for greater, as of yet to her unknown, things than gathering dust, here on this [place](shelf/bed/desk/anywhere really). Dinaa had decided that she wanted to leave, no she had to leave, and to do that she had to move.");
        storyLines.Add("But how did one move? Her body, flat as it was, certainly didn't help her in getting anywhere no matter how much she strained to stretch in any direction. Perhaps she was indeed stuck here forever? No, she refused such a cruel fate, there had to be a way. She just had to do what she had always done and think about this.");
        storyLines.Add("And as she thought about it, Dinaa came to a realization, if her shape as she was now rendered her immobile, perhaps she simply had to change it. She had seen some things rolling about in the past and perhaps she could do something like that too, if she just rolled her body up.");
        storyLines.Add("Haha! She had done it, she was mobile, unconfined by the fetters that had held her in place until just a little while ago, free to go where she pleased. No one could stop her.");
        storyLines.Add("But just after overcoming the perhaps greatest hurdle in her way Dinaa found herself faced with another predicament. Going in a straight line simply wasn't enough, she had to think of some way to go around bends as well.");
        storyLines.Add("Hmmph, in comparison to her previous troubles this had been nothing. But still she had freed herself even further, surely she could be whatever she wanted to be.");
        storyLines.Add("Dinaa was too wide to get through here, but if she didn’t roll up like this she couldn’t move... or could she? Perhaps, she could roll up along her width instead of her length, and squeeze her way through that way?");
        storyLines.Add("Hah! She was doing it, truly no obstacle could stop her.");
        storyLines.Add("Safely on the other side, Dinaa couldn't help but applaud her own genius, surely there was no other as smart as her in this world");
        storyLines.Add("And yet again, Dinaa found herslef faced with a seemingly insurmountable hurdle. How was she going to bridge this gap, there was nothing for her to balance on or anything of the sort to get across. Was this really how far she would go? NO! It couldn't end like this, she had to think of a way to do this.");
        storyLines.Add("And as Dinaa futilely racked her brains, she couldn't think of a solution. There was just no way she saw herself crossing this gap, despite it being so easy to see the other side. To Dinaa it seemed that this was no gap at all but a looming mountain instead. ");
        storyLines.Add("And yet just as Dinaa had been on the brink of giving up she saw it, the solution, it was just outside the window. She could be like the leaves, could she not? Unfurl her body within the air and gently fall to the other side.");
        storyLines.Add("Yes! This was working, she would make it, she was a genius!");
        storyLines.Add("And once again, she emerged victorious, she was unstoppable!");
    }

    public void callStory(int storyPoint){
        if(!currentlyTalking){
            float storyPointLength = wordNumber(storyLines[storyPoint]);
            float subtitleSegments =  Mathf.Ceil(storyPointLength/maxSubtitleLength);

            float clipLength = 0f;
            if(audioAvailable) clipLength = storyAudio[storyPoint].length;
            else clipLength = storyLineLength[storyPoint];

            float clipSegmentLength = clipLength/subtitleSegments;

            currentlyTalking = true;
            StartCoroutine(playStory(storyPoint, subtitleSegments, clipSegmentLength));
        }
        else{
            storyPointQueue.Add(storyPoint);
        }
    }

    IEnumerator playStory(int storyPoint, float subtitleSegments, float clipSegmentLength){


        if(audioAvailable) {
            audioSource.clip = storyAudio[storyPoint];
            audioSource.Play();
        }


        List<string> storyStrings = splitString(storyLines[storyPoint], subtitleSegments);
        
        for(int i = 0; i < subtitleSegments; i++){
            writeSubtitles(storyStrings[i]);
            yield return new WaitForSeconds(clipSegmentLength);
        }
        writeSubtitles("");
        currentlyTalking = false;
    }
    private List<string> splitString(string startString, float subtitleSegments){

        
        List<string> returnStrings = new List<string>();
        string tmp = "";
        float ctr = 1f;


        while(ctr <= subtitleSegments){
            float wrdCtr = 0f;           
            tmp = "";
            for(int i = 0; i < startString.Length; i++){
                if(startString[i] == ' ') wrdCtr++;
                if(wrdCtr >= (ctr-1f) * maxSubtitleLength && wrdCtr < ctr * maxSubtitleLength) tmp += startString[i];
            }
            returnStrings.Add(tmp);
            ctr++;
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
