using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Bird_G : MonoBehaviour
{

    public Transform target;

    public float smoothSpeed = 1f;

    public Vector3 baseOffset = new Vector3(0, 0.5f, 0); // raises camera above bird by default
    public float offset0; // brings camera behind the bird
    public float offset1; // makes the camera look in the direction the player is turning
    public Vector3 offset2; // adjusts distance depending on bird angle
    public float focalOffset = 0.2f; // offset for where the center of camera is looking in relation to the bird

    public Vector3 offsetCamera; // offset created by right joystick
    private float cameraSpeed = 0.18f;

    public GameObject birdparent;
    private Movement_Bird_G birdscript;

    private void Start()
    {
        birdscript = birdparent.GetComponent<Movement_Bird_G>();
    }

    void LateUpdate()
    {
        //offset0 = 0.05f;
        offset1 = birdscript.horiCurrentRotationSpeed / 140f;
        baseOffset = target.up * 0.5f;
        //   offset2.y = birdscript.angle / 60f;

        //offsetCamera = (target.right * Input.GetAxis("Horizontal1") + baseOffset * Input.GetAxis("Vertical1")).normalized * 2f;

        Vector3 desiredPosition = target.position + target.forward * offset0 + target.right * offset1 + offset2 + baseOffset; // + cause bird model has backside on front

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, birdscript.movementSpeed * smoothSpeed * Time.deltaTime);

        //smoothedPosition = Vector3.Lerp(smoothedPosition, smoothedPosition + offsetCamera, birdscript.movementSpeed * cameraSpeed * Time.deltaTime);

        transform.position = smoothedPosition;

        transform.LookAt(target.position + new Vector3(0, focalOffset, 0));


    }
}

/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Bird_G : MonoBehaviour
{

    public Transform target;

    public float smoothSpeed = 10f;
    public Vector3 baseOffset = new Vector3 (0, 0.5f, 0); // raises camera above bird by default
    
    public float offset0; // brings camera behind the bird
    public float offset1; // makes the camera look in the direction the player is turning
    public Vector3 offset2; // adjusts distance depending on bird angle
    public GameObject birdparent;
    private Movement_Bird_G birdscript;

    private void Start()
    {
        birdscript = birdparent.GetComponent<Movement_Bird_G>();
    }

    void LateUpdate()
    {
        offset0 = 0.05f;
        offset1 = birdscript.horiCurrentRotationSpeed/140f;
        offset2.y = birdscript.angle/60f;

        Vector3 desiredPosition = target.position + target.forward * offset0 + target.right * offset1 + offset2 + baseOffset; // + cause bird model has backside on front

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, birdscript.movementSpeed * smoothSpeed * Time.deltaTime);

        transform.position = smoothedPosition;

        transform.LookAt(target);


    }

}
*/