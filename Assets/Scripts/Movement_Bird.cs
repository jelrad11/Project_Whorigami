using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Movement_Bird : MonoBehaviour
{
    public float movementSpeed = 1f;
    public float rotationSpeed = 1f;
    private Vector3 curPos;


    // Start is called before the first frame update
    void Start()
    {
        curPos = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        flyingMovement();
    }

    public void flyingMovement(){
        
        Vector3 movVector = new Vector3();
        Vector3 forwardMovement = gameObject.transform.forward * movementSpeed * Time.deltaTime;
        Vector3 rotVector = new Vector3();

        if(Input.GetKey(KeyCode.Space)) movVector = gameObject.transform.up * movementSpeed * Time.deltaTime;
        if(Input.GetKey(KeyCode.C)) movVector = gameObject.transform.up * -1f * movementSpeed * Time.deltaTime;
        if(Input.GetKey(KeyCode.A)) rotVector = new Vector3(0f, -90f, 0f) * rotationSpeed * Time.deltaTime;
        if(Input.GetKey(KeyCode.D)) rotVector = new Vector3(0f, 90f, 0f) * rotationSpeed * Time.deltaTime;

        gameObject.transform.Rotate(rotVector, Space.Self);
        gameObject.transform.position += movVector + forwardMovement;
    }
}
