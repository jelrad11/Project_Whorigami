using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryTriggers : MonoBehaviour
{
    public int storyPoint;
    private StoryController mainStoryController;

    void Start(){
        mainStoryController = GameObject.Find("StoryController").GetComponent<StoryController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player"){
            mainStoryController.callStory(storyPoint);
            gameObject.SetActive(false);
        }
    }

}
