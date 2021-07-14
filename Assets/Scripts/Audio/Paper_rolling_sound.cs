using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paper_rolling_sound : MonoBehaviour
{
    public AudioSource rollSource;
    public AudioSource flySource;

    public GameObject player;
    private Movement_Paper script;

    private float sound_amplitude = 1.5f;

    private bool grounded;

    // Start is called before the first frame update
    void Start()
    {
        script = player.GetComponent<Movement_Paper>();
    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics.Raycast(script.flatPaper.transform.position, Vector3.down, 0.1f);

        if (script.longSide && !script.flat)
        {
            Rigidbody RB = script.rolledUp_Long.GetComponent<Rigidbody>();

            float spd = RB.velocity.magnitude * sound_amplitude;

            spd = Mathf.Clamp(spd, 0, 1);

            if (grounded)
            {

                rollSource.pitch = 0.8f + spd;
                rollSource.volume = spd;

                flySource.volume = 0;
            } else
            {
                if (rollSource.volume > 0) rollSource.volume -= 1.8f * Time.deltaTime;
            }
        }
        else if (!script.longSide && !script.flat)
        {

            Rigidbody RB = script.rolledUp_Short.GetComponent<Rigidbody>();

            float spd = RB.velocity.magnitude * sound_amplitude;

            spd = Mathf.Clamp(spd, 0, 1);

            if (grounded)
            {
                rollSource.pitch = 0.8f + spd;
                rollSource.volume = spd;

                flySource.volume = 0;
            } else
            {
                if (rollSource.volume > 0) rollSource.volume -= 1.8f * Time.deltaTime;
            }
        }
        else if (script.flat || !grounded)
        {
            if (rollSource.volume > 0) rollSource.volume -= 1.5f * Time.deltaTime;

            Rigidbody RB = script.flatPaper.GetComponent<Rigidbody>();

            float spd = RB.velocity.magnitude * 0.1f;

            spd = Mathf.Clamp(spd, 0, 0.1f);
            flySource.pitch = 1f + spd * 0.2f;
            flySource.volume = spd;

            // rollSource.volume = 0f;
        }

    }

}
