using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_Snake : MonoBehaviour
{

    public List<Transform> bodyParts = new List<Transform>();

    public float segmentDistance = 0.25f;

    public int snakeLength;
    
    public float speed = 1;
    public float rotationSpeed = 50;

    public GameObject bodyPrefabs;

    private float dis;
    private Transform curBodyPart;
    private Transform prevBodyPart;


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < snakeLength - 1; i++) AddBodyPart();
    }

    void Update()
    {
        Move();
    }

    public void Move()
    {
        float curspeed = speed;


        if (Input.GetKey(KeyCode.W)) bodyParts[0].Translate(bodyParts[0].forward * curspeed * Time.deltaTime, Space.World);

        if (Input.GetKey(KeyCode.D)) bodyParts[0].Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        else if (Input.GetKey(KeyCode.A)) bodyParts[0].Rotate(Vector3.up * -1f * rotationSpeed * Time.deltaTime);

        if(Input.GetKey(KeyCode.Space) && (bodyParts[0].rotation.x > -90f && bodyParts[0].rotation.x < 90f)) bodyParts[0].Rotate(Vector3.right * -1f * (rotationSpeed/2) * Time.deltaTime);
        else if(Input.GetKey(KeyCode.E) && (bodyParts[0].rotation.x > -90f && bodyParts[0].rotation.x < 90f)) bodyParts[0].Rotate(Vector3.right * (rotationSpeed/2) * Time.deltaTime);
        if(Input.GetKey(KeyCode.W))
            for (int i = 1; i < bodyParts.Count; i++)
            {
                curBodyPart = bodyParts[i];
                prevBodyPart = bodyParts[i - 1];


                Vector3 tmp = prevBodyPart.forward * segmentDistance;
                dis = Vector3.Distance(prevBodyPart.position, curBodyPart.position);

                Vector3 newPos = prevBodyPart.position;

                //newPos.y = bodyParts[0].position.y;

                float T = Time.deltaTime * dis / segmentDistance * curspeed;

                if (T > 0.5f) T = 0.5f;
                curBodyPart.position = Vector3.Lerp(curBodyPart.position, newPos, T);
                curBodyPart.rotation = Quaternion.Lerp(curBodyPart.rotation, prevBodyPart.rotation, T);
            }
    }

    public void AddBodyPart()
    {
        Transform newpart = (Instantiate (bodyPrefabs, bodyParts[bodyParts.Count - 1].position, bodyParts[bodyParts.Count - 1].rotation) as GameObject).transform;
        newpart.SetParent(transform);
        bodyParts.Add(newpart);
    }
}