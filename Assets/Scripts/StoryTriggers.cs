using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryTriggers : MonoBehaviour
{
    public int storyPoint;

    public List<GameObject> delayActivation;
    public float timeDelay;
    public List<GameObject> nextTrigger;
    public bool deactiveThisTrigger;
    public List<GameObject>  specialTrigger;
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
    
    public bool useMovementPaper;
    public Movement_Paper movement_Paper;

    private Tutorial_ui ui_script;

    void Awake () {
        mainStoryController = GameObject.Find("StoryController").GetComponent<StoryController>();
        ui_script = GameObject.Find("StoryController").GetComponent<Tutorial_ui>();
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
            for(int i = 0; i < nextTrigger.Count; i++) nextTrigger[i].SetActive(true);
            if(deactiveThisTrigger) for(int i = 0; i < specialTrigger.Count; i++) specialTrigger[i].SetActive(false);
            
            if(delayActivation.Count != 0) StartCoroutine(delayActivate());

            if(useMovementPaper){
                movement_Paper = other.GetComponentInParent<Movement_Paper>();
                if(addCanTransformLong || addCanTransformShort || addCanFly || addCanTurn) StartCoroutine(addAbility());
            }

            if(gameStage == 0) SaveSystem.SavePlayer(gameStage, saveLocation, saveRotation, cameraState, movement_Paper.canTransformLong, movement_Paper.canTransformShort, movement_Paper.canFly, movement_Paper.canTurn);
            else if(gameStage == 1) SaveSystem.SavePlayer(gameStage, saveLocation, saveRotation, "null",false, false, false, false);
        }
    }
    
    private IEnumerator delayActivate(){
        yield return new WaitForSeconds(timeDelay);

        for(int i = 0; i < delayActivation.Count; i++) delayActivation[i].SetActive(true);
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

        FadeInUI();

        yield return new WaitForSeconds(9f);

        FadeOutUI();

        if (addAbilityTimer != 0f) gameObject.SetActive(false);
    }

    private void FadeInUI()
    {
        if (addCanTransformLong)
        {
            ui_script.FadeInImage(0);
        }
        else if (addCanTurn)
        {
            ui_script.FadeInImage(1);
        }
        else if (addCanTransformShort)
        {
            ui_script.FadeInImage(2);
        }
        else if (addCanFly)
        {
            ui_script.FadeInImage(3);
        }
    }

    private void FadeOutUI()
    {
        if (addCanTransformLong)
        {
            ui_script.FadeOutImage(0);
        }
        else if (addCanTurn)
        {
            ui_script.FadeOutImage(1);
        }
        else if (addCanTransformShort)
        {
            ui_script.FadeOutImage(2);
        }
        else if (addCanFly)
        {
            ui_script.FadeOutImage(3);
        }
    }

}
