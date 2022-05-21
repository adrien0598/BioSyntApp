using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Mono.Data.Sqlite;
using System.Data;

public class ButtonInList : MonoBehaviour
{
    public Text mytext;
    public Text mytext2;
    public GameObject geneTemplate;
    public GameObject message;
    public Canvas canv;
    public string lg;

    public void Start()
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
        string sqlQuery = "SELECT langue " + "FROM Preferences WHERE id == 'user1'";
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

    }

    public void SetText(string textString)
    {
        mytext.text = textString;
    }

    public void OnClick1()
    {
        message.SetActive(true);
        mytext2.text = mytext.text;
        string mes = mytext2.text + "\n" + "\n" + mytext2.text;

        ////SQL/////
        string p = "basedd.sqlite";
        string filepath = Application.persistentDataPath + "/" + p;

        string connection = "URI=file:" + filepath;

        SqliteConnection dbconn = new SqliteConnection(connection);

        dbconn.Open(); //Open connection to the database.
        IDbCommand dbcmd = dbconn.CreateCommand();

        Debug.Log(mytext2.text);
        string sqlQuery = "SELECT type, need, target " + "FROM Brick" + " WHERE name == " + "'" + mytext2.text + "'";
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {
            string type = reader.GetString(0);
            string need = reader.GetString(1);
            string target = reader.GetString(2);


            switch (lg)
            {
                case "FR":
                    mes = mes + " est un " + type + "\n" + "\n" + "Doit être activé par : " +
                need + "\n" + "\n" + "Cible : " + target + "\n" + "\n" + "Est inactivé par : ";
                    break;

                case "EN":
                    mes = mes + " is a " + type + "\n" + "\n" + "Need to be activated : " +
                need + "\n" + "\n" + "Target : " + target + "\n" + "\n" + "Is inactivated by : ";
                    break;

            }
            
        }
        reader.Close();
        reader = null;

        sqlQuery = "SELECT name FROM Brick WHERE (type == 'Repressor' OR type == 'Environment-') AND target LIKE '%" + 
            mytext2.text + "%'";
        dbcmd.CommandText = sqlQuery;
        reader = dbcmd.ExecuteReader();
        int cont = 0;
        while (reader.Read())
        {
            cont++;
            string na = reader.GetString(0);
            if (cont > 1)
            {
                mes = mes + ", " + na;
            }
            else
            {
                mes = mes + na;
            }
        }
        if (cont == 0)
        {
            mes = mes + "no";
        }

            dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
        ////SQL//////
        ///

        message.GetComponent<MessageBrique>().setText(mes);
    }

    public void OnClick()
    {
        GameObject gene = Instantiate(geneTemplate) as GameObject;
        gene.SetActive(true);

        ////SQL/////
        string p = "basedd.sqlite";
        string filepath = Application.persistentDataPath + "/" + p;

        string connection = "URI=file:" + filepath;

        SqliteConnection dbconn = new SqliteConnection(connection);

        dbconn.Open(); //Open connection to the database.
        IDbCommand dbcmd = dbconn.CreateCommand();

        string sqlQuery = "SELECT type, sprite " + "FROM Brick" + " WHERE name == " + "'" + mytext2.text + "'";
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {
            //int value = reader.GetInt32(0);
            string type = reader.GetString(0);
            int numero = reader.GetInt32(1);
            //int rand = reader.GetInt32(2);

            gene.GetComponent<objet1>().SetSprite(type + numero);
            //mettre un random ou pas parmis la bibliothèque d'images
            //GRAVE A FAIRE !!!

            gene.tag = type;

            canv.sortingOrder = 31;
            
        }
        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
        ////SQL//////

        
        gene.GetComponent<objet1>().SetName(mytext2.text);
        gene.transform.position = new Vector2(55, -150);
        message.SetActive(false);

    }
}
