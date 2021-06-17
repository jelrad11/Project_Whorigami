using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Bird_G : MonoBehaviour
{

    public Transform target;

    public float smoothSpeed = 10f;
    public Vector3 baseOffset = new Vector3 (0, 0.5f, 0);
    public float offset0;
    public float offset1;
    public Vector3 offset2;
    public GameObject bird;
    private Movement_Bird_G birdscript;

    private void Start()
    {
        birdscript = bird.GetComponent<Movement_Bird_G>();
    }

    void LateUpdate()
    {
        offset0 = 0.05f;
        offset1 = birdscript.horiCurrentRotationSpeed/140f;
        offset2.y = birdscript.angle/60f;

        Vector3 desiredPosition = target.position + target.forward * offset0 + target.right * offset1 + offset2 + baseOffset; // + cause bird model has backside on front

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, birdscript.movementSpeed * smoothSpeed * Time.deltaTime);

        transform.position = smoothedPosition;

        transform.LookAt(target);


    }

}
