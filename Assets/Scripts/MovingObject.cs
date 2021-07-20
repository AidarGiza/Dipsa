using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{

    public GameObject core;
    public float speed;
    public float speedFactor;
    private Vector3 currentPoint;
    private Vector3 offset;
    private Vector3 direction;
    private float y;
    private float x;
    public float timeout;

    public bool IsDying
    {
        get => isDying;
        set
        {
            if (value) GetComponent<Rigidbody>().velocity = new Vector3();
            isDying = value;
        }
    } 
    private bool isDying;

    // Start is called before the first frame update
    void Start()
    {
        core = GameObject.Find("Core");
        direction = (core.transform.position - transform.position).normalized;

        transform.forward = direction;
        if (gameObject.tag == "GoodBonus")
        {
            float angle = Random.Range(7f, 35f);
            float sign = Mathf.Sign(Random.Range(-1f, 1f));
            transform.localRotation = Quaternion.Euler(new Vector3(0f, transform.localRotation.eulerAngles.y + (angle * sign), 0f));
            //offset = new Vector3(Random.Range(-0.9f, 0.9f), 0, Random.Range(-0.9f, 0.9f));
            //direction += offset;
        }
        Destroy(gameObject, timeout);

        //GetComponent<Rigidbody>().velocity = dir * speed;

        // Движение по параболе потом

        /*y = (spawnPoint - core.transform.position).magnitude;
        x = Mathf.Sqrt(y);
        
        currentPoint = new Vector3(spawnPoint.x+ (spawnPoint.x-x), 0, spawnPoint.z+(spawnPoint.z-y));
        transform.position = currentPoint;*/
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (core != null && !isDying)
        {
            GetComponent<Rigidbody>().velocity = transform.forward * speed * speedFactor;
        }

        /*y -= speed;
        x = Mathf.Sqrt(y);
        currentPoint.x += currentPoint.x - x;
        currentPoint.z += currentPoint.z - y;
        transform.right = new Vector3(currentPoint.x, 0, currentPoint.z);
        gameObject.transform.position = currentPoint;*/
    }
}
