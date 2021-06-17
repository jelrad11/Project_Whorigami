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

    public float angle;

    public float grav = -2f;
    public float basicFlap = 0.1f;



    public GameObject bird; 

    // Update is called once per frame
    void Update()
    {
        flyG();
    }

    private void flyG()
    {
        Vector3 dirAngle = bird.transform.rotation.eulerAngles;
        Vector3 dir = bird.transform.forward * (-1f); // the front of the model faces back for some reason

        angle = dirAngle.x;

        if (angle > 180f) angle = angle - 360f;

        bool W = false;
        bool S = false;
        bool A = false;
        bool D = false;

        if (Input.GetKey(KeyCode.W)) W = true;
        if (Input.GetKey(KeyCode.S)) S = true;
        if (Input.GetKey(KeyCode.A)) A = true;
        if (Input.GetKey(KeyCode.D)) D = true;


        if (W && !S)
        {
            vertCurrentRotationSpeed += vertRotAcc * Time.deltaTime;
        }

        if (S && !W)
        {
            vertCurrentRotationSpeed -= vertRotAcc * Time.deltaTime;
        }


        if (vertCurrentRotationSpeed > 0f)
        {
            vertCurrentRotationSpeed -= vertRotFriction * Time.deltaTime;
        } else
        {
            vertCurrentRotationSpeed += vertRotFriction * Time.deltaTime;
        }

        vertCurrentRotationSpeed = Mathf.Clamp(vertCurrentRotationSpeed, -vertMaxRotationSpeed, vertMaxRotationSpeed);


        if (angle < vertRotationLimit && angle > -vertRotationLimit)
        {
            bird.transform.Rotate(vertCurrentRotationSpeed * Time.deltaTime, 0, 0);
        } else if (angle > vertRotationLimit)
        {
            while (angle > vertRotationLimit)
            {
                bird.transform.Rotate(-0.01f, 0, 0);
                angle -= 0.01f;
            }
        } else if (angle < -vertRotationLimit)
        {
            while (angle < -vertRotationLimit)
            {
                bird.transform.Rotate(0.01f, 0, 0);
                angle += 0.01f;
            }
        }
        
        if (A && !D)
        {
            horiCurrentRotationSpeed -= horiRotAcc * Time.deltaTime;
        }

        if (D && !A)
        {
            horiCurrentRotationSpeed += horiRotAcc * Time.deltaTime;
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


        Quaternion rot1 = Quaternion.Euler(bird.transform.rotation.eulerAngles.x, bird.transform.rotation.eulerAngles.y, horiCurrentRotationSpeed * 0.3f);
        bird.transform.localRotation = rot1;

        float angleRad = (Mathf.PI / 180) * angle;

        // Debug.Log(Mathf.Sin(angleRad));
        // Debug.Log(angle);

        movementSpeed += Mathf.Sin(angleRad) * grav * Time.deltaTime;

        movementSpeed += basicFlap * (maxSpeed/movementSpeed) * Time.deltaTime;


        movementSpeed = Mathf.Clamp(movementSpeed,minSpeed,maxSpeed);

        gameObject.transform.position += dir * movementSpeed * Time.deltaTime;

    }

}
