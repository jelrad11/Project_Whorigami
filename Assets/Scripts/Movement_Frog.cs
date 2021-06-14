using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_Frog : MonoBehaviour
{
    public float jumpingStrength = 25f;

    // Update is called once per frame
    void Update()
    {
        jumpingMovement();
    }

    private void jumpingMovement(){

        Vector3 jumpVector = gameObject.transform.up + (gameObject.transform.forward * 0.5f);
        if(Input.GetKeyDown(KeyCode.Space)){
            gameObject.GetComponent<Rigidbody>().AddForce(jumpVector * jumpingStrength);
        }
    }
}
