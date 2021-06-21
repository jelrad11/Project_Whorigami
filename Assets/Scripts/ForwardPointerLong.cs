using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardPointerLong : MonoBehaviour
{

    public GameObject rolledUp_Long;
    public GameObject flatPaper;

    // Update is called once per frame
    void Update()
    {
        transform.position = rolledUp_Long.transform.position + flatPaper.transform.right * -0.4f;
        transform.LookAt(transform.position + new Vector3(0, 1, 0), flatPaper.transform.forward);
    }
}
