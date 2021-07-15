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

    public bool addCanTransformLong;
    public bool addCanTransformShort;
    public bool addCanFly;
    public bool addCanTurn;

    public Movement_Paper movement_Paper;
    void Awake (){
        mainStoryController = GameObject.Find("StoryController").GetComponent<StoryController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player"){
            mainStoryController.callStory(storyPoint);
            gameObject.SetActive(false);

            movement_Paper = other.GetComponent<Movement_Paper>();

            if(addCanTransformLong) movement_Paper.canTransformLong = true;
            if(addCanTransformShort) movement_Paper.canTransformShort = true;
            if(addCanFly) movement_Paper.canFly = true;
            if(addCanTurn) movement_Paper.canTurn = true;

            SaveSystem.SavePlayer(gameStage, saveLocation, saveRotation, cameraState, movement_Paper.canTransformLong, movement_Paper.canTransformShort, movement_Paper.canFly, movement_Paper.canTurn);
        }
    }

}
