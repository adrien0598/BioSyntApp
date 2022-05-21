using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Home_fond : MonoBehaviour
{

    public GameObject fond;
    private Vector3 vecAdd;
    public float speed;
    private int rot;
    private int a;

    // Start is called before the first frame update
    void Start()
    {
        vecAdd.x = 1;
        vecAdd.y = 1;
        vecAdd.z = 0;
        rot = 0;
        a = 1;
    }

    // Update is called once per frame
    void Update()
    {
        //position
        transform.position = transform.position + vecAdd*speed;
        if (transform.position.x > 400)
        {
            vecAdd.x = -1;
        }

        else if (transform.position.y > 270)
        {
            vecAdd.y = -1;
        }

        else if (transform.position.y < 150)
        {
            vecAdd.y = 1;
        }

        else if (transform.position.x < 200)
        {
            vecAdd.x = 1;
        }

        //rotation
        rot = rot + a;
        if (rot < 0)
        {
            transform.Rotate(0, 0, 0.005f);
        }
        else
        {
            transform.Rotate(0, 0, -0.005f);
        }

        if (rot <= -1000 | rot >= 1000)
        {
            a = a * (-1);
        }
    }
}
