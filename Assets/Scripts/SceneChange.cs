using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public bool onTrigger;
    public string sceneToChangeTo;
    public float timeToChange;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player"){
            SceneManager.LoadScene(sceneToChangeTo, LoadSceneMode.Single);
        }
    }

    private void Start()
    {
        if(!onTrigger){
            StartCoroutine(sceneChangeTimer());
        }
    }

    private IEnumerator sceneChangeTimer(){
        yield return new WaitForSeconds(timeToChange);

        SceneManager.LoadScene(sceneToChangeTo, LoadSceneMode.Single);
    }
}
