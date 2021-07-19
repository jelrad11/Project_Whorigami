using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soft_Impact_Sound : MonoBehaviour
{
    public AudioSource source;

    public GameObject player;
    private Movement_Paper script;
    private float sound_amplitude = 1.2f;

    private float cooldown = 0.4f;

    // Start is called before the first frame update
    void Start()
    {
        script = player.GetComponent<Movement_Paper>();
    }

    // Update is called once per frame
    void Update()
    {
        if (cooldown > 0f) cooldown -= Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (cooldown <= 0)
        {
            cooldown = 0.4f;

            if (script.longSide && !script.flat)
            {
                Rigidbody RB = script.rolledUp_Long.GetComponent<Rigidbody>();

                float spd = RB.velocity.magnitude * sound_amplitude;

                spd = Mathf.Clamp(spd, 0, 0.2f);
                source.volume = spd;
                source.pitch = Random.Range(1f, 1.2f);
                source.Play();

            }
            else if (!script.longSide && !script.flat)
            {

                Rigidbody RB = script.rolledUp_Short.GetComponent<Rigidbody>();

                float spd = RB.velocity.magnitude * sound_amplitude;

                spd = Mathf.Clamp(spd, 0, 0.2f);
                source.volume = spd;
                source.pitch = Random.Range(1f, 1.2f);
                source.Play();

            }
            else if (script.flat)
            {
                Rigidbody RB = script.flatPaper.GetComponent<Rigidbody>();

                float spd = RB.velocity.magnitude * sound_amplitude;

                spd = Mathf.Clamp(spd, 0, 0.4f);
                source.volume = spd;
                source.pitch = Random.Range(0.9f, 1.1f);
                source.Play();
            }
        }
    }

}
