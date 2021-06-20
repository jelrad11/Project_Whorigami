using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAudioPlayer : MonoBehaviour
{
    // this script was written by Kai for some testing purposes,
    // it's apparently pretty horrible for performance 'cause having two parakeets reduces FPS to like 60, one is fine though
    public bool isChirping; // so that I can have only one or multiple birds chirping at a time
    public float timerMin, timerMax; // min/max time between birdchirps respectively
    private float timer = 0;
    private float timeToAudio;

    AudioSource[] allMyAudio;


    // Start is called before the first frame update
    void Start()
    {
        allMyAudio = GetComponents<AudioSource>();
        RandomiseTimeToAudio();
    }

    // Update is called once per frame
    void Update()
    {
        if (isChirping == false) return;

        timer += Time.deltaTime;
        if (timer >= timeToAudio)
        {
            PlayRandomAudio();
            RandomiseTimeToAudio();
            timer = 0;
        }
    }

    void RandomiseTimeToAudio()// honestly cooulda just written this directly into update but whatever
    {
        timeToAudio = Random.Range(timerMin, timerMax);
        return;
    }

    void PlayRandomAudio()
    {
        int rand = Random.Range(0, allMyAudio.Length);
        allMyAudio[rand].Play();
        return;
    }
}
