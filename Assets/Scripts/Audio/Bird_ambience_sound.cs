using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Bird_ambience_sound : MonoBehaviour
{

    public AudioSource source;
    public AudioLowPassFilter lowPass;
    public AudioHighPassFilter highPass;

    public GameObject paper;
    public GameObject trigger;

    private bool secondRoom;

    // Start is called before the first frame update
    void Start()
    {
        highPass.cutoffFrequency = 5000f;
    }

    // Update is called once per frame
    void Update()
    {
        if (secondRoom == false)
        {
            float distance = (paper.transform.position - trigger.transform.position).magnitude;
            lowPass.cutoffFrequency = Mathf.Clamp(2600 + 5000 / (distance * distance), 2600, 22000);
            // highPass.cutoffFrequency = Mathf.Clamp(distance*300, 100, 1000);
            source.volume = Mathf.Clamp(0.5f - 0.025f * (distance * distance), 0.15f, 0.45f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            secondRoom = true;
        }
    }
}
