using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class setting : MonoBehaviour
{
    public void set()
    {
        SceneManager.LoadScene("set");
    }

    public void quitter()
    {
        Application.Quit();
    }

}
