using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class sql_creation : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
        string p = "basedd.sqlite";
        string filepath = Application.persistentDataPath + "/" + p;

        if (!File.Exists(filepath))

        {
            // si existe pas déjà ->
            // open StreamingAssets directory and load the db ->

            WWW loadDB = new WWW("jar:file://" + Application.dataPath + "!/assets/" + p);  // this is the path to the StreamingAssets in android

            while (!loadDB.isDone) { }  // CAREFUL here, for safety reasons shouldn't let this while loop unattended, maybe place a timer and error check

            // then save to Application.persistentDataPath

            File.WriteAllBytes(filepath, loadDB.bytes);

        }
    }

    void Update()
    {
        
    }
}
