using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    public GameObject playerObj;
    private Animator stateSwitchAnim;

    public StoryController storyController;
    private void Awake()
    {
        if(Data.LoadSave){
            PlayerData data = SaveSystem.LoadPlayer();

            if(data != null) loadPlayer(data);
        }
    }

    private void loadPlayer(PlayerData data){
        
        if(data.cameraState != "null") {
            stateSwitchAnim = GameObject.Find("CameraSwitchController").GetComponent<Animator>();
            stateSwitchAnim.SetTrigger(data.cameraState);
        }

        Vector3 pos = new Vector3(data.position[0], data.position[1], data.position[2]);
        playerObj.transform.position = pos;

        Quaternion rot = Quaternion.Euler(data.rotation[0], data.rotation[1], data.rotation[2]);        
        playerObj.transform.rotation = rot;

        storyController.applySettings();
    }
}
