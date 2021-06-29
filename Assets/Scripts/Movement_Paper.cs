using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_Paper : MonoBehaviour 
{
    public float maxVelocity = 5f;
    private float curVelocity; 
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

    private float transfCooldown = 1f;
    
    void Start(){
        curVelocity = 0f;
    }
    void Update(){
        
        rollUp();
        rolledUpMovement();
        checkVelocity();


        transfCooldown -= Time.deltaTime;

        if(Input.GetKey(KeyCode.J)){
            Debug.Log(flatPaper.transform.right);
            Debug.Log(flatPaper.transform.forward);
        }
        
    }

    private void checkVelocity(){
        if(longSide){
            Rigidbody longRB = rolledUp_Long.GetComponent<Rigidbody>();
            if(longRB.velocity.magnitude > maxVelocity){
                longRB.velocity = longRB.velocity.normalized * maxVelocity;
            }
            curVelocity = longRB.velocity.magnitude;
        }
        else if(!flat){
            Rigidbody shortRB = rolledUp_Short.GetComponent<Rigidbody>();
            if(shortRB.velocity.magnitude > maxVelocity){
                shortRB.velocity = shortRB.velocity.normalized * maxVelocity;
            }
            curVelocity = shortRB.velocity.magnitude;
        }

    }

    private void rolledUpRotation(){
        Vector3 rotation = new Vector3(0f, 90f, 0f) * rotationSpeed * Time.deltaTime;
        Rigidbody longRb = rolledUp_Long.GetComponent<Rigidbody>();
        Rigidbody shortRb = rolledUp_Short.GetComponent<Rigidbody>();
        
        float directions = Input.GetAxis("Horizontal");

        if(Input.GetKey(KeyCode.W) || Input.GetAxis("RightTriggerAxis") > 0f){
            if(longSide) rolledUp_Long.transform.Rotate(rotation * directions, Space.World);
            else rolledUp_Short.transform.Rotate(rotation * directions, Space.World);
        }
        else if(Input.GetKey(KeyCode.S) || Input.GetAxis("LeftTriggerAxis") > 0f){
            if(longSide) rolledUp_Long.transform.Rotate(rotation * -1f * directions, Space.World);
            else rolledUp_Short.transform.Rotate(rotation * -1f * directions, Space.World);
        }
        else if(directions != 0f){
            if(longSide) rolledUp_Long.transform.Rotate(rotation * directions, Space.World);
            else rolledUp_Short.transform.Rotate(rotation * directions, Space.World);
        }
        if(longSide) synchroniseObjects(1f);
        else synchroniseObjects(2f);

    }
    private void rolledUpMovement(){
        
        float forwardDir = Input.GetAxis("RightTriggerAxis");
        if(Input.GetAxis("RightTriggerAxis") > 0f) forwardDir = Input.GetAxis("RightTriggerAxis");
        else if(Input.GetAxis("LeftTriggerAxis") > 0f) forwardDir = -1f * Input.GetAxis("LeftTriggerAxis");

        if(Input.GetKey(KeyCode.W)) forwardDir = 1f;
        else if(Input.GetKey(KeyCode.S)) forwardDir = -1f;

        Vector3 r = flatPaper.transform.right;
        Vector3 rightTransl = new Vector3(-r.z, r.y, r.x);
        
        if(!flat){
            if(longSide){
                if(forwardDir != 0f) rolledUp_Long.GetComponent<Rigidbody>().AddForce(flatPaper.transform.right * -1f * forwardDir * Time.deltaTime * rollSpeed);
                else rolledUp_Long.GetComponent<Rigidbody>().velocity -= rolledUp_Long.GetComponent<Rigidbody>().velocity * slowDownSpeed * Time.deltaTime;
            }
            else{
                if(forwardDir != 0f) rolledUp_Short.GetComponent<Rigidbody>().AddForce(flatPaper.transform.right * -1f * forwardDir * Time.deltaTime * rollSpeed);
                else rolledUp_Short.GetComponent<Rigidbody>().velocity -= rolledUp_Short.GetComponent<Rigidbody>().velocity * slowDownSpeed * Time.deltaTime;
            }

            rolledUpRotation();
        }
    }

    private void rollUp(){
        if(flat){
            if(((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)) || (Input.GetAxis("DPadVertical") != 0f)) && transfCooldown <= 0f){
                flatPaper.SetActive(false);
                rolledUp_Long.SetActive(false);
                rolledUp_Short.SetActive(true);
                
                rolledUp_Short.GetComponent<Rigidbody>().velocity = new Vector3();

                if(Input.GetKey(KeyCode.W) || (Input.GetAxis("DPadVertical") > 0f)) synchroniseObjects(0f, 0f);
                else if(Input.GetKey(KeyCode.S) || (Input.GetAxis("DPadVertical") < 0f)) synchroniseObjects(0f, 1f);
                
                flat = false;
                longSide = false;

                transfCooldown = 1f;
            }
            else if(((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A)) || (Input.GetAxis("DPadHorizontal") != 0f)) && transfCooldown <= 0f){
                flatPaper.SetActive(false);
                rolledUp_Long.SetActive(true);
                rolledUp_Short.SetActive(false);
                
                rolledUp_Long.GetComponent<Rigidbody>().velocity = new Vector3();

                if(Input.GetKey(KeyCode.D) || (Input.GetAxis("DPadHorizontal") > 0f)) synchroniseObjects(0f, 3f);
                else if(Input.GetKey(KeyCode.A) || (Input.GetAxis("DPadHorizontal") < 0f)) synchroniseObjects(0f, 2f);                
                
                flat = false;
                longSide = true;

                transfCooldown = 1f;
            }
        }
        else {
            if(((Input.GetKey(KeyCode.E) && (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.W))) 
            || (Input.GetAxis("DPadVertical") != 0f) || (Input.GetAxis("DPadHorizontal") != 0f))  && transfCooldown <= 0f){
                flatPaper.SetActive(true);
                rolledUp_Long.SetActive(false);
                rolledUp_Short.SetActive(false);
            
                if(longSide) synchroniseObjects(1f);
                else synchroniseObjects(2f);
                
                Quaternion rot;
                if(longSide) {
                    rot = Quaternion.Euler(0f, rolledUp_Long.transform.rotation.eulerAngles.y, 0f);
                    flatPaper.transform.rotation = new Quaternion(0f, rot.y, 0f, rot.w);           
                }     
                else {
                    Quaternion rot2 = Quaternion.Euler(0f, rolledUp_Short.transform.rotation.eulerAngles.y -90f, 0f);
                    flatPaper.transform.rotation = new Quaternion(0f, rot2.y, 0f, rot2.w);  
                } 
                flat = true;
                longSide = false;

                transfCooldown = 1f;
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

                rolledUp_Long.transform.rotation = Quaternion.Euler(0f, flatPaper.transform.rotation.eulerAngles.y, 0f);
                rolledUp_Short.transform.rotation = Quaternion.Euler(0f, flatPaper.transform.rotation.eulerAngles.y + 90f, 0f);
                break;
            case 1:
                paperPosition = new Vector3(rolledUp_Long.transform.position.x, rolledUp_Long.transform.position.y, rolledUp_Long.transform.position.z);
                flatPaper.transform.position = paperPosition;
                rolledUp_Short.transform.position = paperPosition;

                Quaternion rot = Quaternion.Euler(0f, rolledUp_Long.transform.rotation.eulerAngles.y, 0f);
                flatPaper.transform.rotation = new Quaternion(0f, rot.y, 0f, rot.w);               
                
                rolledUp_Short.transform.rotation = rolledUp_Long.transform.rotation;
                break;
            case 2:
                paperPosition = new Vector3(rolledUp_Short.transform.position.x, rolledUp_Short.transform.position.y, rolledUp_Short.transform.position.z);
                rolledUp_Long.transform.position = paperPosition;
                flatPaper.transform.position = paperPosition;

                Quaternion asdf = rolledUp_Short.transform.rotation;

                Quaternion rot2 = Quaternion.Euler(0f, rolledUp_Short.transform.rotation.eulerAngles.y , 0f);
                flatPaper.transform.rotation = new Quaternion(0f, rot2.y, 0f, rot2.w);                  
                
                // flatPaper.transform.LookAt(flatPaper.transform.position + rolledUp_Long.transform.up);
                rolledUp_Long.transform.rotation = rolledUp_Short.transform.rotation;   

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

