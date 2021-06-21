using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardPointerFlat : MonoBehaviour
{
    public GameObject flatPaper;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = flatPaper.transform.position + flatPaper.transform.up * 0.2f;
        transform.LookAt(flatPaper.transform.position + new Vector3(0, 1, 0), flatPaper.transform.right);
    }
}
