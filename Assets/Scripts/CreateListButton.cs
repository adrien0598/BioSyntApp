using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using System;
using UnityEngine.SceneManagement;

public class CreateListButton : MonoBehaviour
{
    public GameObject buttonTemplate;
    private Vector3 position;
    private int taille;
    char car;

    private void Start()
    {
        position = buttonTemplate.transform.position;

        var p1 = buttonTemplate.transform.TransformPoint(0, 0, 0);
        var p2 = buttonTemplate.transform.TransformPoint(0, 70, 0);
        //var s1 = Camera.main.WorldToScreenPoint(p1);
        //var s2 = Camera.main.WorldToScreenPoint(p2);
        var w = p2.y - p1.y;
        //var sw = s2.y - s1.y;

        //Debug.Log(w);
        //Debug.Log(sw);

        ////SQL/////
        string p = "basedd.sqlite";
        string filepath = Application.persistentDataPath + "/" + p;

        string connection = "URI=file:" + filepath;

        SqliteConnection dbconn = new SqliteConnection(connection);

        dbconn.Open(); //Open connection to the database.
        IDbCommand dbcmd = dbconn.CreateCommand();

        string sqlQuery = "SELECT need " + "FROM Level WHERE lvl == " + CreateListLevels.lvl + " AND langue == 'FR'";
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {
            //int value = reader.GetInt32(0);
            string name = reader.GetString(0);
            //int rand = reader.GetInt32(2);
            int j = 0;
            for (int i = 0; i < name.Length; i++)
            {
                car = ',';
                if (name[i].Equals(car))
                {
                    GenButton(name.Substring(j, i - j));
                    j = i + 2;
                }
            }
            GenButton(name.Substring(j, name.Length - j));
        }
        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
        ////SQL//////

        void GenButton(string name)
        {
            

            GameObject button = Instantiate(buttonTemplate) as GameObject;
            button.SetActive(true);

            button.GetComponent<ButtonInList>().SetText(name);

            button.transform.SetParent(buttonTemplate.transform.parent, false); //false pour le worlposition
            button.transform.position = position;

            position.y = position.y - (w + 5);
        }

    }

    
}
