using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_Paper : MonoBehaviour 
{
    public float rollSpeed = 5f;
    public float rotationSpeed = 5f;
    public float slowDownSpeed = 1f;

    private bool flat = true;
    private bool longSide = false;

    public GameObject flatPaper;
    public GameObject rolledUp_Long;
    public GameObject rolledUp_Short;


    public GameObject posRefW;
    public GameObject posRefS;
    public GameObject posRefD;
    public GameObject posRefA;


    void Update(){
        rollUp();
        rolledUpMovement();
    }

    private void rolledUpRotation(){
        Vector3 rotation = new Vector3(0f, 90f, 0f) * rotationSpeed * Time.deltaTime;
        Rigidbody longRb = rolledUp_Long.GetComponent<Rigidbody>();
        Rigidbody shortRb = rolledUp_Short.GetComponent<Rigidbody>();

        if(Input.GetKey(KeyCode.W)){
            if(longSide){
                if(Input.GetKey(KeyCode.A)) {
                    rolledUp_Long.transform.Rotate(rotation * -1f, Space.World);
                    flatPaper.transform.Rotate(rotation * -1f, Space.World);
                }
                else if(Input.GetKey(KeyCode.D)) {
                    rolledUp_Long.transform.Rotate(rotation, Space.World);
                    flatPaper.transform.Rotate(rotation, Space.World);
                } 
            }
            else{
                if(Input.GetKey(KeyCode.A)) {
                    rolledUp_Short.transform.Rotate(rotation * -1f, Space.World);
                    flatPaper.transform.Rotate(rotation * -1f, Space.World);
                }
                else if(Input.GetKey(KeyCode.D)) {
                    rolledUp_Short.transform.Rotate(rotation, Space.World);
                    flatPaper.transform.Rotate(rotation, Space.World);
                } 
            }
        }
        else if(Input.GetKey(KeyCode.S)){
            if(longSide){
                if(Input.GetKey(KeyCode.D)) {
                    rolledUp_Long.transform.Rotate(rotation * -1f, Space.World);
                    flatPaper.transform.Rotate(rotation * -1f, Space.World);
                }
                else if(Input.GetKey(KeyCode.A)) {
                    rolledUp_Long.transform.Rotate(rotation, Space.World);
                    flatPaper.transform.Rotate(rotation, Space.World);
                } 
            }
            else{
                if(Input.GetKey(KeyCode.D)) {
                    rolledUp_Short.transform.Rotate(rotation * -1f, Space.World);
                    flatPaper.transform.Rotate(rotation * -1f, Space.World);
                }
                else if(Input.GetKey(KeyCode.A)) {
                    rolledUp_Short.transform.Rotate(rotation, Space.World);
                    flatPaper.transform.Rotate(rotation, Space.World);
                } 
            }
        }

        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) && !(Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S))){
            float rotationMod = 1f;
            if(Input.GetKey(KeyCode.A)) rotationMod = -1f;

            if(longSide){
                rolledUp_Long.transform.Rotate(rotation * rotationMod, Space.World);
                flatPaper.transform.Rotate(rotation * rotationMod, Space.World);
            }
            else{
                rolledUp_Short.transform.Rotate(rotation * rotationMod, Space.World);
                flatPaper.transform.Rotate(rotation * rotationMod, Space.World);
            }
        }


    }
    private void rolledUpMovement(){
        if(!flat){
            if(longSide){
                if(Input.GetKey(KeyCode.S)) rolledUp_Long.GetComponent<Rigidbody>().AddForce(flatPaper.transform.right * Time.deltaTime * rollSpeed);
                else if(Input.GetKey(KeyCode.W)) rolledUp_Long.GetComponent<Rigidbody>().AddForce(flatPaper.transform.right * -1 * Time.deltaTime * rollSpeed);
                else rolledUp_Long.GetComponent<Rigidbody>().velocity -= rolledUp_Long.GetComponent<Rigidbody>().velocity * slowDownSpeed * Time.deltaTime;
            }
            else{
                if(Input.GetKey(KeyCode.W)) rolledUp_Short.GetComponent<Rigidbody>().AddForce(flatPaper.transform.forward * Time.deltaTime * rollSpeed);
                else if(Input.GetKey(KeyCode.S)) rolledUp_Short.GetComponent<Rigidbody>().AddForce(flatPaper.transform.forward * -1 * Time.deltaTime * rollSpeed);
                else rolledUp_Short.GetComponent<Rigidbody>().velocity -= rolledUp_Short.GetComponent<Rigidbody>().velocity * slowDownSpeed * Time.deltaTime;
            }

            rolledUpRotation();
        }
    }

    private void rollUp(){
        if(flat){
            if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)){
                flatPaper.SetActive(false);
                rolledUp_Long.SetActive(false);
                rolledUp_Short.SetActive(true);
                
                rolledUp_Short.GetComponent<Rigidbody>().velocity = new Vector3();

                if(Input.GetKey(KeyCode.W)) synchroniseObjects(0f, 0f);
                else if(Input.GetKey(KeyCode.S)) synchroniseObjects(0f, 1f);
                
                flat = false;
                longSide = false;
            }
            else if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A)){
                flatPaper.SetActive(false);
                rolledUp_Long.SetActive(true);
                rolledUp_Short.SetActive(false);
                
                rolledUp_Long.GetComponent<Rigidbody>().velocity = new Vector3();

                if(Input.GetKey(KeyCode.D)) synchroniseObjects(0f, 3f);
                else if(Input.GetKey(KeyCode.A)) synchroniseObjects(0f, 2f);                
                
                flat = false;
                longSide = true;
            }
        }
        else {
            if(Input.GetKey(KeyCode.E) && (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.W) )){
                flatPaper.SetActive(true);
                rolledUp_Long.SetActive(false);
                rolledUp_Short.SetActive(false);
            
                if(longSide) synchroniseObjects(1f);
                else synchroniseObjects(2f);

                flat = true;
                longSide = false;
            }
        }
    }

    private void synchroniseObjects(float choice, float direction = -1f){  //0 == flatPaper; 1 == rolledUp_Long; 2 == rolledUp_Short; //0 == W; 1 == S; 2 == A; 3 == D;
        Vector3 paperPosition;
        switch(choice){ 
            case 0:
                paperPosition = new Vector3(flatPaper.transform.position.x, flatPaper.transform.position.y + 0.25f, flatPaper.transform.position.z);
                rolledUp_Long.transform.position = synchroniseObjectPos(direction);
                rolledUp_Short.transform.position = synchroniseObjectPos(direction);

                rolledUp_Long.transform.rotation = Quaternion.Euler(90f, flatPaper.transform.rotation.eulerAngles.y, 0f);
                rolledUp_Short.transform.rotation = Quaternion.Euler(0f, flatPaper.transform.rotation.eulerAngles.y, 90f);
                break;
            case 1:
                paperPosition = new Vector3(rolledUp_Long.transform.position.x, rolledUp_Long.transform.position.y, rolledUp_Long.transform.position.z);
                flatPaper.transform.position = paperPosition;
                rolledUp_Short.transform.position = paperPosition;

                //flatPaper.transform.rotation = Quaternion.Euler(0f, rolledUp_Long.transform.rotation.eulerAngles.y + 90f, 0f);
                rolledUp_Short.transform.rotation = Quaternion.Euler(0f, rolledUp_Long.transform.rotation.eulerAngles.y, 90f);
                break;
            case 2:
                paperPosition = new Vector3(rolledUp_Short.transform.position.x, rolledUp_Short.transform.position.y, rolledUp_Short.transform.position.z);
                rolledUp_Long.transform.position = paperPosition;
                flatPaper.transform.position = paperPosition;

                rolledUp_Long.transform.rotation = Quaternion.Euler(90f, rolledUp_Short.transform.rotation.eulerAngles.y, 0f);
                //flatPaper.transform.rotation = Quaternion.Euler(0f, rolledUp_Short.transform.rotation.eulerAngles.y, 0f);
                break;
        }
    }
    private Vector3 synchroniseObjectPos(float direction){ //0 == W; 1 == S; 2 == A; 3 == D;
        switch(direction){
            case 0:
                return new Vector3(posRefW.transform.position.x, posRefW.transform.position.y + (rolledUp_Long.transform.localScale.y / 2f) - (flatPaper.transform.localScale.y), posRefW.transform.position.z) ;
            case 1:
                return new Vector3(posRefS.transform.position.x, posRefS.transform.position.y + (rolledUp_Long.transform.localScale.y / 2f) - (flatPaper.transform.localScale.y), posRefS.transform.position.z) ;
            case 2:
                return new Vector3(posRefA.transform.position.x, posRefA.transform.position.y + (rolledUp_Long.transform.localScale.y / 2f) - (flatPaper.transform.localScale.y), posRefA.transform.position.z) ;
            case 3:
                return new Vector3(posRefD.transform.position.x, posRefD.transform.position.y + (rolledUp_Long.transform.localScale.y / 2f) - (flatPaper.transform.localScale.y), posRefD.transform.position.z) ;
            default:
                return new Vector3();
        }
    }
}

