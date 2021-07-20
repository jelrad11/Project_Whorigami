using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StoryController : MonoBehaviour
{
    public GameObject subBackGround;
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
        //The thing below prevents me from playing so I've disabled it for now
        //applySettings();
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
        if(optData != null) {
            subtitleBox.fontSize = 25f + (10 * optData.subtitles);
            audioSource.volume = optData.audio;
        }

    }
    private void addStoryLines(){
        storyLines = new List<string>();
        // this is where story lines for the plain paper scene start, index numbers 0-14, also added individual numbers in front of lines -Kai
        /*0*/ storyLines.Add("There once was a not-so-ordinary piece of paper named Dinaa. In fact, she was quite the extraordinary piece of paper, all things considered for Dinaa could think, and therefore, she was sure she was. She existed! "
                + "In fact, she had already existed for a while now, and in that time, she had come to understand a few things. One of which is that she was a piece of paper, although Dinaa did not know the purpose of a piece of paper. "
                + "The other thing she realized is that she was blank. And being a blank piece of paper meant that she was undefined; she did not yet have an identity. So to find who she was, who she wanted to be, she had to leave and to do that, she had to move. "
                + "But how did one move? Her body, flat as it was, certainly didn't help her in getting anywhere, no matter how much she strained to stretch in any direction. Perhaps she was indeed stuck here forever? "
                + "No, she refused such a cruel fate. There had to be a way. She just had to do what she had always done and think about this. And as she thought about it, Dinaa came to a realization, if her shape as she was now rendered her immobile, she simply had to change it. " 
                + "She had seen some things rolling about in the past, and maybe she could do something like that too if she just rolled up her body.");
        /*1*/ storyLines.Add("Haha! She had done it. She was mobile, unconfined by the fetters that had held her in place until just now, free to go where she pleased! No one could stop her!");
        /*2*/ storyLines.Add("Yet just after overcoming the perhaps greatest hurdle in her way, Dinaa found herself faced with the next predicament. Going in a straight line simply wasn't enough. She had to think of a way to go around bends as well.");
        /*3*/ storyLines.Add("Hmmph, in comparison to her previous troubles, this had been nothing. But still, she had freed herself even further; surely, she could be whatever she wanted to be. She just had to figure that part out still.");
        /*4*/ storyLines.Add("Dinaa was too wide to get through here, but if she didn't roll up like this, she couldn't move... or could she? Perhaps, she could roll up along her width instead of her length and squeeze through that way.");
        /*5*/ storyLines.Add("Hah! She was doing it! Truly, no obstacle could stop her!");
        /*6*/ storyLines.Add("Safely on the other side of the bridge, Dinaa couldn't help but applaud her own genius; surely, there was no other as smart as her in this world.");
        /*7*/ storyLines.Add("And once again, Dinaa found herself faced with a seemingly insurmountable hurdle. She somehow had to bridge this gap and reach the room past the guitar and the books. But there was nothing for her to balance on or anything of the sort to get across. "
                + "Was this really how far she would go? NO! It couldn't end like this. She had to think of a way to do this. "
                + "And so Dinaa futilely racked her brains for a solution until she saw them. It was the leaves, gently swaying outside the window. She could be like the leaves, could she not? Unfurl her body within the air and gently fall to the other side.");
        /*8*/ storyLines.Add("Yes! It had worked! She was a genius!");
        /*9*/ storyLines.Add("Now, she had to descend the stairs because she could see her goal below, the open door, a pathway to the great outside, where she would hopefully find who she was.");
        /*10*/ storyLines.Add("And just as she had reached the door it was blown shut by a mighty gust of wind. The only other exit left was the window, and to get there she had to ascend the stairs again and then glide to it.");
        /*11*/ storyLines.Add("She had almost made it back. There was just a little left, and then she would need to make that last fateful jump.");

        // just some narrative filler to populate the scene with narrative, e.g. falling down from desk and needing to get back on to the bed - Kai
        /*filler 12*/ storyLines.Add("How clumsy of her, she had fallen down, and there was no way to get back up to where she was before from here. But perhaps there was a way for her to return to the bed?");
        /*filler 13*/ storyLines.Add("How convenient that this book was folded open. It provided an easy path back to the bed.");
        /*filler 14*/ storyLines.Add("Curious of any secrets that might be lurking in the dark, Dinaa ventured beneath the bed. Yet there was nothing much there, just a hard-to-see path to the other side of the room.");

        // The line below is alternate version of number 9. that triggers if the player lands on the floor of the second room instead of the stairs.
        /*15*/ storyLines.Add("Now, she just had to get to her goal, the open door, a pathway to the great outside, where she would hopefully find who she was.");

        // this is where the story lines for the bird scene start, index numbers 16-? -Kai
        /*16*/ storyLines.Add("Now... what did one do as a bird? Perhaps following these red flags would reveal more to her.");
        /*17*/ storyLines.Add("She'd have to lean in the direction she wanted to go, so down and to the left.");
        /*18*/ storyLines.Add("And as the air rushed past her, Dinaa felt that this was it, this was what she wanted. Nothing could match this exhilarating sensation. ");
        /*19*/ storyLines.Add("As she flew, Dinaa could not help but think that these flags might be a test... meant to see if she really could be a bird.");
        /*20*/ storyLines.Add("The next part was already in sight, through the tire swing and then the blue flags.");
        /*21*/ storyLines.Add("And then she was through. This was easy. Now she just had to climb back up into the air, proving that she, just like a bird, could defy gravity itself and then follow the next set of flags.");

        /*22*/ storyLines.Add("So far, Dinaa felt like she was doing quite well. If things continued like this, then surely, she really would be able to become a bird. " 
                   + "But was this really all that birds did? Following flags? There had to be more, with all this freedom they could do anything.");
        /*23*/ storyLines.Add("As she passed over the small treehouse that had contained her not too long ago, Dinaa smiled in triumph; what had seemed like a large world to her back then was but a fraction of the world she had now. " 
                   + "And it was all because she had become a bird. Being a bird was truly the greatest.");
        /*24*/ storyLines.Add("And perhaps soon, her world would expand once again when she left this tree behind and set out to conquer the skies. Dinaa could hardly wait for that moment.");
        /*25*/ storyLines.Add("She could see them now, the last flags, the green ones, winding up into the canopy. This was the final test. If she managed to break through, break free, then she would prove herself a true bird.");

        // Too lazy to reassign all the values in the inspector so we just get some placeholders here
        /*26*/
        storyLines.Add("placeholder");
        /*27*/
        storyLines.Add("Placeholder");


        // The following are storylines that play if the player decided not to follow the flag, centred more around free exploration of the tree the three below are play when the player first strays from the flags at any point
        /*28*/
        storyLines.Add("But… why should she follow the flags? She was free, wasn't she? She could fly wherever she wanted? So Dinaa decided to explore the tree on her own terms.");
        /*29*/ storyLines.Add("These flags didn't seem to be revealing any great wisdom to her, so Dinaa decided to explore the tree on her own terms.");
        /*30*/ storyLines.Add("Actually, screw these flags! Dinaa could go wherever she wanted. Who cared if they were some sort of test?");

        //The below are the narrative points that play as the player explores the tree. This particular line is only relevenat if the player doesn't even follow the red flags
        /*31*/ storyLines.Add("This sense of freedom was exhilarating. Nothing could compare to the air rushing by her wings. This was what she had wanted.");
        /*32*/ storyLines.Add("Now, where should she explore first? Perhaps she ought to fly by the treehouse; she had never seen it from the outside after all. "
                   + "Or maybe, she ought to swoop down to see if the tree's lower branches held any secrets.");

        //following line serves to direct player towards lower part of tree
        /*33*/ storyLines.Add("Perhaps she ought to fly by the treehouse; she had never seen it from the outside after all.");
        /*34*/ storyLines.Add("Maybe she should explore the lower branches, see if they held any secrets.");
        // only play if tire swing hasn't been crossed yet
        /*35*/ storyLines.Add("While Dinaa explored the tree, something caught her eye. It was A tire swing, gently swaying in the breeze, basically begging her to fly right through it.");
        /*36*/ storyLines.Add("As she burst through to the other side, Dinaa reveled in the freedom granted by her wings. It was truly remarkable how they allowed her to defy gravity itself, just like a real bird.");


        //Treehouse phrase, only plays if treehouse hasn't been passed yet in flag story line
        /*37*/ storyLines.Add("As she passed by the small treehouse that had contained her not too long ago, Dinaa couldn't help but smile triumphantly. What had seemed like a large world to her back then was but a fraction of the world she had now. "
                    + "And it was all because she had become a bird. This truly was who she wanted to be.");
        /*38*/ storyLines.Add("Well, where to now? She could spend some more time flying through the tree, but... there didn't really seem to be anything left. So, maybe, it was time for her to leave?");



        // Cutscene story lines are below here, mostly just doing this because I don't want to have to deal with reassigning all the storyline values in the bird scene
        // this is the bit where it should transition into illustrations between scenes, they're usually their own scene, though that hasn't been implemented yet apparently - Kai
        /*39*/
        storyLines.Add("Well, what now, this was everything wasn’t it? There wasn’t really anywhere else she could go here, was there? And yet, she still hadn't found her identity, her purpose. So perhaps, it laid outside, but how would she get outside…? "
                    + "Dinaa did not have an answer to that. It might be possible for her to get outside if she imitated something else again, like she’d done with the leaf, but what could possibly get her out of here… and that is when she saw it, a bird. "
                    + "She could become a bird and attain true freedom, just like a bird unbound by the unseen chains of gravity.And so Dinaa folded herself, determined to become free. And then she had done it, she had become a bird");

        // and this is the ending scene
        /*40*/ storyLines.Add("The branches struck her body like whips, unwilling to let her go free. But she was a bird now, she was sure of it, she would overcome this. As she further ascended the leaves grew denser and the branches became thinner but all the more vicious for it. " 
                    + "And then, she broke free. Free from the grasp of gravity and the claws of the tree, now she was truly free, she really was a bird. "
                    + "And so Dinaa flew through the skies, free and unbound, dancing amongst the clouds, and frolicking with her kin, her flock. Though the happiness would not last, as clouds darkened her friends left one by one. " 
                    + "To Dinaa, this made no sense, why would they leave? They were free, what could have them return to the earth? So Dinaa, flew on, although soon the winds picked up and rain started pouring. " 
                    + "And Dinaa, frightened as she was, began to realise that perhaps this was what had driven her friends away. Something that she had had no chance to predict, they had predicted, so perhaps she was no bird after all? "
                    + "Then, as if to confirm her doubts a bolt of pure power struck her down from above, blindingly bright, and full of searing heat. And then it was gone, but the damage was done. "
                    + "Dinaa, unable to keep herself afloat in this rage of the gods, fell down to earth, perhaps a better fate awaited her there?");

        //this is a variation of the ending scen that plays if the player doesn't follow the flags at any point
        /*41*/ storyLines.Add("Screw this tree! Dinaa had had enough. She was a bird! She was free! Why would she confine herself to a single tree? She could go wherever she wanted to! Nobody could tell her where to go. "
                    + "And then, she broke free. Free from the grasp of gravity and the claws of the tree, now she was truly free, she really was a bird. "
                    + "And so Dinaa flew through the skies, free and unbound, dancing amongst the clouds, and frolicking with her kin, her flock. Though the happiness would not last, as clouds darkened her friends left one by one. "
                    + "To Dinaa, this made no sense, why would they leave? They were free, what could have them return to the earth? So Dinaa, flew on, although soon the winds picked up and rain started pouring. "
                    + "And Dinaa, frightened as she was, began to realise that perhaps this was what had driven her friends away. Something that she had had no chance to predict, they had predicted, so perhaps she was no bird after all? "
                    + "Then, as if to confirm her doubts a bolt of pure power struck her down from above, blindingly bright, and full of searing heat. And then it was gone, but the damage was done. "
                    + "Dinaa, unable to keep herself afloat in this rage of the gods, fell down to earth, perhaps a better fate awaited her there? ");

        //this is also  a variation of the ending scen that plays if the player doesn't follow the flags at any point
        /*42*/ storyLines.Add("It was time to say farewell to the tree. There was nothing left for Dinaa to explore. And she was a bird, she was free, she couldn't confine herself to a single tree. She could go wherever she wanted to, so she set off to conquer the skies. "
                    + "And then, she broke free. Free from the grasp of gravity and the claws of the tree, now she was truly free, she really was a bird. "
                    + "And so Dinaa flew through the skies, free and unbound, dancing amongst the clouds, and frolicking with her kin, her flock. Though the happiness would not last, as clouds darkened her friends left one by one. "
                    + "To Dinaa, this made no sense, why would they leave? They were free, what could have them return to the earth? So Dinaa, flew on, although soon the winds picked up and rain started pouring. "
                    + "And Dinaa, frightened as she was, began to realise that perhaps this was what had driven her friends away. Something that she had had no chance to predict, they had predicted, so perhaps she was no bird after all? "
                    + "Then, as if to confirm her doubts a bolt of pure power struck her down from above, blindingly bright, and full of searing heat. And then it was gone, but the damage was done. "
                    + "Dinaa, unable to keep herself afloat in this rage of the gods, fell down to earth, perhaps a better fate awaited her there? ");
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
