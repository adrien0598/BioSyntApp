using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonInList2 : MonoBehaviour
{
    [SerializeField]
    public Text mytext;
    [SerializeField]
    private CreateListLevels buttonControl;

    private string thetext;

    public void SetText(string textString)
    {
        thetext = textString;
        mytext.text = thetext;
    }

    public void OnClick()
    {
        buttonControl.clicked(thetext);
    }
}
