using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Kai : MonoBehaviour
{
    // wrote this script based off of Camera_G with some tweaks and stuff - Kai
    public Transform target; //birdModel Transform
    public Movement_Bird_G birdScript;// birdparent

    public Vector3 positionOffset = new Vector3(0f, 1f, 0f); // Offset of the camera position in relation to player
    public Vector3 focalOffset = new Vector3(0f, 0.3f, 0f); // Offset of point in space that camera is looking at in relation to player

    public float movementLagCoefficient = 1f;
    public float rotationLagCoefficient = 1f;
    private Vector3 desiredPosition;
    private Vector3 smoothedPosition;

    public bool quaternionDependant = false;
    public bool angleDependant = false;
    public bool vertSpeedDependant = false;
    public bool horiSpeedDependant = false;


    // Start is called before the first frame update
    void Start()
    {
        transform.position = target.position + positionOffset;
        transform.LookAt(target.position + focalOffset);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // determines target position behind the bird
        desiredPosition = target.position + positionOffset;

        // rotates desiredPosition around target.position to have offsets be relative to the bird position
        // there's two ways to do this in this script, either the line below or the one that has [2] commented above it
        //[1]
        if (quaternionDependant == true) desiredPosition = Quaternion.Euler(target.eulerAngles) * (desiredPosition - target.position) + target.position;



        //determines position between current camera position and target position and then moves to it iof movementLagCoefficient = 1f then it should move at the same speed as the bird
        smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, Mathf.Clamp01(movementLagCoefficient * birdScript.movementSpeed * Time.deltaTime));
        transform.position = smoothedPosition;

        //alternate way to get this thing to basically always be behind the bird
        //[2]

        //use either of the lines below, .angle is slightly less aggressive than vericcurrentspeed, when using this disable [1]
        if (angleDependant == true) transform.RotateAround(target.transform.position, Vector3.right, birdScript.angle * Time.deltaTime * rotationLagCoefficient);
        if (vertSpeedDependant == true) transform.RotateAround(target.transform.position, Vector3.right, birdScript.vertCurrentRotationSpeed * Time.deltaTime * rotationLagCoefficient);


        // I recommend always using the line below
        if (horiSpeedDependant == true) transform.RotateAround(target.transform.position, Vector3.up, birdScript.horiCurrentRotationSpeed * Time.deltaTime * rotationLagCoefficient);


        //has the camera look at point offset by focalOffset from bird
        transform.LookAt(target.position + focalOffset);

    }
}
