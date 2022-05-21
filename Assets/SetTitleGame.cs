using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using System;
using UnityEngine.UI;

public class SetTitleGame : MonoBehaviour
{

    public Text titre;
    public Text submit;
    public Text retour;
    public Text suite;
    public string lg;

    // Start is called before the first frame update
    void Start()
    {
        ////SQL/////
        ///
        string p = "basedd.sqlite";
        string filepath = Application.persistentDataPath + "/" + p;

        string connection = "URI=file:" + filepath;

        SqliteConnection dbconn = new SqliteConnection(connection);

        dbconn.Open(); //Open connection to the database.
        IDbCommand dbcmd = dbconn.CreateCommand();
        lg = "";
        string sqlQuery = "SELECT langue " + "FROM Preferences";
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {
            //int value = reader.GetInt32(0);
            lg = reader.GetString(0);
            //int rand = reader.GetInt32(2);
        }

        reader.Close();
        reader = null;

        ////SQL//////

        switch (lg)
        {
            case "FR":
                submit.text = "Valider";
                retour.text = "Retour";
                suite.text = "Suivant";
                break;

            case "EN":
                submit.text = "Submit";
                retour.text = "Back";
                suite.text = "Next";
                break;

        }

        ////SQL/////
        ///
        p = "basedd.sqlite";
        filepath = Application.persistentDataPath + "/" + p;

        connection = "URI=file:" + filepath;

        dbconn = new SqliteConnection(connection);

        dbconn.Open(); //Open connection to the database.
        dbcmd = dbconn.CreateCommand();

        sqlQuery = "SELECT title " + "FROM Level WHERE lvl == " + CreateListLevels.lvl + " AND langue == '" + lg + "'"; //and langue = FR
        dbcmd.CommandText = sqlQuery;
        reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {
            //int value = reader.GetInt32(0);
            string name = reader.GetString(0);
            //int rand = reader.GetInt32(2);
            titre.text = "Level " + CreateListLevels.lvl + " : " + name;
        }

        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
        ////SQL//////
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
