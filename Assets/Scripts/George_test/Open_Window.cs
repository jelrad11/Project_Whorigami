using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Open_Window : MonoBehaviour
{
    public float angle = 0f;
    public float closingAngle = 10f;
    float speed = 100f;
    public bool moving = false;
    public GameObject window;

    private float timer = 0f;
    private float delay = 0.5f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!moving) return;

        if (timer < delay)
        {
            timer += Time.deltaTime;
        }

        if ((timer > delay) && (angle < closingAngle))
        {
            window.transform.Rotate(Vector3.up, Time.deltaTime * speed);
            angle += Time.deltaTime * speed;
        }
    }

    public void Shut()
    {
        moving = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Shut();
        }
    }

}
