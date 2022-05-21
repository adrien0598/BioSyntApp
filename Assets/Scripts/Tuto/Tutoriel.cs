using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Tutoriel : MonoBehaviour
{
    public GameObject zoneH;
    public GameObject zoneB;
    public GameObject ecran;
    private string[] contenu;
    private static int haut;
    private static int bas;
    public UnityEngine.Video.VideoPlayer video;

    // Start is called before the first frame update
    void Start()
    {
        string p = "tuto.txt";
        string filepath = Application.persistentDataPath + "/" + p;

        if (!File.Exists(filepath))

        {
            WWW loadDB = new WWW("jar:file://" + Application.dataPath + "!/assets/" + p);  // this is the path to your StreamingAssets in android

            while (!loadDB.isDone) { }  // CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check

            File.WriteAllBytes(filepath, loadDB.bytes);

        }
        haut = 2;
        bas = 3;
        //string fileName = Application.streamingAssetsPath + "/tutoriel/tuto.txt";
        string fileName = Application.persistentDataPath + "/tutoriel/tuto.txt";
        contenu = File.ReadAllLines(fileName);
        zoneH.GetComponent<TMPro.TextMeshProUGUI>().text = contenu[haut];
        zoneH.GetComponent<TMPro.TextMeshProUGUI>().fontSize = 55;
        zoneB.GetComponent<TMPro.TextMeshProUGUI>().text = contenu[bas];

        GameObject camera = GameObject.Find("Main Camera");

        // VideoPlayer automatically targets the camera backplane when it is added
        // to a camera object, no need to change videoPlayer.targetCamera.
        //video = camera.AddComponent<UnityEngine.Video.VideoPlayer>();
        video.isLooping = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Next()
    {
        if(contenu[haut+2] == "/end" & contenu[bas + 2] == "/end")
        {
            SceneManager.LoadScene("Tutoriel2");
        }
        else
        {
            haut += 2;
            bas += 2;

            if(haut != 2)
            {
                zoneH.GetComponent<TMPro.TextMeshProUGUI>().fontSize = 30;
            }
            else
            {
                zoneH.GetComponent<TMPro.TextMeshProUGUI>().fontSize = 55;
            }

            if (contenu[haut].Substring(0, 6) == "$video")
            {
                
                zoneH.SetActive(false);
                ecran.SetActive(true);
                video.url = Application.streamingAssetsPath + "/tutoriel/" + nVid(contenu[haut]) + ".avi";
            }
            else
            {
                ecran.SetActive(false);
                zoneH.SetActive(true);
                zoneH.GetComponent<TMPro.TextMeshProUGUI>().text = contenu[haut];
            }
            zoneB.GetComponent<TMPro.TextMeshProUGUI>().text = contenu[bas];
        }
    }

    public void Back()
    {
        if (contenu[haut-2] == "/begin" & contenu[bas-2] == "/begin")
        {
            SceneManager.LoadScene("Menu");
        }
        else
        {
            haut -= 2;
            bas -= 2;

            if (haut != 2)
            {
                zoneH.GetComponent<TMPro.TextMeshProUGUI>().fontSize = 30;
            }
            else
            {
                zoneH.GetComponent<TMPro.TextMeshProUGUI>().fontSize = 55;
            }
            if (contenu[haut].Substring(0,6) == "$video")
            {
                ecran.SetActive(true);
                zoneH.SetActive(false);
                video.url = "Assets/tutoriel/" + nVid(contenu[haut]) + ".avi";
            }
            else
            {
                ecran.SetActive(false);
                zoneH.SetActive(true);
                zoneH.GetComponent<TMPro.TextMeshProUGUI>().text = contenu[haut];
            }
            zoneB.GetComponent<TMPro.TextMeshProUGUI>().text = contenu[bas];
        }
    }

    public string nVid(string texte)
    {
        return texte.Substring(1, 6);
    }
}
