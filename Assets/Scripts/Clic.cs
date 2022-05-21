using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clic : MonoBehaviour
{
    public Button boutton;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void clic()
    {
        boutton.GetComponent<ButtonInList>().OnClick();
    }
}
