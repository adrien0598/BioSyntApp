using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mono.Data.Sqlite;
using System.Data;

public class NextLevel : MonoBehaviour
{
    // Start is called before the first frame update
    public void NextLvl()
    {
        int lvl = CreateListLevels.lvl + 1;
        bool nex = false;

        ////SQL/////
        string p = "basedd.sqlite";
        string filepath = Application.persistentDataPath + "/" + p;

        string connection = "URI=file:" + filepath;

        SqliteConnection dbconn = new SqliteConnection(connection);

        dbconn.Open(); //Open connection to the database.
        IDbCommand dbcmd = dbconn.CreateCommand();

        string sqlQuery = "SELECT id " + "FROM Level";
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {
            int value = reader.GetInt32(0);
            //string name = reader.GetString(1);
            //int rand = reader.GetInt32(2);
            if(value == lvl)
            {
                nex = true;
            }
        }
        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
        ////SQL//////

        if (nex)
        {
            CreateListLevels.lvl++;
            Debug.Log(CreateListLevels.lvl);
            SceneManager.LoadScene("Game");
        }

        else
        {
            Debug.Log("pas de niveau supérieur");
            //faire mieu
        }
    }
}
