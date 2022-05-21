using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageBrique : MonoBehaviour
{

    public Text zoneDeText;
    public GameObject messag;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setText(string contenu) //définis le texte du message
    {
        zoneDeText.text = contenu;
    }

    public void Annuler()
    {
        messag.SetActive(false);
    }
}
