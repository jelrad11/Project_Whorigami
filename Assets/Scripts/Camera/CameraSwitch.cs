using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
//    public string switchToStateBackward;
    public string switchToStateForward;
   

    private Animator stateSwitchAnimator;

    private void Start() {
        stateSwitchAnimator = GameObject.Find("CameraSwitchController").GetComponent<Animator>();
    }

     private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player"){
            stateSwitchAnimator.SetTrigger(switchToStateForward);
        }
    }
    
    private void switchState(){
            stateSwitchAnimator.SetTrigger(switchToStateForward);
    }   
}
