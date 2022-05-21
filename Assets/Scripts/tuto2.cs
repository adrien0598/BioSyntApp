using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Mono.Data.Sqlite;
using System.Data;


public class tuto2 : MonoBehaviour
{
    public GameObject gene;
    public GameObject prom;
    public GameObject acti;
    public GameObject repre;
    public TextMeshProUGUI explication;
    private int conteur;

    public string lg;

    public string te_gene;
    public string te_pro;
    public string te_acti;
    public string te_repre;
    // Start is called before the first frame update
    void Start()
    {
        conteur = 0;
        gene.SetActive(true);


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
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
        ////SQL//////
        ///

        switch (lg)
        {
            case "FR":
                te_gene = "Les gènes peuvent produire des protéines " + "correspondant à leur couleur lorsqu'ils sont" + " precédés d'un promoteur actif.";
                te_pro = "Les promoteurs permettent l'expression des gènes. " + "Pour cela le promoteur doit être actif" + " et être placé avant le gène";
                te_acti = "Les activateurs activent l'element qu'ils ciblent." + " Il n'ont pas besoin de promoteur pour fonctionner";
                te_repre = "Les represseurs inactivent l'element qu'ils ciblent." + " Il n'ont pas besoin de promoteur pour fonctionner";
                break;

            case "EN":
                te_gene = "Genes can produce protein according to their color when preceded by an active promoter";
                te_pro = "Promoters enable the expression of genes. For this to happen, the promoter must be active and placed before the gene";
                te_acti = "Activators activate the element they target. They do not need a promoter to function";
                te_repre = "Represors inactivate the element they target. They do not need a promoter to function";
                break;

        }

        explication.text = te_gene;

    }

    public void suivant()
    {
        conteur += 1;
        switch (conteur)
        {
            case -1://n'arrive jamais en vrai
                SceneManager.LoadScene("Menu");
                break;
            case 0: //n'arrive jamais en vrai
                gene.SetActive(true);
                explication.text = te_gene;
                break;
            case 1:
                gene.SetActive(false);
                prom.SetActive(true);
                explication.text = te_pro;
                break;
            case 2:
                prom.SetActive(false);
                acti.SetActive(true);
                explication.text = te_acti;
                break;
            case 3:
                acti.SetActive(false);
                repre.SetActive(true);
                explication.text = te_repre;
                break;
            case 4:
                SceneManager.LoadScene("Menu");
                break;
        }
    }

    public void precedent()
    {
        conteur -= 1;
        switch (conteur)
        {
            case -1:
                SceneManager.LoadScene("Menu");
                break;
            case 0:
                gene.SetActive(true);
                prom.SetActive(false);
                explication.text = te_gene;
                break;
            case 1:
                prom.SetActive(true);
                acti.SetActive(false);
                explication.text = te_pro;
                break;
            case 2:
                acti.SetActive(true);
                repre.SetActive(false);
                explication.text = te_acti;
                break;
            case 3: //n'arrive jamais en vrai
                explication.text = te_repre;
                break;
            case 4: //n'arrive jamais en vrai
                SceneManager.LoadScene("Menu");
                break;
        }
    }
}
