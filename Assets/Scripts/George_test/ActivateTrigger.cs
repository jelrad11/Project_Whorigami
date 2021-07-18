using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateTrigger : MonoBehaviour
{
    public GameObject trigger;
    public GameObject trigger2;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            trigger.SetActive(true);
            trigger2.SetActive(true);
        }
    }

}
