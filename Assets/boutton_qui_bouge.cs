using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boutton_qui_bouge : MonoBehaviour
{

    public GameObject boutton1;
    private int a;

    // Start is called before the first frame update
    void Start()
    {
        a = 1;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 scal = boutton1.transform.localScale;

        if (scal.x > 1.1)
        {
            a = -1;
        }
        else if (scal.x < 0.98)
        {
            a = 1;
        }

        scal.x += a*0.0005f;
        scal.y += a*0.0005f;
        boutton1.transform.localScale = scal;
    }
}
