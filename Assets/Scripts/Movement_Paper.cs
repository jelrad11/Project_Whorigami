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
    void Update(){
        rollUp();
        rolledUpMovement();
    }

    private void rolledUpRotation(){
        Vector3 rotation = new Vector3(0f, 1f, 0f) * rotationSpeed * Time.deltaTime;
        Rigidbody longRb = rolledUp_Long.GetComponent<Rigidbody>();
        Rigidbody shortRb = rolledUp_Short.GetComponent<Rigidbody>();

        if(Input.GetKey(KeyCode.W)){
            if(longSide){
                if(Input.GetKey(KeyCode.A)) 
                    longRb.AddTorque(rotation * -1f);
                if(Input.GetKey(KeyCode.D))
                    longRb.AddTorque(rotation);
            }
            else{
                if(Input.GetKey(KeyCode.A))
                    shortRb.AddTorque(rotation * -1f);
                if(Input.GetKey(KeyCode.D))
                    shortRb.AddTorque(rotation);
            }
        }
        else if(Input.GetKey(KeyCode.S)){
            if(longSide){
                if(Input.GetKey(KeyCode.A))
                    longRb.AddTorque(rotation * -1f);
                if(Input.GetKey(KeyCode.D))
                    longRb.AddTorque(rotation);
            }
            else{
                if(Input.GetKey(KeyCode.A))
                    shortRb.AddTorque(rotation * -1f);
                if(Input.GetKey(KeyCode.D))
                    shortRb.AddTorque(rotation);
            }
        }

        //if(longSide) synchroniseObjects(1f);
        //else synchroniseObjects(2f);
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

                synchroniseObjects(0f);
                flat = false;
                longSide = false;
            }
            else if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A)){
                flatPaper.SetActive(false);
                rolledUp_Long.SetActive(true);
                rolledUp_Short.SetActive(false);

                synchroniseObjects(0f);
                flat = false;
                longSide = true;
            }
        }
    }

    private void synchroniseObjects(float choice){
        Vector3 paperPosition;
        switch(choice){ //0 == flatPaper; 1 == rolledUp_Long; 2 == rolledUp_Short;
            case 0:
                paperPosition = new Vector3(flatPaper.transform.position.x, flatPaper.transform.position.y + 0.5f, flatPaper.transform.position.z);
                rolledUp_Long.transform.position = paperPosition;
                rolledUp_Short.transform.position = paperPosition;

                rolledUp_Long.transform.rotation = Quaternion.Euler(90f, flatPaper.transform.rotation.eulerAngles.y, 0f);
                rolledUp_Short.transform.rotation = Quaternion.Euler(0f, flatPaper.transform.rotation.eulerAngles.y, 90f);
                break;
            case 1:
                paperPosition = new Vector3(rolledUp_Long.transform.position.x, rolledUp_Long.transform.position.y, rolledUp_Long.transform.position.z);
                flatPaper.transform.position = paperPosition;
                rolledUp_Short.transform.position = paperPosition;

                flatPaper.transform.rotation = Quaternion.Euler(0f, rolledUp_Long.transform.rotation.eulerAngles.y, 0f);
                rolledUp_Short.transform.rotation = Quaternion.Euler(0f, rolledUp_Long.transform.rotation.eulerAngles.y, 90f);
                break;
            case 2:
                paperPosition = new Vector3(rolledUp_Short.transform.position.x, rolledUp_Short.transform.position.y, rolledUp_Short.transform.position.z);
                rolledUp_Long.transform.position = paperPosition;
                flatPaper.transform.position = paperPosition;

                rolledUp_Long.transform.rotation = Quaternion.Euler(90f, flatPaper.transform.rotation.eulerAngles.y, 0f);
                flatPaper.transform.rotation = Quaternion.Euler(0f, flatPaper.transform.rotation.eulerAngles.y, 0f);
                break;
        }
    }
}
