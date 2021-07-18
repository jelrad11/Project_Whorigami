using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryTriggers : MonoBehaviour
{
    public int storyPoint;
<<<<<<< HEAD
    public GameObject nextTrigger;
    public bool deactiveThisTrigger;
    public GameObject specialTrigger;
=======
>>>>>>> parent of 8c5ab83 (Update)
    private StoryController mainStoryController;
    public Vector3 saveLocation;
    public Vector3 saveRotation;
    public int gameStage;
    public string cameraState;

    public float addAbilityTimer;
    public bool addCanTransformLong;
    public bool addCanTransformShort;
    public bool addCanFly;
    public bool addCanTurn;

<<<<<<< HEAD

    
=======
>>>>>>> parent of 8c5ab83 (Update)
    public Movement_Paper movement_Paper;
    void Awake () {
        mainStoryController = GameObject.Find("StoryController").GetComponent<StoryController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player"){
            mainStoryController.callStory(storyPoint);

            if (addAbilityTimer == 0f)
            {
                gameObject.SetActive(false);
            } else
            {
                gameObject.GetComponent<Collider>().enabled = false;
                foreach (Transform child in transform)
                    child.gameObject.SetActive(false);
            }
<<<<<<< HEAD
            nextTrigger.SetActive(true);
            if(deactiveThisTrigger) specialTrigger.SetActive(false);
            
=======

>>>>>>> parent of 8c5ab83 (Update)
            movement_Paper = other.GetComponentInParent<Movement_Paper>();
            
            if(addCanTransformLong || addCanTransformShort || addCanFly || addCanTurn) StartCoroutine(addAbility());

            //SaveSystem.SavePlayer(gameStage, saveLocation, saveRotation, cameraState, movement_Paper.canTransformLong, movement_Paper.canTransformShort, movement_Paper.canFly, movement_Paper.canTurn);
        }
    }

    private IEnumerator addAbility(){
        Data.AbilityAddTimer += addAbilityTimer;
        yield return new WaitForSeconds(Data.AbilityAddTimer);

        Data.AbilityAddTimer = 0f;
        if(addCanTransformLong) movement_Paper.canTransformLong = true;
        if(addCanTransformShort) movement_Paper.canTransformShort = true;
        if(addCanFly) movement_Paper.canFly = true;
        if (addCanTurn)
        {
            movement_Paper.canTurn = true;
            movement_Paper.unrestrictRb();
        }
        if(addAbilityTimer != 0f) gameObject.SetActive(false);
    }
}
