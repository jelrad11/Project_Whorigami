using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Movement_Bird_G : MonoBehaviour
{

    public float movementSpeed = 3f;
    public float minSpeed = 1f;
    public float maxSpeed = 7f;

    public float vertMaxRotationSpeed = 50f;
    public float vertCurrentRotationSpeed = 0f; // set this to public so I could read it in the Camera_Kai script - signed Kai
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

    // crashing stuff
    private bool stable = true;
    private bool justCrashed = false;

    // audio stuff
    public AudioSource flyingSource;
    public AudioSource glideSource;
    public AudioSource impactSource;

    // Update is called once per frame
    void Update()
    {
        if (stable)
        {
            flyG();

            flyingSource.volume = 1f * Mathf.Clamp(0.05f + 0.005f * movementSpeed * movementSpeed / (3), 0.05f, 0.1f);
            flyingSource.pitch = 1.2f * Mathf.Clamp(0.8f + (movementSpeed * movementSpeed) / (3), 0.8f, 1.2f);


            glideSource.volume = 4 * Mathf.Clamp(0.04f + 0.01f * movementSpeed * movementSpeed / (3), 0.05f, 0.1f);
            glideSource.pitch = Mathf.Clamp(0.8f + (movementSpeed * movementSpeed) / (3), 0.8f, 1.2f);
        }
        else
        {
            flyingSource.volume = Mathf.Lerp(flyingSource.volume, 0.05f * 1f, Time.deltaTime);
            glideSource.volume = Mathf.Lerp(flyingSource.volume, 0.05f * 4f, Time.deltaTime);
        }

        //   Debug.Log(stable);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "storyTrigger")
        {
            return;
        }
        else if (justCrashed == false)
        {
            stable = false;
            impactSource.volume = Mathf.Clamp(0.2f * movementSpeed * movementSpeed / (3), 0.2f, 0.6f);
            impactSource.pitch = Mathf.Clamp(0.8f + (3) / (movementSpeed * movementSpeed), 0.8f, 1.2f);
            impactSource.Play();
            StartCoroutine("Falling");
        }
    }

    private IEnumerator Falling()
    {
        float falltime = 0f;
        while (falltime < 1f)
        {
            falltime += Time.deltaTime;
            yield return null;
        }
        stable = true;
        justCrashed = true;
        movementSpeed = minSpeed;
        bird.transform.Rotate(0, 180, 0, Space.Self);

        while (falltime < 1.2f)
        {
            falltime += Time.deltaTime;
            yield return null;
        }
        justCrashed = false;

        yield break;
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
        bool shift = false;
        bool control = false;

        if (Input.GetKey(KeyCode.W)) W = true;
        if (Input.GetKey(KeyCode.S)) S = true;
        if (Input.GetKey(KeyCode.A)) A = true;
        if (Input.GetKey(KeyCode.D)) D = true;
        if (Input.GetKey(KeyCode.LeftShift)) shift = true;
        if (Input.GetKey(KeyCode.LeftControl)) control = true;

        float horizontalInput = Input.GetAxis("Horizontal");
        //Get the value of the Horizontal input axis.

        float verticalInput = Input.GetAxis("Vertical");
        //Get the value of the Vertical input axis.

        float boost = Input.GetAxis("RightTriggerAxis") - Input.GetAxis("LeftTriggerAxis");

        if (shift && !control)
        {
            boost = 1f;
        }

        if (!shift && control)
        {
            boost = -1f;
        }


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

        if (verticalInput != 0)
        {
            vertCurrentRotationSpeed += verticalInput * vertRotAcc * Time.deltaTime;
        }

        if (vertCurrentRotationSpeed > 0f) //this applies at all times, even when inputting, might want to change this to only applying when there is no vertical input - Kai
        {
            vertCurrentRotationSpeed -= vertRotFriction * Time.deltaTime;
        }
        else
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

        if (horizontalInput != 0)
        {
            horiCurrentRotationSpeed += horizontalInput * horiRotAcc * Time.deltaTime;
        }

        if (horiCurrentRotationSpeed > 0f) //this also applies at all times, even when inputting, might want to change this to only applying when there is no vertical input - Kai
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


        boostMultiplier = boost * 6f;

        movementSpeed += basicFlap * boostMultiplier * (maxSpeed / (movementSpeed - 0.2f)) * Time.deltaTime;
        movementSpeed = Mathf.Clamp(movementSpeed, minSpeed, maxSpeed);
        gameObject.transform.position += dir * movementSpeed * Time.deltaTime;

    }

}
