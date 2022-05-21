using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clicOnScroll : MonoBehaviour
{
    public GameObject fragment;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        fragment.SetActive(true);
    }
}
