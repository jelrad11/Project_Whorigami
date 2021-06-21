using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardPointerShort : MonoBehaviour
{

    public GameObject rolledUp_Short;
    public GameObject flatPaper;

    // Update is called once per frame
    void Update()
    {
        transform.position = rolledUp_Short.transform.position + flatPaper.transform.forward * 0.4f;
        transform.LookAt(transform.position + new Vector3(0, 1, 0), flatPaper.transform.right);
    }
}
