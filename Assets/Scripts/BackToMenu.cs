using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMenu : MonoBehaviour
{
    public void Back()
    {
        SceneManager.LoadScene("Menu");
    }
    public void Back2()
    {
        SceneManager.LoadScene("ChoixLevel");
    }
}
