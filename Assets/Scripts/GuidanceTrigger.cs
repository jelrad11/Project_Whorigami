using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidanceTrigger : MonoBehaviour
{
    public bool enable;
    public GameObject enableTrigger;
    public bool disable;
    public GameObject disableTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player"){
            if(enable) enableTrigger.SetActive(true);
            if(disable) disableTrigger.SetActive(false);

            gameObject.SetActive(false);
        }
    }
}
