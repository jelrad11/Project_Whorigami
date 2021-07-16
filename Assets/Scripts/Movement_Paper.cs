using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_Paper : MonoBehaviour
{
    public float maxVelocityRoll = 0.4f;
    public float maxVelocityFlat = 1.2f;
    public float rollSpeed = 5f;

    private float rotationSpeed = 6f;
    private float maxRotationSpeed = 6f;
    private float minRotationSpeed = 0f;

    public float slowDownSpeed = 1f;

    public bool flat = true;
    public bool longSide = false;

    public GameObject flatPaper;
    public GameObject rolledUp_Long;
    public GameObject rolledUp_Short;

    public GameObject posRefW;
    public GameObject posRefS;
    public GameObject posRefD;
    public GameObject posRefA;

    private float transfCooldown = 1f;

    private float vertRotationLimit = 20f;
    private float jumpstrengh = 0.5f;
    private float weakGravity = 9f; // reduces gravity for flatPaper from "9.81" to "9.81 - variable" for flatPaper
    private float jumpForward = 0.5f;

    private float criticalSpeed = 0.05f;

    // state of roll before transforming into flat
    private bool lastDirectionForward = true; // true if forward, false if back
    private float lastVelocity = 0f;
    private bool lastFormLong = false; // false = short, true = long

    // inputs
    float x_axis;
    float y_axis;

    bool transform_up;
    bool transform_down;
    bool transform_right;
    bool transform_left;
    bool transform_any;

    // variables about what the paper can do through story progression

    public bool canTransformLong;
    public bool canTransformShort;
    public bool canFly;
    public bool canTurn;

    // audio stuff
    public AudioSource audioSource;
    public AudioClip roll_up;
    public AudioClip roll_down;

    public void unrestrictRb()
    {
        rolledUp_Long.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        flatPaper.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
    }

    void Update()
    {
        if(canTurn) unrestrictRb();
        
        getInput();
        rollUp();

        if (canFly)
        {
            flatPaperFly();
        }

        transfCooldown -= Time.deltaTime;
        
        // debug test
        /*
        if (Input.GetKeyDown(KeyCode.G))
        {
            unrestrictRb();
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            canTurn = true;
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            canTransformLong = true;
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            canTransformShort = true;
        }
        
        if (Input.GetKeyDown(KeyCode.U))
        {
            canFly = true;
        }
        */
    }

    private void FixedUpdate()
    {
        if (flat)
        {
            flatPaper.GetComponent<Rigidbody>().AddForce(Vector3.up * weakGravity, ForceMode.Acceleration);
        }
        else
        {
            if (longSide)
            {
                rolledUp_Long.GetComponent<Rigidbody>().AddForce(Vector3.up * 4f, ForceMode.Acceleration);
            }
            else
            {
                rolledUp_Short.GetComponent<Rigidbody>().AddForce(Vector3.up * 4f, ForceMode.Acceleration);
            }
        }
        rolledUpMovement();
        checkVelocity();
    }

    void getInput()
    {
        x_axis = Input.GetAxis("Horizontal");


        if (flat)
        {
            y_axis = Input.GetAxis("Vertical");
        }
        else
        {
            y_axis = Input.GetAxis("RightTriggerAxis") - Input.GetAxis("LeftTriggerAxis");
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                y_axis = 1;
            }
            else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                y_axis = -1;
            }
        }

        if (canTransformShort)
        {
            transform_up = (Input.GetAxis("DPadVertical") > 0 || Input.GetButtonDown("Button_bot"));
            transform_down = (Input.GetAxis("DPadVertical") < 0 || Input.GetButtonDown("Button_bot"));
        }
        else
        {
            transform_up = false;
            transform_down = false;
        }

        if (canTransformLong)
        {
            transform_right = (Input.GetAxis("DPadHorizontal") > 0 || Input.GetButtonDown("Button_left"));
            transform_left = (Input.GetAxis("DPadHorizontal") < 0 || Input.GetButtonDown("Button_left"));
        }
        else
        {
            transform_left = false;
            transform_right = false;
        }

        transform_any = (transform_up || transform_down || transform_right || transform_left);
    }
    private void rollUp()
    {
        if (flat)
        {
            if ((transform_up || transform_down) && transfCooldown <= 0f)
            {
                flatPaper.SetActive(false);
                rolledUp_Long.SetActive(false);
                rolledUp_Short.SetActive(true);

                rolledUp_Short.GetComponent<Rigidbody>().velocity = new Vector3();

                if (transform_up) synchroniseObjects(0f, 0f);
                else if (transform_down) synchroniseObjects(0f, 1f);

                flat = false;
                longSide = false;

                transfCooldown = 1f;

                audioSource.clip = roll_up;
                audioSource.pitch = Random.Range(0.8f, 1.2f);
                audioSource.Play();
            }
            else if ((transform_left || transform_right) && transfCooldown <= 0f)
            {
                flatPaper.SetActive(false);
                rolledUp_Long.SetActive(true);
                rolledUp_Short.SetActive(false);

                rolledUp_Long.GetComponent<Rigidbody>().velocity = new Vector3();

                if (transform_right) synchroniseObjects(0f, 3f);
                else if (transform_left) synchroniseObjects(0f, 2f);

                flat = false;
                longSide = true;

                transfCooldown = 1f;

                audioSource.clip = roll_up;
                audioSource.pitch = Random.Range(0.8f, 1.2f);
                audioSource.Play();
            }
        }
        else
        {
            if (transform_any && transfCooldown <= 0f)
            {
                flatPaper.SetActive(true);
                rolledUp_Long.SetActive(false);
                rolledUp_Short.SetActive(false);

                if (longSide) synchroniseObjects(1f);
                else synchroniseObjects(2f);

                Quaternion rot;
                if (longSide)
                {
                    rot = Quaternion.Euler(0f, rolledUp_Long.transform.rotation.eulerAngles.y, 0f);
                    flatPaper.transform.rotation = new Quaternion(0f, rot.y, 0f, rot.w);

                    lastFormLong = true;
                }
                else
                {
                    Quaternion rot2 = Quaternion.Euler(0f, rolledUp_Short.transform.rotation.eulerAngles.y - 90f, 0f);
                    flatPaper.transform.rotation = new Quaternion(0f, rot2.y, 0f, rot2.w);

                    lastFormLong = false;
                }
                flat = true;
                longSide = false;

                transfCooldown = 1f;

                audioSource.clip = roll_down;
                audioSource.pitch = Random.Range(0.8f, 1.2f);
                audioSource.Play();

                Jump();
            }
        }
    }

    private void Jump()
    {
        Rigidbody flatRb = flatPaper.GetComponent<Rigidbody>();

        flatRb.velocity = Vector3.zero;
        flatRb.angularVelocity = Vector3.zero;

        float forwardBoost = (lastVelocity / maxVelocityRoll) * 1.5f;
        forwardBoost = Mathf.Clamp(forwardBoost, 0, 1);

        if (lastFormLong == false) // last form is short
        {
            if (lastDirectionForward)
            {
                flatRb.AddForce(Vector3.up * jumpstrengh + flatPaper.transform.forward * jumpForward * forwardBoost, ForceMode.VelocityChange);
                flatPaper.transform.Rotate(15 * forwardBoost, 0, 0, Space.Self);
            }
            else
            {
                flatRb.AddForce(Vector3.up * jumpstrengh - flatPaper.transform.forward * jumpForward * forwardBoost, ForceMode.VelocityChange);
                flatPaper.transform.Rotate(-15 * forwardBoost, 0, 0, Space.Self);
            }
        }
        else
        {
            if (lastDirectionForward)
            {
                flatRb.AddForce(Vector3.up * jumpstrengh + flatPaper.transform.right * -1f * jumpForward * forwardBoost, ForceMode.VelocityChange);
                flatPaper.transform.Rotate(0, 0, 15 * forwardBoost, Space.Self);
            }
            else
            {
                flatRb.AddForce(Vector3.up * jumpstrengh - flatPaper.transform.right * -1f * jumpForward * forwardBoost, ForceMode.VelocityChange);
                flatPaper.transform.Rotate(0, 0, -15 * forwardBoost, Space.Self);
            }
        }
    }

    private void rolledUpMovement()
    {
        if (!flat)
        {
            if (longSide)
            {
                if (y_axis != 0)
                {
                    rolledUp_Long.GetComponent<Rigidbody>().AddForce(flatPaper.transform.right * Time.fixedDeltaTime * rollSpeed * y_axis * -1f);
                }
                else
                {
                    //   rolledUp_Long.GetComponent<Rigidbody>().velocity -= rolledUp_Long.GetComponent<Rigidbody>().velocity * slowDownSpeed * Time.deltaTime;
                }
            }
            else
            {
                if (y_axis != 0)
                {
                    rolledUp_Short.GetComponent<Rigidbody>().AddForce(flatPaper.transform.right * -1f * Time.fixedDeltaTime * rollSpeed * y_axis);
                }
                else
                {
                    //   rolledUp_Short.GetComponent<Rigidbody>().velocity -= rolledUp_Short.GetComponent<Rigidbody>().velocity * slowDownSpeed * Time.deltaTime;
                }
            }

            rolledUpRotation();

        }
    }

    private void rolledUpRotation()
    {
        if (longSide)
        {
            if (canTurn)
            {
                Rigidbody longRb = rolledUp_Long.GetComponent<Rigidbody>();

                Vector3 vel_without_y = new Vector3(longRb.velocity.x, 0f, longRb.velocity.z);

                float rotation0 = rotationSpeed * vel_without_y.magnitude;
                rotation0 = Mathf.Clamp(rotation0, minRotationSpeed, maxRotationSpeed);
                Vector3 rotation = new Vector3(0f, 90f, 0f) * rotation0 * Time.fixedDeltaTime;

                bool forward = (Vector3.Angle(vel_without_y, flatPaper.transform.right * -1) < 10f);
                bool backward = (Vector3.Angle(vel_without_y, flatPaper.transform.right * 1) < 10f);

                lastDirectionForward = !backward;
                lastVelocity = vel_without_y.magnitude;

                if ((longRb.velocity.magnitude > criticalSpeed) && (forward) && (x_axis != 0))
                {
                    rolledUp_Long.transform.Rotate(x_axis * rotation, Space.World);
                }
                else if ((longRb.velocity.magnitude > criticalSpeed) && (backward) && (x_axis != 0))
                {
                    rolledUp_Long.transform.Rotate(rotation * -1f * x_axis, Space.World);
                }
            }
            synchroniseObjects(1f);
        }
        else
        {
            if (canTurn)
            {
                Rigidbody shortRb = rolledUp_Short.GetComponent<Rigidbody>();

                Vector3 vel_without_y = new Vector3(shortRb.velocity.x, 0f, shortRb.velocity.z);

                float rotation0 = rotationSpeed * vel_without_y.magnitude;
                rotation0 = Mathf.Clamp(rotation0, minRotationSpeed, maxRotationSpeed);
                Vector3 rotation = new Vector3(0f, 90f, 0f) * rotation0 * Time.fixedDeltaTime;

                bool forward = (Vector3.Angle(vel_without_y, flatPaper.transform.right * -1) < 10f);
                bool backward = (Vector3.Angle(vel_without_y, flatPaper.transform.right * 1) < 10f);

                lastDirectionForward = !backward;
                lastVelocity = vel_without_y.magnitude;

                if ((shortRb.velocity.magnitude > criticalSpeed) && (forward) && (x_axis != 0))
                {
                    rolledUp_Short.transform.Rotate(x_axis * rotation, Space.World);
                }
                else if ((shortRb.velocity.magnitude > criticalSpeed) && (backward) && (x_axis != 0))
                {
                    rolledUp_Short.transform.Rotate(rotation * -1f * x_axis, Space.World);
                }
            }
            synchroniseObjects(2f);
        }
    }
    private void checkVelocity()
    {
        Rigidbody RB;
        if (!flat)
        {
            if (longSide)
            {
                RB = rolledUp_Long.GetComponent<Rigidbody>();
            }
            else
            {
                RB = rolledUp_Short.GetComponent<Rigidbody>();
            }

            if (RB.velocity.magnitude > maxVelocityRoll)
            {
                RB.velocity = RB.velocity.normalized * maxVelocityRoll;
            }

        }
        else
        {
            RB = flatPaper.GetComponent<Rigidbody>();

            if (RB.velocity.magnitude > maxVelocityFlat)
            {
                RB.velocity = RB.velocity.normalized * maxVelocityFlat;
            }
        }
    }

    private void flatPaperFly()
    {
        if (!flat) return;

        if (!lastFormLong)
        {
            if (y_axis != 0)
            {

                flatPaper.GetComponent<Rigidbody>().AddTorque(flatPaper.transform.right * y_axis * Time.deltaTime * 150f, ForceMode.Acceleration);
            }

            if (flatPaper.GetComponent<Rigidbody>().velocity.magnitude > 0.1f && (x_axis != 0))
            {
                if (lastDirectionForward)
                {
                    flatPaper.GetComponent<Rigidbody>().AddTorque(transform.up * x_axis * Time.deltaTime * 150f, ForceMode.Acceleration);
                }
                else
                {
                    flatPaper.GetComponent<Rigidbody>().AddTorque(transform.up * x_axis * Time.deltaTime * -150f, ForceMode.Acceleration);
                }
            }

            Vector3 dirAngle = flatPaper.transform.rotation.eulerAngles;

            float angle = dirAngle.x;

            if (angle > 180f) angle = angle - 360f;

            if (angle > vertRotationLimit)
            {
                while (angle > vertRotationLimit)
                {
                    flatPaper.transform.Rotate(-0.01f, 0, 0);
                    angle -= 0.01f;
                }
            }
            else if (angle < -vertRotationLimit)
            {
                while (angle < -vertRotationLimit)
                {
                    flatPaper.transform.Rotate(0.01f, 0, 0);
                    angle += 0.01f;
                }
            }

            float angleRad = (Mathf.PI / 180) * angle;
            flatPaper.GetComponent<Rigidbody>().AddForce(flatPaper.transform.forward * Time.deltaTime * angleRad * 400f, ForceMode.Acceleration);

        }
        else
        {
            if (y_axis != 0)
            {
                flatPaper.GetComponent<Rigidbody>().AddTorque(flatPaper.transform.forward * y_axis * Time.deltaTime * 150f, ForceMode.Acceleration);
            }

            if (flatPaper.GetComponent<Rigidbody>().velocity.magnitude > 0.1f && (x_axis != 0))
            {
                if (lastDirectionForward)
                {
                    flatPaper.GetComponent<Rigidbody>().AddTorque(transform.up * x_axis * Time.deltaTime * 150f, ForceMode.Acceleration);
                }
                else
                {
                    flatPaper.GetComponent<Rigidbody>().AddTorque(transform.up * x_axis * Time.deltaTime * -150f, ForceMode.Acceleration);
                }
            }

            Vector3 dirAngle = flatPaper.transform.rotation.eulerAngles;

            float angle = dirAngle.z;

            if (angle > 180f) angle = angle - 360f;

            if (angle > vertRotationLimit)
            {
                while (angle > vertRotationLimit)
                {
                    flatPaper.transform.Rotate(0, 0, -0.01f);
                    angle -= 0.01f;
                }
            }
            else if (angle < -vertRotationLimit)
            {
                while (angle < -vertRotationLimit)
                {
                    flatPaper.transform.Rotate(0, 0, 0.01f);
                    angle += 0.01f;
                }
            }

            float angleRad = (Mathf.PI / 180) * angle;
            flatPaper.GetComponent<Rigidbody>().AddForce(flatPaper.transform.right * -1f * Time.deltaTime * angleRad * 400f, ForceMode.Acceleration);
        }
    }

    private void synchroniseObjects(float choice, float direction = -1f)
    {  //0 == flatPaper; 1 == rolledUp_Long; 2 == rolledUp_Short; //0 == W; 1 == S; 2 == A; 3 == D;
        Vector3 paperPosition;
        switch (choice)
        {
            case 0:
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

                Quaternion rot2 = Quaternion.Euler(0f, rolledUp_Short.transform.rotation.eulerAngles.y, 0f);
                flatPaper.transform.rotation = new Quaternion(0f, rot2.y, 0f, rot2.w);

                rolledUp_Long.transform.rotation = rolledUp_Short.transform.rotation;

                break;
        }
    }
    private Vector3 synchroniseObjectPos(float direction)
    { //0 == W; 1 == S; 2 == A; 3 == D;
        switch (direction)
        {
            case 0:
                return new Vector3(posRefW.transform.position.x, posRefW.transform.position.y + (rolledUp_Long.transform.localScale.y / 4f) - (flatPaper.transform.localScale.y), posRefW.transform.position.z);
            case 1:
                return new Vector3(posRefS.transform.position.x, posRefS.transform.position.y + (rolledUp_Long.transform.localScale.y / 4f) - (flatPaper.transform.localScale.y), posRefS.transform.position.z);
            case 2:
                return new Vector3(posRefA.transform.position.x, posRefA.transform.position.y + (rolledUp_Long.transform.localScale.y / 4f) - (flatPaper.transform.localScale.y), posRefA.transform.position.z);
            case 3:
                return new Vector3(posRefD.transform.position.x, posRefD.transform.position.y + (rolledUp_Long.transform.localScale.y / 4f) - (flatPaper.transform.localScale.y), posRefD.transform.position.z);
            default:
                return new Vector3();
        }
    }

}
