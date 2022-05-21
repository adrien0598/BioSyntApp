using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class CreateListLevels : MonoBehaviour
{
    public GameObject buttonTemplate;
    private Vector3 position;
    public static int lvl;
    public string lg;

    private void Start()
    {
        position = buttonTemplate.transform.position;

        var p1 = buttonTemplate.transform.TransformPoint(0, 0, 0);
        var p2 = buttonTemplate.transform.TransformPoint(0, 65, 0);
        var w = p2.y - p1.y;

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


        dbconn = new SqliteConnection(connection);

        dbconn.Open(); //Open connection to the database.

        dbcmd = dbconn.CreateCommand();

        sqlQuery = "SELECT DISTINCT lvl " + "FROM Level";
        dbcmd.CommandText = sqlQuery;
        reader = dbcmd.ExecuteReader();
        //triche
        int tmp = 1;
        while (reader.Read())
        {
            int value = reader.GetInt32(0);
            //string name = reader.GetString(1);
            //int rand = reader.GetInt32(2);
            GeneBut(tmp);
            tmp += 1;
        }
        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
        ////SQL//////


        void GeneBut(int valeur)
        {
            GameObject button = Instantiate(buttonTemplate) as GameObject;
            button.SetActive(true);

            switch (lg)
            {
                case "FR":
                    button.GetComponent<ButtonInList2>().SetText("Niveau " + valeur);
                    break;

                case "EN":
                    button.GetComponent<ButtonInList2>().SetText("Level " + valeur);
                    break;
            }

            button.transform.SetParent(buttonTemplate.transform.parent, false); //false pour le worlposition
            button.transform.position = position;

            position.y = position.y - (w + 5);
        }
        
    }

    public void clicked(String name)
    {
        string sub;
        sub = name.Substring(name.IndexOf(" ") + 1, name.Length - name.IndexOf(" ")-1);
        lvl = int.Parse(sub);
        SceneManager.LoadScene("Game");
    }

    private int FindEspace(String chaine) //équivaut à faire un chaine.IndexOf(" ")
    {
        int place = 0;

        for(int i = 0; i < chaine.Length; i++)
        {
            if (chaine[i].Equals(" "))
            {
                place = i;
            }
        }

        return place;
    }
}