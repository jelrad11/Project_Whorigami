using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidanceTrigger : MonoBehaviour
{
    public bool enable;
    public List<GameObject> enableTrigger;
    public bool disable;
    public List<GameObject> disableTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player"){
            if(enable) for(int i = 0; i < enableTrigger.Count; i++)enableTrigger[i].SetActive(true);
            if(disable) for(int i = 0; i < disableTrigger.Count; i++)disableTrigger[i].SetActive(false);

            gameObject.SetActive(false);
        }
    }
}
