using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Movement_Bird : MonoBehaviour
{
    public float movementSpeed = 5f;
    public float rotationSpeed = 2f; 

    public GameObject bird; 

    // Update is called once per frame
    void Update()
    {
        flyingMovement();
    }

    public void flyingMovement(){
        
        Vector3 movVector = new Vector3();
        Vector3 forwardMovement = gameObject.transform.forward * movementSpeed * Time.deltaTime;
        Vector3 rotVector = new Vector3();

        if(Input.GetKey(KeyCode.W)) movVector += new Vector3(0f, 1f, 0f) * movementSpeed * Time.deltaTime;
        if(Input.GetKey(KeyCode.S)) movVector += new Vector3(0f, -1f, 0f) * movementSpeed * Time.deltaTime;
        if(Input.GetKey(KeyCode.A)) rotVector += new Vector3(0f, -90f, 0f) * rotationSpeed * Time.deltaTime;
        if(Input.GetKey(KeyCode.D)) rotVector += new Vector3(0f, 90f, 0f) * rotationSpeed * Time.deltaTime;

        if(!Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W)){
            if(Input.GetKeyUp(KeyCode.D) || Input.GetKeyDown(KeyCode.A)) rotateBird(false);
            if(Input.GetKeyUp(KeyCode.A) || Input.GetKeyDown(KeyCode.D)) rotateBird(true);
        }
        if(!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)){
            if(Input.GetKeyUp(KeyCode.S) || Input.GetKeyDown(KeyCode.W)) tiltBird(true);
            if(Input.GetKeyUp(KeyCode.W) || Input.GetKeyDown(KeyCode.S)) tiltBird(false);
        }
        
        
        if(!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S)) {
            Quaternion tmp3 = Quaternion.Euler(0f, 180f, 0f);
            bird.transform.localRotation = tmp3;


        }
        gameObject.transform.Rotate(rotVector, Space.Self);
        gameObject.transform.position += movVector + forwardMovement;
    }


    private void rotateBird(bool right){
        Vector3 rotationR = new Vector3(0f, 0f, 45f);
        Vector3 rotationL = new Vector3(0f, 0f, -45f);
        
        if(right) bird.transform.Rotate(rotationR, Space.Self);
        else bird.transform.Rotate(rotationL, Space.Self);    
    }
    private void tiltBird(bool up){
        Vector3 rotationU = new Vector3(45f, 0f, 0f);
        Vector3 rotationD = new Vector3(-45f, 0f, 0f);
        
        if(up) bird.transform.Rotate(rotationU, Space.Self);
        else bird.transform.Rotate(rotationD, Space.Self);
    }
}
