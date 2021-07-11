using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryTriggers : MonoBehaviour
{
    public int storyPoint;
    private StoryController mainStoryController;
    public Vector3 saveLocation;
    public Vector3 saveRotation;
    public int gameStage;
    public string cameraState;

    void Start(){
        mainStoryController = GameObject.Find("StoryController").GetComponent<StoryController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player"){
            mainStoryController.callStory(storyPoint);
            gameObject.SetActive(false);

            SaveSystem.SavePlayer(gameStage, saveLocation, saveRotation, cameraState);
        }
    }

}
