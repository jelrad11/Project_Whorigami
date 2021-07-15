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
    private List<int> storyPointQueue = new List<int>();
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
        // this is where story lines for the plain paper scene start, index numbers 0-14, also added individual numbers in front of lines -Kai
        /*0*/ storyLines.Add("There once was a not-so-ordinary piece of paper named Dinaa. In fact she was quite the extraordinary piece of paper all things considered for Dinaa could think and therefore, she was sure, she is. She existed!"
        + "In fact, she had already existed for a while now, and in that time she had existed she had come to the conclusion that she was meant for greater, as of yet to her unknown, things than gathering dust, here on this shelf. Dinaa had decided that she wanted to leave, no she had to leave, and to do that she had to move."
        + "But how did one move? Her body, flat as it was, certainly didn't help her in getting anywhere no matter how much she strained to stretch in any direction. Perhaps she was indeed stuck here forever? No, she refused such a cruel fate, there had to be a way. She just had to do what she had always done and think about this."
        + "And as she thought about it, Dinaa came to a realization, if her shape as she was now rendered her immobile, perhaps she simply had to change it. She had seen some things rolling about in the past and perhaps she could do something like that too, if she just rolled her body up.");
        /*1*/ storyLines.Add("Haha! She had done it, she was mobile, unconfined by the fetters that had held her in place until just a little while ago, free to go where she pleased. No one could stop her.");
        /*2*/ storyLines.Add("But just after overcoming the perhaps greatest hurdle in her way Dinaa found herself faced with another predicament. Going in a straight line simply wasn't enough, she had to think of some way to go around bends as well.");
        /*3*/ storyLines.Add("Hmmph, in comparison to her previous troubles this had been nothing. But still she had freed herself even further, surely she could be whatever she wanted to be.");
        /*4*/ storyLines.Add("Dinaa was too wide to get through here, but if she didn’t roll up like this she couldn’t move... or could she? Perhaps, she could roll up along her width instead of her length, and squeeze her way through that way?");
        /*5*/ storyLines.Add("Hah! She was doing it, truly no obstacle could stop her.");
        /*6*/ storyLines.Add("Safely on the other side, Dinaa couldn't help but applaud her own genius, surely there was no other as smart as her in this world");
        /*7*/ storyLines.Add("And yet again, Dinaa found herslef faced with a seemingly insurmountable hurdle. How was she going to bridge this gap, there was nothing for her to balance on or anything of the sort to get across. Was this really how far she would go? NO! It couldn't end like this, she had to think of a way to do this.");
        /*8*/ storyLines.Add("And as Dinaa futilely racked her brains, she couldn't think of a solution. There was just no way she saw herself crossing this gap, despite it being so easy to see the other side. To Dinaa it seemed that this was no gap at all but a looming mountain instead. ");
        /*9*/ storyLines.Add("And yet just as Dinaa had been on the brink of giving up she saw it, the solution, it was just outside the window. She could be like the leaves, could she not? Unfurl her body within the air and gently fall to the other side.");
        /*10*/ storyLines.Add("Yes! This was working, she would make it, she was a genius!");
        /*11*/ storyLines.Add("And once again, she emerged victorious, she was unstoppable!");
        /*12*/ storyLines.Add("Well, what now, this was everything wasn’t it? There wasn’t really anywhere else she could go here, was there? And yet, she still hadn't found her identity, her purpose. So perhaps, it laid outside, but how would she get outside…?");
        /*13*/ storyLines.Add("Dinaa did not have an answer to that. It might be possible for her to get outside if she imitated something else again, like she’d done with the leaf, but what could possibly get her out of here… and that is when she saw it, a bird.");
        /*14*/ storyLines.Add("She could become a bird and attain true freedom, just like a bird unbound by the unseen chains of gravity.And so Dinaa folded herself, determined to become free. And then she had done it, she had become a bird");

        // this is where the story lines for the bird scene start, index numbers 15-? -Kai
        /*15*/ storyLines.Add("Now... what did one do as a bird? Perhaps following them as well would reveal more to her.");
        /*16*/ storyLines.Add("She’d have to lean into the direction she wanted to go,so this time it was down and to the left.");
        /*17*/ storyLines.Add("And as she titled down Dinaa realized that she became faster when going downwards, this would surely come in handy");
        /*18*/ storyLines.Add("Ah these flags seemed to lead back into the tree, she’d have to slow down if she wanted to get in without any troubles.");
        /*19*/ storyLines.Add("Now where to next, it seemed like the red flags led into the orange flags so that seemed to be the next logical step.");
        /*20*/ storyLines.Add("sdfdsfs");
        /*21*/ storyLines.Add("Lorem Ipsum");
        /*22*/ storyLines.Add("Lorem Ipsum");
        /*23*/ storyLines.Add("Lorem Ipsum");
        /*24*/ storyLines.Add("Lorem Ipsum");
        /*25*/ storyLines.Add("Lorem Ipsum");
        /*26*/ storyLines.Add("Lorem Ipsum");
        /*27*/ storyLines.Add("Lorem Ipsum");
        /*28*/ storyLines.Add("Lorem Ipsum");
        /*29*/ storyLines.Add("Lorem Ipsum");

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
