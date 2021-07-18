using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StoryController : MonoBehaviour
{
    public float maxSubtitleLength = 15f;
    public GameObject subBackGround;
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

        if(currentlyTalking) subBackGround.SetActive(true);
        else subBackGround.SetActive(false);
    }

    public void applySettings(){
        OptionData optData = SaveSystem.LoadOptions();
        if(optData != null){
            float subtitleSize = 25f + (10 * optData.subtitles);
            subtitleBox.fontSize = subtitleSize;
        }
    }
    private void addStoryLines(){
        storyLines = new List<string>();
        // this is where story lines for the plain paper scene start, index numbers 0-14, also added individual numbers in front of lines -Kai
        /*0*/ storyLines.Add("There once was a not-so-ordinary piece of paper named Dinaa. In fact she was quite the extraordinary piece of paper all things considered for Dinaa could think and therefore, she was sure, she is. She existed!" 
                + "In fact, she had already existed for a while now, and in that time she had come to understand a few things. One of which is that she was a piece of paper, although Dinaa did not know what the purpose of a piece of paper was. "
                + "The other thing she realized is that she was blank. And being a blank piece of paper meant that she was undefined, she did not yet have an identity. So to find who she was, who she wanted to be, she had to leave and to do that she had to move." 
                + "But how did one move? Her body, flat as it was, certainly didn't help her in getting anywhere no matter how much she strained to stretch in any direction. Perhaps she was indeed stuck here forever? "
                + "No, she refused such a cruel fate, there had to be a way. She just had to do what she had always done and think about this. And as she thought about it, Dinaa came to a realization, if her shape as she was now rendered her immobile, perhaps she simply had to change it." 
                + "She had seen some things rolling about in the past and perhaps she could do something like that too, if she just rolled her body up.");
        /*1*/ storyLines.Add("Haha! She had done it, she was mobile, unconfined by the fetters that had held her in place until just now, free to go where she pleased! No one could stop her!");
        /*2*/ storyLines.Add("Yet just after overcoming the perhaps greatest hurdle in her way Dinaa found herself faced with the next predicament. Going in a straight line simply wasn’t enough, she had to think of a way to go around bends as well.");
        /*3*/ storyLines.Add("Hmmph, in comparison to her previous troubles this had been nothing. But still she had freed herself even further, surely she could be whatever she wanted to be. She just had to figure that part out still.");
        /*4*/ storyLines.Add("Dinaa was too wide to get through here, but if she didn’t roll up like this she couldn’t move... or could she? Perhaps, she could roll up along her width instead of her length, and squeeze her way through that way?");
        /*5*/ storyLines.Add("Hah! She was doing it, truly no obstacle could stop her.");
        /*6*/ storyLines.Add("Safely on the other side, Dinaa couldn't help but applaud her own genius, surely there was no other as smart as her in this world");
        /*7*/ storyLines.Add("And once again, Dinaa found herself faced with a seemingly insurmountable hurdle. She somehow had to bridge this gap and reach the room past the guitar and the books but there was nothing for her to balance on or anything of the sort to get across. "
                + "Was this really how far she would go? NO! It couldn't end like this, she had to think of a way to do this. "
                + "And so Dinaa futilely racked her brains for a solution, until she saw them. It was the leaves, gently swaying outside the window. She could be like the leaves, could she not? Unfurl her body within the air and gently fall to the other side.");
        /*8*/ storyLines.Add("Yes! It had worked, she was a genius!");
        /*9*/ storyLines.Add("Now, she had to descend the stairs, because she could see her goal below, the open door, a pathway to the great outside, where she would hopefully find who she was.");//change to guidance after 
        /*10*/ storyLines.Add("And just as she had reached the door it was blown shut by a mighty gust of wind. The only other exit left was the window, and to get there she had to ascend the stairs again and then glide to it.");
        /*11*/ storyLines.Add("She had almost made it back, there was just a little left and then she would need to make that last fateful jump.");

        // just some narrative filler to populate the scene with narrative, e.g. falling down from desk and needing to get back on to the bed - Kai
        /*filler 12*/ storyLines.Add("How clumsy of her, she had fallen down and there was no way to get back up to where she was before from here. But perhaps there was a way for her to return to the bed?");
        /*filler 13*/ storyLines.Add("How convenient that this book was folded open. It provided an easy path back to the bed.");
        /*filler 14*/ storyLines.Add("Curious of any secrets that might be lurking in the dark, Dinaa ventured beneath the bed. Yet there was nothing much there, just a hard to see path to the other side of the room.");

        
        // this is where the story lines for the bird scene start, index numbers 15-? -Kai
        /*15*/ storyLines.Add("Now... what did one do as a bird? Perhaps following these red flags would reveal more to her.");
        /*16*/ storyLines.Add("She’d have to lean into the direction she wanted to go, so this time it was down and to the left.");
        /*17*/ storyLines.Add("And as she tilted down Dinaa realized that she became faster when going downwards.");
        /*18*/ storyLines.Add("Ah, these flags seemed to lead back into the tree, where they shifted towards an orange colour. Therefore, the orange flags were next.");
        /*19*/ storyLines.Add("As she flew, Dinaa could not help but think that these flags might be... a test... meant to see if she could be a bird.");
        /*20*/ storyLines.Add("The next part was already in sight, a tire swing, and then the blue flags.");
        /*21*/ storyLines.Add("And she was through, this was easy, now she just had to climb back up into the air, proving that she, just like a bird, could defy gravity itself.");
        /*22*/ storyLines.Add("Alright, these flags led back through the tree again and into the purple flags.");
        /*23*/ storyLines.Add("So far, Dinaa felt like she was doing quite well. If things continued like this, then surely, she really would be able to become a bird." 
                   + "But was this really all that birds did? Following flags? There had to be more, with all this freedom they could do anything.");
        /*24*/ storyLines.Add("Speaking of flags, next up were the yellow flags.");
        /*25*/ storyLines.Add("This seemed like it would be the most difficult path yet. Perhaps, it was even the last.");
        /*26*/ storyLines.Add("So what would she do after following these flags. Perhaps she would fly around freely? That..  actually sounded quite appealing.");
        /*27*/ storyLines.Add("Ah, it seemed like there was one final test left, although perhaps she ought to collect herself first. ");
        /*28*/ storyLines.Add("As she ");
        /*29*/ storyLines.Add("This was it, the final test. Into the canopy in an attempt to break free completely.");

        // Cutscene story lines are below here, mostly just doing this because I don't want to have to deal with reassigning all the storyline values in the bird scene
        // this is the bit where it should transition into illustrations between scenes, they're usually their own scene, though that hasn't been implemented yet apparently - Kai
        /*30*/ storyLines.Add("Well, what now, this was everything wasn’t it? There wasn’t really anywhere else she could go here, was there? And yet, she still hadn't found her identity, her purpose. So perhaps, it laid outside, but how would she get outside…? "
                + "Dinaa did not have an answer to that. It might be possible for her to get outside if she imitated something else again, like she’d done with the leaf, but what could possibly get her out of here… and that is when she saw it, a bird. "
                + "She could become a bird and attain true freedom, just like a bird unbound by the unseen chains of gravity.And so Dinaa folded herself, determined to become free. And then she had done it, she had become a bird");
        // and this is the ending scene
        /*31*/ storyLines.Add("The branches struck her body like whips, unwilling to let her go free. But she was a bird now, she was sure of it, she would overcome this. As she further ascended the leaves grew denser and the branches became thinner but all the more vicious for it." +
                   "And then, she broke free. Free from the grasp of gravity and the claws of the tree, now she was truly free, she really was a bird. " +
                   "And so Dinaa flew through the skies, free and unbound, dancing amongst the clouds, and frolicking with her kin, her flock. Though the happiness would not last, as clouds darkened her friends left one by one. " +
                   "To Dinaa, this made no sense, why would they leave? They were free, what could have them return to the earth? So Dinaa, flew on, although soon the winds picked up and rain started pouring." +
                   "And Dinaa, frightened as she was, began to realise that perhaps this was what had driven her friends away. Something that she had had no chance to predict, they had predicted, so perhaps she was no bird after all?" +
                   "Then, as if to confirm her doubts a bolt of pure power struck her down from above, blindingly bright, and full of searing heat. And then it was gone, but the damage was done." +
                   "Dinaa, unable to keep herself afloat in this rage of the gods, fell down to earth, perhaps a better fate awaited her there?");

        //



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
