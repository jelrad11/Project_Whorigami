using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_Paper : MonoBehaviour
{
    public float maxVelocity = 0.6f;
    public float rollSpeed = 5f;

    private float rotationSpeed = 4f;
    private float maxRotationSpeed = 6f;
    private float minRotationSpeed = 0f;

    public float slowDownSpeed = 1f;

    private bool flat = true;
    private bool longSide = false;

    public GameObject flatPaper;
    public GameObject rolledUp_Long;
    public GameObject rolledUp_Short;


    public GameObject posRefW;
    public GameObject posRefS;
    public GameObject posRefD;
    public GameObject posRefA;

    private float transfCooldown = 1f;

    // private float flatHorMoveSpeed = 0f;
    // private float floatFriction = 1f;
    private float vertRotationLimit = 20f;
    private float jumpstrengh = 0.5f;
    private float weakGravity = 9f; // reduces gravity for flatPaper from "9.81" to "9.81 - variable"
    private float jumpForward = 0.5f;

    // inputs
    float x_axis;
    float y_axis;

    bool transform_up;
    bool transform_down;
    bool transform_right;
    bool transform_left;

    bool transform_any;

    private float criticalSpeed = 0.05f;

    private bool lastDirectionForward = true; // true if forward, false if back
    private float lastVelocity = 0f;
    private bool lastFormLong = false; // false = short, true = long

    void getInput()
    {
        x_axis = Input.GetAxis("Horizontal");

        if (flat) {
            y_axis = Input.GetAxis("Vertical");
        } else
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

        transform_up = (Input.GetAxis("DPadVertical") > 0 || Input.GetButton("Button_bot"));
        transform_down = (Input.GetAxis("DPadVertical") < 0 || Input.GetButton("Button_bot"));
        transform_right = (Input.GetAxis("DPadHorizontal") > 0 || Input.GetButton("Button_left"));
        transform_left = (Input.GetAxis("DPadHorizontal") < 0 || Input.GetButton("Button_left"));

        transform_any = (transform_up || transform_down || transform_right || transform_left);
    }

    private void FixedUpdate()
    {
        if (flat)
        {
            flatPaper.GetComponent<Rigidbody>().AddForce(Vector3.up * weakGravity, ForceMode.Acceleration);
        }
    }

    private void flyG()
    {
        if (!flat) return;

        if (!lastFormLong)
        {
            if (y_axis != 0)
            {
                //flatPaper.transform.Rotate(100f * Time.deltaTime, 0, 0, Space.Self);
                flatPaper.GetComponent<Rigidbody>().AddTorque(flatPaper.transform.right * y_axis * Time.deltaTime * 150f, ForceMode.Acceleration);
            }

            if (flatPaper.GetComponent<Rigidbody>().velocity.magnitude > 0.1f && (x_axis != 0)) // Mathf.Abs(flatHorMoveSpeed) > 0.1f)
            {
                // flatPaper.transform.Rotate(0, -100f * Time.deltaTime, 0, Space.World);
                if (lastDirectionForward)
                {
                    flatPaper.GetComponent<Rigidbody>().AddTorque(transform.up * x_axis * Time.deltaTime * 150f, ForceMode.Acceleration);
                } else
                {
                    flatPaper.GetComponent<Rigidbody>().AddTorque(transform.up * x_axis * Time.deltaTime * -150f, ForceMode.Acceleration);
                }
            }

            Vector3 dirAngle = flatPaper.transform.rotation.eulerAngles;
            // Vector3 dir = flatPaper.transform.forward;
            // dir = new Vector3(dir.x, 0, dir.z);

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

            // Debug.Log(angleRad);

            // flatHorMoveSpeed += Mathf.Sin(angleRad) * Time.deltaTime * 25f;
            // flatHorMoveSpeed -= floatFriction * Time.deltaTime * Mathf.Sign(flatHorMoveSpeed);

            // flatHorMoveSpeed = Mathf.Clamp(flatHorMoveSpeed, -1, 1);
            // flatPaper.transform.position += dir * flatHorMoveSpeed * Time.deltaTime;

            flatPaper.GetComponent<Rigidbody>().AddForce(flatPaper.transform.forward * Time.deltaTime * angleRad * 400f, ForceMode.Acceleration);

        } else
        {
            if (y_axis != 0)
            {
                //flatPaper.transform.Rotate(100f * Time.deltaTime, 0, 0, Space.Self);
                flatPaper.GetComponent<Rigidbody>().AddTorque(flatPaper.transform.forward * y_axis * Time.deltaTime * 150f, ForceMode.Acceleration);
            }

            if (flatPaper.GetComponent<Rigidbody>().velocity.magnitude > 0.1f && (x_axis != 0)) // Mathf.Abs(flatHorMoveSpeed) > 0.1f)
            {
                // flatPaper.transform.Rotate(0, -100f * Time.deltaTime, 0, Space.World);
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
            // Vector3 dir = flatPaper.transform.forward;
            // dir = new Vector3(dir.x, 0, dir.z);

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

            // Debug.Log(angleRad);

            // flatHorMoveSpeed += Mathf.Sin(angleRad) * Time.deltaTime * 25f;
            // flatHorMoveSpeed -= floatFriction * Time.deltaTime * Mathf.Sign(flatHorMoveSpeed);

            // flatHorMoveSpeed = Mathf.Clamp(flatHorMoveSpeed, -1, 1);
            // flatPaper.transform.position += dir * flatHorMoveSpeed * Time.deltaTime;

            flatPaper.GetComponent<Rigidbody>().AddForce(flatPaper.transform.right * -1f * Time.deltaTime * angleRad * 400f, ForceMode.Acceleration);
        }
    }

    void Update()
    {
        getInput();
        rollUp();
        rolledUpMovement();
        checkVelocity();
        flyG();

        transfCooldown -= Time.deltaTime;

        if (Input.GetKey(KeyCode.J))
        {
            Debug.Log(flatPaper.transform.right);
            Debug.Log(flatPaper.transform.forward);
        }

    }



    private void checkVelocity()
    {

        Rigidbody RB;

        if (longSide)
        {
            RB = rolledUp_Long.GetComponent<Rigidbody>();
        }
        else if (!flat)
        {
            RB = rolledUp_Short.GetComponent<Rigidbody>();
        }
        else
        {
            RB = flatPaper.GetComponent<Rigidbody>();
        }

        if (RB.velocity.magnitude > maxVelocity)
        {
            RB.velocity = RB.velocity.normalized * maxVelocity;
        }

        /*
        if (longSide){
            Rigidbody longRB = rolledUp_Long.GetComponent<Rigidbody>();
            if(longRB.velocity.magnitude > maxVelocity){
                longRB.velocity = longRB.velocity.normalized * maxVelocity;
            }
            curVelocity = longRB.velocity.magnitude;
        }
        else if(!flat){
            Rigidbody shortRB = rolledUp_Short.GetComponent<Rigidbody>();
            if(shortRB.velocity.magnitude > maxVelocity){
                shortRB.velocity = shortRB.velocity.normalized * maxVelocity;
            }
            curVelocity = shortRB.velocity.magnitude;
        }
        */
    }

    private void Jump()
    {
        float forwardBoost = lastVelocity / maxVelocity;

        if (!lastFormLong) // last form is short
        {
            if (lastDirectionForward)
            {
                flatPaper.GetComponent<Rigidbody>().AddForce(Vector3.up * jumpstrengh + flatPaper.transform.forward * jumpForward * forwardBoost, ForceMode.VelocityChange);
                flatPaper.transform.Rotate(10 * forwardBoost + 5, 0, 0, Space.Self);
            } else
            {
                flatPaper.GetComponent<Rigidbody>().AddForce(Vector3.up * jumpstrengh - flatPaper.transform.forward * jumpForward * forwardBoost, ForceMode.VelocityChange);
                flatPaper.transform.Rotate(-10 * forwardBoost - 5, 0, 0, Space.Self);
            }
        } else
        {
            if (lastDirectionForward)
            {
                flatPaper.GetComponent<Rigidbody>().AddForce(Vector3.up * jumpstrengh + flatPaper.transform.right *-1f * jumpForward * forwardBoost, ForceMode.VelocityChange);
                flatPaper.transform.Rotate(0, 0, 10 * forwardBoost + 5, Space.Self);
            }
            else
            {
                flatPaper.GetComponent<Rigidbody>().AddForce(Vector3.up * jumpstrengh - flatPaper.transform.right * -1f * jumpForward * forwardBoost, ForceMode.VelocityChange);
                flatPaper.transform.Rotate(0, 0, -10 * forwardBoost - 5, Space.Self);
            }
        }
    }

    private void rolledUpRotation()
    {
        if (longSide)
        {
            Rigidbody longRb = rolledUp_Long.GetComponent<Rigidbody>();

            Vector3 vel_without_y = new Vector3(longRb.velocity.x, 0f, longRb.velocity.z);

            float rotation0 = rotationSpeed * vel_without_y.magnitude;
            rotation0 = Mathf.Clamp(rotation0, minRotationSpeed, maxRotationSpeed);
            Vector3 rotation = new Vector3(0f, 90f, 0f) * rotation0 * Time.deltaTime;

            bool forward = (Vector3.Angle(vel_without_y, flatPaper.transform.right * -1) < 10f);
            // Debug.Log(forward);

            bool backward = (Vector3.Angle(vel_without_y, flatPaper.transform.right * 1) < 10f);

            lastDirectionForward = !backward;
            lastVelocity = vel_without_y.magnitude;

            // Debug.Log(backward);

            if ((longRb.velocity.magnitude > criticalSpeed) && (forward) && (x_axis != 0))
            {
                rolledUp_Long.transform.Rotate(x_axis * rotation, Space.World);
            }
            else if ((longRb.velocity.magnitude > criticalSpeed) && (backward) && (x_axis != 0))
            {
                rolledUp_Long.transform.Rotate(rotation * -1f * x_axis, Space.World);
            }

            synchroniseObjects(1f);
        }
        else
        {
            Rigidbody shortRb = rolledUp_Short.GetComponent<Rigidbody>();

            Vector3 vel_without_y = new Vector3(shortRb.velocity.x, 0f, shortRb.velocity.z);

            float rotation0 = rotationSpeed * vel_without_y.magnitude;
            rotation0 = Mathf.Clamp(rotation0, minRotationSpeed, maxRotationSpeed);
            Vector3 rotation = new Vector3(0f, 90f, 0f) * rotation0 * Time.deltaTime;

            bool forward = (Vector3.Angle(vel_without_y, flatPaper.transform.right * -1) < 10f);
            // Debug.Log(forward);

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

    private void rolledUpMovement()
    {
        if (!flat)
        {
            if (longSide)
            {
                if (y_axis != 0)
                {
                    rolledUp_Long.GetComponent<Rigidbody>().AddForce(flatPaper.transform.right * Time.deltaTime * rollSpeed * y_axis * -1f);
                }
                else rolledUp_Long.GetComponent<Rigidbody>().velocity -= rolledUp_Long.GetComponent<Rigidbody>().velocity * slowDownSpeed * Time.deltaTime; // drag
            }
            else
            {
                if (y_axis != 0)
                {
                    rolledUp_Short.GetComponent<Rigidbody>().AddForce(flatPaper.transform.right * -1f * Time.deltaTime * rollSpeed * y_axis);
                }
                else rolledUp_Short.GetComponent<Rigidbody>().velocity -= rolledUp_Short.GetComponent<Rigidbody>().velocity * slowDownSpeed * Time.deltaTime;
            }

            rolledUpRotation();
        }
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

                Jump();
            }
        }
    }

    private void synchroniseObjects(float choice, float direction = -1f)
    {  //0 == flatPaper; 1 == rolledUp_Long; 2 == rolledUp_Short; //0 == W; 1 == S; 2 == A; 3 == D;
        Vector3 paperPosition;
        switch (choice)
        {
            case 0:
                paperPosition = new Vector3(flatPaper.transform.position.x, flatPaper.transform.position.y + 0.25f, flatPaper.transform.position.z);
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

                // flatPaper.transform.LookAt(flatPaper.transform.position + rolledUp_Long.transform.up);
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

    private void rolledUpRotation0()
    {
        Vector3 rotation = new Vector3(0f, 90f, 0f) * rotationSpeed * Time.deltaTime;
        Rigidbody longRb = rolledUp_Long.GetComponent<Rigidbody>();
        Rigidbody shortRb = rolledUp_Short.GetComponent<Rigidbody>();

        float directions = Input.GetAxis("Horizontal");

        if (Input.GetKey(KeyCode.W) || Input.GetAxis("RightTriggerAxis") > 0f)
        {
            if (longSide) rolledUp_Long.transform.Rotate(rotation * directions, Space.World);
            else rolledUp_Short.transform.Rotate(rotation * directions, Space.World);
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetAxis("LeftTriggerAxis") > 0f)
        {
            if (longSide) rolledUp_Long.transform.Rotate(rotation * -1f * directions, Space.World);
            else rolledUp_Short.transform.Rotate(rotation * -1f * directions, Space.World);
        }
        else if (directions != 0f)
        {
            if (longSide) rolledUp_Long.transform.Rotate(rotation * directions, Space.World);
            else rolledUp_Short.transform.Rotate(rotation * directions, Space.World);
        }
        if (longSide) synchroniseObjects(1f);
        else synchroniseObjects(2f);

    }
    private void rolledUpMovement0()
    {

        float forwardDir = Input.GetAxis("RightTriggerAxis");
        if (Input.GetAxis("RightTriggerAxis") > 0f) forwardDir = Input.GetAxis("RightTriggerAxis");
        else if (Input.GetAxis("LeftTriggerAxis") > 0f) forwardDir = -1f * Input.GetAxis("LeftTriggerAxis");

        if (Input.GetKey(KeyCode.W)) forwardDir = 1f;
        else if (Input.GetKey(KeyCode.S)) forwardDir = -1f;

        Vector3 r = flatPaper.transform.right;
        Vector3 rightTransl = new Vector3(-r.z, r.y, r.x);

        if (!flat)
        {
            if (longSide)
            {
                if (forwardDir != 0f) rolledUp_Long.GetComponent<Rigidbody>().AddForce(flatPaper.transform.right * -1f * forwardDir * Time.deltaTime * rollSpeed);
                else rolledUp_Long.GetComponent<Rigidbody>().velocity -= rolledUp_Long.GetComponent<Rigidbody>().velocity * slowDownSpeed * Time.deltaTime;
            }
            else
            {
                if (forwardDir != 0f) rolledUp_Short.GetComponent<Rigidbody>().AddForce(flatPaper.transform.right * -1f * forwardDir * Time.deltaTime * rollSpeed);
                else rolledUp_Short.GetComponent<Rigidbody>().velocity -= rolledUp_Short.GetComponent<Rigidbody>().velocity * slowDownSpeed * Time.deltaTime;
            }

            rolledUpRotation();
        }
    }

}