using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Movement_Bird_G : MonoBehaviour
{

    public float movementSpeed = 3f;
    public float minSpeed = 1f;
    public float maxSpeed = 7f;

    public float vertMaxRotationSpeed = 50f;
    private float vertCurrentRotationSpeed = 0f;
    public float vertRotFriction = 50f;
    public float vertRotationLimit = 60f;
    public float vertRotAcc = 150f;

    public float horiMaxRotationSpeed = 80f;
    public float horiCurrentRotationSpeed = 0f;
    public float horiRotFriction = 50f;
    public float horiRotAcc = 140f;

    public float angle; // describes if the bird goes up or down

    public float grav = -2f;
    public float basicFlap = 0.1f;
    public float boostMultiplier = 3f;

    public GameObject bird; 

    // Update is called once per frame
    void Update()
    {
        flyG();
    }

    private void flyG()
    {

        // set up for angle variable

        Vector3 dirAngle = bird.transform.rotation.eulerAngles;
        Vector3 dir = bird.transform.forward * (-1f); // the front of the model faces back for some reason

        angle = dirAngle.x;

        if (angle > 180f) angle = angle - 360f;


        // get inputs

        bool W = false;
        bool S = false;
        bool A = false;
        bool D = false;
        bool spacebar = false;

        if (Input.GetKey(KeyCode.W)) W = true;
        if (Input.GetKey(KeyCode.S)) S = true;
        if (Input.GetKey(KeyCode.A)) A = true;
        if (Input.GetKey(KeyCode.D)) D = true;
        if (Input.GetKey(KeyCode.Space)) spacebar = true;

        float horizontalInput = Input.GetAxis("Horizontal");
        //Get the value of the Horizontal input axis.

        float verticalInput = Input.GetAxis("Vertical");
        //Get the value of the Vertical input axis.



        // apply vertical movement,

        if (W && !S)
        {
            // vertCurrentRotationSpeed += vertRotAcc * Time.deltaTime;
            verticalInput = 1f;
        }

        if (S && !W)
        {
            // vertCurrentRotationSpeed -= vertRotAcc * Time.deltaTime;
            verticalInput = -1f;
        }

        if (verticalInput!=0)
        {
            vertCurrentRotationSpeed += verticalInput * vertRotAcc * Time.deltaTime;
        }

        if (vertCurrentRotationSpeed > 0f)
        {
            vertCurrentRotationSpeed -= vertRotFriction * Time.deltaTime;
        } else
        {
            vertCurrentRotationSpeed += vertRotFriction * Time.deltaTime;
        }

        vertCurrentRotationSpeed = Mathf.Clamp(vertCurrentRotationSpeed, -vertMaxRotationSpeed, vertMaxRotationSpeed);
        
        // apply left or right turning movement

        if (A && !D)
        {
            // horiCurrentRotationSpeed -= horiRotAcc * Time.deltaTime;
            horizontalInput = -1f;
        }

        if (D && !A)
        {
            // horiCurrentRotationSpeed += horiRotAcc * Time.deltaTime;
            horizontalInput = 1f;
        }

        if (horizontalInput!=0)
        {
            horiCurrentRotationSpeed += horizontalInput * horiRotAcc * Time.deltaTime;
        }

        if (horiCurrentRotationSpeed > 0f)
        {
            horiCurrentRotationSpeed -= horiRotFriction * Time.deltaTime;
        }
        else
        {
            horiCurrentRotationSpeed += horiRotFriction * Time.deltaTime;
        }

        horiCurrentRotationSpeed = Mathf.Clamp(horiCurrentRotationSpeed, -horiMaxRotationSpeed, horiMaxRotationSpeed);

        bird.transform.Rotate(0, horiCurrentRotationSpeed * Time.deltaTime, 0, Space.World);


        // clamp angle between min and max value

        if (angle < vertRotationLimit && angle > -vertRotationLimit)
        {
            bird.transform.Rotate(vertCurrentRotationSpeed * Time.deltaTime, 0, 0);
        }
        else if (angle > vertRotationLimit)
        {
            while (angle > vertRotationLimit)
            {
                bird.transform.Rotate(-0.01f, 0, 0);
                angle -= 0.01f;
            }
        }
        else if (angle < -vertRotationLimit)
        {
            while (angle < -vertRotationLimit)
            {
                bird.transform.Rotate(0.01f, 0, 0);
                angle += 0.01f;
            }
        }


        // tilt bird by how much its turning left or right

        Quaternion rot1 = Quaternion.Euler(bird.transform.rotation.eulerAngles.x, bird.transform.rotation.eulerAngles.y, horiCurrentRotationSpeed * 0.3f);
        bird.transform.localRotation = rot1;


        // apply movement in forward direction of bird

        float angleRad = (Mathf.PI / 180) * angle;
        movementSpeed += Mathf.Sin(angleRad) * grav * Time.deltaTime;

        
        if (spacebar)
        {
            boostMultiplier = 3f;
        } else
        {
            boostMultiplier = 1f;
        }

        movementSpeed += basicFlap * boostMultiplier * (maxSpeed/(movementSpeed-0.2f)) * Time.deltaTime;
        movementSpeed = Mathf.Clamp(movementSpeed,minSpeed,maxSpeed);
        gameObject.transform.position += dir * movementSpeed * Time.deltaTime;

    }

}
