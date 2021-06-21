using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_Paper : MonoBehaviour
{
    public float rollSpeed = 5f;
    public float rotationSpeed = 5f;
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


    private float x_axis;
    private float y_axis;
    private bool button0;
    private bool button1;
    private bool button2;
    private bool button3;

    private float deadzone = 0f;

    
    private float cooldown = 0.5f;
    private float cooldownTimer = 1f;
    private bool onCooldown = false;
    

    void Update()
    {
        getInput();
        checkCooldown();
        rollUp();
        rolledUpMovement();
        realign();

        //   Debug.Log(flatPaper.transform.rotation.eulerAngles.y - rolledUp_Long.transform.rotation.eulerAngles.y);
    }

    private void checkCooldown()
    {
        cooldownTimer += Time.deltaTime;
        if (cooldownTimer > cooldown)
        {
            onCooldown = false;
        }
    }

    private void realign()
    {
        if (longSide)
        {
            flatPaper.transform.LookAt(flatPaper.transform.position + rolledUp_Long.transform.up);
        } else if (!flat)
        {
            flatPaper.transform.LookAt(flatPaper.transform.position + rolledUp_Short.transform.up);
            flatPaper.transform.Rotate(new Vector3(0,90,0),Space.Self);
        }
    }

    private void getInput()
    {

        x_axis = Input.GetAxis("Horizontal");
        if (Input.GetKey(KeyCode.A))
        {
            x_axis = -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            x_axis = 1;
        }

        // y_axis = Input.GetAxis("Vertical");

        y_axis = Input.GetAxis("Triggers");
        if (Input.GetKey(KeyCode.W))
        {
            y_axis = 1;
        } else if (Input.GetKey(KeyCode.S))
        {
            y_axis = -1;
        }

        button0 = Input.GetButtonDown("button0") || Input.GetKeyDown(KeyCode.DownArrow);
        button1 = Input.GetButtonDown("button1") || Input.GetKeyDown(KeyCode.RightArrow);
        button2 = Input.GetButtonDown("button2") || Input.GetKeyDown(KeyCode.LeftArrow);
        button3 = Input.GetButtonDown("button3") || Input.GetKeyDown(KeyCode.UpArrow);

    }

    private void rolledUpRotation()
    {
        if (longSide)
        {
            Rigidbody longRb = rolledUp_Long.GetComponent<Rigidbody>();

            rotationSpeed = 1f * longRb.velocity.magnitude;
            rotationSpeed = Mathf.Clamp(rotationSpeed, 0.1f, 1f);
            Vector3 rotation = new Vector3(0f, 90f, 0f) * rotationSpeed * Time.deltaTime;

            bool forward;
            Vector3 vel_without_y = new Vector3(longRb.velocity.x, 0f, longRb.velocity.z);
            forward = (Vector3.Angle(vel_without_y, flatPaper.transform.right * -1) < 10f);
            // Debug.Log(forward);

            bool backward;
            backward = (Vector3.Angle(vel_without_y, flatPaper.transform.right * 1) < 10f);

            // Debug.Log(backward);

            if ((longRb.velocity.magnitude > 0.1f) && (forward))
            {
                if (Input.GetKey(KeyCode.A) || x_axis < -deadzone)
                {
                    rolledUp_Long.transform.Rotate(x_axis * rotation * 1f, Space.World);
                }
                else if (Input.GetKey(KeyCode.D) || x_axis > deadzone)
                {
                    rolledUp_Long.transform.Rotate(x_axis * rotation, Space.World);
                }
            }
            else if ((longRb.velocity.magnitude > 0.1f) && (backward))
            {
                if (Input.GetKey(KeyCode.D) || x_axis > deadzone)
                {
                    rolledUp_Long.transform.Rotate(rotation * -1f * x_axis, Space.World);
                }
                else if (Input.GetKey(KeyCode.A) || x_axis < -deadzone)
                {
                    rolledUp_Long.transform.Rotate(rotation * x_axis * -1f, Space.World);
                }
            }
        } else
        {
            Rigidbody shortRb = rolledUp_Short.GetComponent<Rigidbody>();

            rotationSpeed = 1f * shortRb.velocity.magnitude;
            rotationSpeed = Mathf.Clamp(rotationSpeed, 0.1f, 1f);
            Vector3 rotation = new Vector3(0f, 90f, 0f) * rotationSpeed * Time.deltaTime;

            bool forward;
            Vector3 vel_without_y = new Vector3(shortRb.velocity.x, 0f, shortRb.velocity.z);
            forward = (Vector3.Angle(vel_without_y, flatPaper.transform.forward * 1) < 10f);
            // Debug.Log(forward);

            bool backward;
            backward = (Vector3.Angle(vel_without_y, flatPaper.transform.forward * -1) < 10f);

            if ((shortRb.velocity.magnitude > 0.1f) && (forward))
            {
                if (Input.GetKey(KeyCode.A) || x_axis < -deadzone)
                {
                    rolledUp_Short.transform.Rotate(x_axis * rotation * 1f, Space.World);
                }
                else if (Input.GetKey(KeyCode.D) || x_axis > deadzone)
                {
                    rolledUp_Short.transform.Rotate(x_axis * rotation, Space.World);
                }
            }
            else if ((shortRb.velocity.magnitude > 0.1f) && (backward))
            {
                if (Input.GetKey(KeyCode.D) || x_axis > deadzone)
                {
                    rolledUp_Short.transform.Rotate(rotation * -1f * x_axis, Space.World);
                }
                else if (Input.GetKey(KeyCode.A) || x_axis < -deadzone)
                {
                    rolledUp_Short.transform.Rotate(rotation * x_axis * -1f, Space.World);
                }
            }

        }

        /*
        if ((longRb.velocity.magnitude > 0.1f) && (forward))
        // if (Input.GetKey(KeyCode.W) || y_axis > deadzone)
        {
            if (longSide)
            {
                if (Input.GetKey(KeyCode.A) || x_axis < -deadzone)
                {
                    rolledUp_Long.transform.Rotate(x_axis * rotation * 1f, Space.World);
                }
                else if (Input.GetKey(KeyCode.D) || x_axis > deadzone)
                {
                    rolledUp_Long.transform.Rotate(x_axis * rotation, Space.World);
                }
            }
            else
            {
                if (Input.GetKey(KeyCode.A))
                {
                    rolledUp_Short.transform.Rotate(rotation * -1f, Space.World);
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    rolledUp_Short.transform.Rotate(rotation, Space.World);
                }
            }
        }
        else if (Input.GetKey(KeyCode.S) || y_axis < -deadzone)
        {
            if (longSide)
            {
                if (Input.GetKey(KeyCode.D) || x_axis > deadzone)
                {
                    rolledUp_Long.transform.Rotate(rotation * -1f * x_axis, Space.World);
                }
                else if (Input.GetKey(KeyCode.A) || x_axis < deadzone)
                {
                    rolledUp_Long.transform.Rotate(rotation * x_axis * -1f, Space.World);
                }
            }
            else
            {
                if (Input.GetKey(KeyCode.D))
                {
                    rolledUp_Short.transform.Rotate(rotation * -1f, Space.World);
                    // flatPaper.transform.Rotate(rotation * -1f, Space.World);
                }
                else if (Input.GetKey(KeyCode.A))
                {
                    rolledUp_Short.transform.Rotate(rotation, Space.World);
                    // flatPaper.transform.Rotate(rotation, Space.World);
                }
            }
        }
        */
    }
    private void rolledUpMovement()
    {
        if (!flat)
        {
            if (longSide)
            {
                if (Input.GetKey(KeyCode.S) || y_axis < -deadzone)
                {
                    rolledUp_Long.GetComponent<Rigidbody>().AddForce(flatPaper.transform.right * Time.deltaTime * rollSpeed * y_axis * -1f);
                }
                else if (Input.GetKey(KeyCode.W) || y_axis > deadzone)
                {
                    rolledUp_Long.GetComponent<Rigidbody>().AddForce(flatPaper.transform.right * Time.deltaTime * rollSpeed * y_axis * -1f);
                }
                // else rolledUp_Long.GetComponent<Rigidbody>().velocity -= rolledUp_Long.GetComponent<Rigidbody>().velocity * slowDownSpeed * Time.deltaTime; // use rigidbody drag instead
            }
            else
            {
                if (Input.GetKey(KeyCode.W) || y_axis < -deadzone)
                {
                    rolledUp_Short.GetComponent<Rigidbody>().AddForce(flatPaper.transform.forward * Time.deltaTime * rollSpeed * y_axis);
                }
                else if (Input.GetKey(KeyCode.S) || y_axis > deadzone)
                {
                    rolledUp_Short.GetComponent<Rigidbody>().AddForce(flatPaper.transform.forward * Time.deltaTime * rollSpeed * y_axis);
                }
                // else rolledUp_Short.GetComponent<Rigidbody>().velocity -= rolledUp_Short.GetComponent<Rigidbody>().velocity * slowDownSpeed * Time.deltaTime;
            }

            rolledUpRotation();
        }
    }

    private void rollUp()
    {
        if (onCooldown) return;

        if (flat)
        {
            if (button3 || button0)
            {
                flatPaper.SetActive(false);
                rolledUp_Long.SetActive(false);
                rolledUp_Short.SetActive(true);

                rolledUp_Short.GetComponent<Rigidbody>().velocity = new Vector3();

                if (button3) synchroniseObjects(0f, 0f);
                else if (button0) synchroniseObjects(0f, 1f);

                flat = false;
                longSide = false;

                cooldownTimer = 0;
                onCooldown = true;
            }
            else if (button1 || button2)
            {
                flatPaper.SetActive(false);
                rolledUp_Long.SetActive(true);
                rolledUp_Short.SetActive(false);

                rolledUp_Long.GetComponent<Rigidbody>().velocity = new Vector3();

                if (button1) synchroniseObjects(0f, 3f);
                else if (button2) synchroniseObjects(0f, 2f);

                flat = false;
                longSide = true;

                cooldownTimer = 0;
                onCooldown = true;
            }
        }
        else
        {
            if (button0 || button1 || button2 || button3)
            {
                flatPaper.SetActive(true);
                rolledUp_Long.SetActive(false);
                rolledUp_Short.SetActive(false);

                if (longSide) synchroniseObjects(1f);
                else synchroniseObjects(2f);

                flat = true;
                longSide = false;

                cooldownTimer = 0;
                onCooldown = true;
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

                rolledUp_Long.transform.rotation = Quaternion.Euler(90f, flatPaper.transform.rotation.eulerAngles.y, 0f);
                rolledUp_Short.transform.rotation = Quaternion.Euler(0f, flatPaper.transform.rotation.eulerAngles.y, 90f);
                break;
            case 1:
                paperPosition = new Vector3(rolledUp_Long.transform.position.x, rolledUp_Long.transform.position.y, rolledUp_Long.transform.position.z);
                flatPaper.transform.position = paperPosition;
                rolledUp_Short.transform.position = paperPosition;

                //flatPaper.transform.rotation = Quaternion.Euler(0f, rolledUp_Long.transform.rotation.eulerAngles.y + 90f, 0f);
                rolledUp_Short.transform.rotation = Quaternion.Euler(0f, rolledUp_Long.transform.rotation.eulerAngles.y, 90f);
                break;
            case 2:
                paperPosition = new Vector3(rolledUp_Short.transform.position.x, rolledUp_Short.transform.position.y, rolledUp_Short.transform.position.z);
                rolledUp_Long.transform.position = paperPosition;
                flatPaper.transform.position = paperPosition;

                rolledUp_Long.transform.rotation = Quaternion.Euler(90f, rolledUp_Short.transform.rotation.eulerAngles.y, 0f);
                //flatPaper.transform.rotation = Quaternion.Euler(0f, rolledUp_Short.transform.rotation.eulerAngles.y, 0f);
                break;
        }
    }
    private Vector3 synchroniseObjectPos(float direction)
    { //0 == W; 1 == S; 2 == A; 3 == D;
        switch (direction)
        {
            case 0:
                return new Vector3(posRefW.transform.position.x, posRefW.transform.position.y + (rolledUp_Long.transform.localScale.y / 2f) - (flatPaper.transform.localScale.y), posRefW.transform.position.z);
            case 1:
                return new Vector3(posRefS.transform.position.x, posRefS.transform.position.y + (rolledUp_Long.transform.localScale.y / 2f) - (flatPaper.transform.localScale.y), posRefS.transform.position.z);
            case 2:
                return new Vector3(posRefA.transform.position.x, posRefA.transform.position.y + (rolledUp_Long.transform.localScale.y / 2f) - (flatPaper.transform.localScale.y), posRefA.transform.position.z);
            case 3:
                return new Vector3(posRefD.transform.position.x, posRefD.transform.position.y + (rolledUp_Long.transform.localScale.y / 2f) - (flatPaper.transform.localScale.y), posRefD.transform.position.z);
            default:
                return new Vector3();
        }
    }
}