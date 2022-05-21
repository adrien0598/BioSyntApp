using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mono.Data.Sqlite;
using System.Data;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class choix_langue : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void HandleInputData(int val)
    {
        string lg = "";
        switch (val)
        {
            case 0 :
                lg = "FR";
                break;

            case 1 :
                lg = "EN";
                break;

        }

        ///SQL/////
        ///
        string p = "basedd.sqlite";
        string filepath = Path.Combine(Application.persistentDataPath, p);

        string connection = "URI=file:" + filepath;
        //string connection = filepath;

        IDbConnection dbconn = new SqliteConnection(connection);

        dbconn.Open(); //Open connection to the database.

        IDbCommand dbcmd = dbconn.CreateCommand();

        string sqlQuery = "UPDATE Preferences " + "SET langue = '" + lg + "' WHERE id == 'user1'";
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();

        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
        ////SQL//////
    }

}
