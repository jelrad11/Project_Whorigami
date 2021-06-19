using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_Frog : MonoBehaviour
{
    public float walkingSpeed = 1f;
    public float turnSpeed = 1f;

    public float jumpingStrength = 25f;
    public float directionalJumpStrength = 1f;

    private float startJumpTime;
    private float maxJumpTime;
    public float jumpBaseAccel = 1f;
    public float jumpForwardAccel = 1f;
    public float jumpBackwardAccel = 1f;
    public float airJumpTime;

    private bool jumping = false;
    private bool walking;
    Rigidbody frogRigidbody;
    
    void Start(){
        frogRigidbody = gameObject.GetComponent<Rigidbody>();
    }
    void Update()
    {
        jumpingMovement();
        if(!jumping) walkingMovement();
    }
    private void walkingMovement(){
        Vector3 movementVector = new Vector3();
        Vector3 rotationVector = new Vector3();

        if(Input.GetKey(KeyCode.W)) { 
            movementVector = gameObject.transform.forward; 
            walking = true;
        }
        else if(Input.GetKey(KeyCode.S)) {
            movementVector = gameObject.transform.forward * -1f;
            walking = true;
        }
        else if(Input.GetKey(KeyCode.A)) {
            movementVector = gameObject.transform.right * -1f; 
            walking = true;
        }
        else if(Input.GetKey(KeyCode.D)) {
            movementVector = gameObject.transform.right; 
            walking = true;
        }
        else walking = false;

        if(!walking){
            if(Input.GetKey(KeyCode.E)) rotationVector = new Vector3(0f, 90f, 0f);
            else if(Input.GetKey(KeyCode.Q)) rotationVector = new Vector3(0f, -90f, 0f);
        }
        

        gameObject.transform.position = gameObject.transform.position + (movementVector * walkingSpeed * Time.deltaTime);
        gameObject.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.eulerAngles + (rotationVector * turnSpeed * Time.deltaTime));
    }
    private void jumpingMovement(){

        Vector3 jumpVector = gameObject.transform.up + (gameObject.transform.forward * directionalJumpStrength);

        if(Input.GetKey(KeyCode.Space) && !jumping){
            jumping = true;
            startJumpTime = Time.time;
            maxJumpTime = startJumpTime + airJumpTime;

            frogRigidbody.AddForce(jumpVector * jumpingStrength);
        }
        else if(Input.GetKey(KeyCode.Space) && jumping && (startJumpTime + maxJumpTime > Time.time )){
            frogRigidbody.AddForce(gameObject.transform.up * jumpBaseAccel, ForceMode.Acceleration);
        }

        if(Input.GetKey(KeyCode.W) && jumping){
            frogRigidbody.AddForce(gameObject.transform.forward * jumpForwardAccel, ForceMode.Acceleration);
        }
        if(Input.GetKey(KeyCode.S) && jumping){
            frogRigidbody.AddForce(gameObject.transform.forward * -1f * jumpForwardAccel, ForceMode.Acceleration);
        }
    }
    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "Ground") jumping = false;
    }
}
