using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition0_sound : MonoBehaviour
{
    public float delay;
    public AudioSource source0;
    public AudioSource source1;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(sound());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator sound()
    {
        yield return new WaitForSeconds(delay);
        source0.Stop();
        source1.Play();
    }
}
