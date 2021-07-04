using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roll_Pointer : MonoBehaviour
{
    public GameObject rolledUp;
    public GameObject flatPaper;

    private float distance = 0.06f;

    // Update is called once per frame
    void Update()
    {   
        transform.position = rolledUp.transform.position + flatPaper.transform.right * -1f * distance;
        transform.LookAt(transform.position + new Vector3(0, 1, 0), flatPaper.transform.forward);
    }

}
