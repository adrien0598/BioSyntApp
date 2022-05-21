using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menu : MonoBehaviour
{

    public void Start()
    {
        Debug.Log("Début");
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("ChoixLevel");
    }

    public void PlaySet()
    {
        SceneManager.LoadScene("Set");
    }

    public void PlayQuit()
    {
        Application.Quit();
    }

    public void PlayTuto()
    {
        SceneManager.LoadScene("Tutoriel2");
    }
}
