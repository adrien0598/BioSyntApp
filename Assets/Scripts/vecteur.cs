using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;

public class vecteur : MonoBehaviour
{

    public GameObject geneA;
    public GameObject promoter;
    public Vector3 Centre;
    private float placeGene;
    private float placeProm;
    public Animator anim;
    public GameObject panneau;
    public Camera cam;
    private GameObject[] geneList;
    private GameObject[] proList;
    private GameObject[] envNegList;
    private GameObject[] envPosList;
    private GameObject[] repreList;
    private GameObject[] ehanList;
    public static bool ok = true;
    char car;
    private string goal;
    public GameObject next;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        next.SetActive(false);

        ////SQL/////
        string p = "basedd.sqlite";
        string filepath = Application.persistentDataPath + "/" + p;

        string connection = "URI=file:" + filepath;

        SqliteConnection dbconn = new SqliteConnection(connection);

        dbconn.Open(); //Open connection to the database.
        IDbCommand dbcmd = dbconn.CreateCommand();

        string sqlQuery = "SELECT goal " + "FROM Level WHERE id == " + CreateListLevels.lvl;
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {
            //int value = reader.GetInt32(0);
            goal = reader.GetString(0);
            //int rand = reader.GetInt32(2);
            
        }
        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
        ////SQL//////
        
    }

    private float d(Vector3 x1, Vector3 x2)
    {
        return (Mathf.Sqrt(Mathf.Pow((x1.x - x2.x), 2) + Mathf.Pow((x1.y - x2.y), 2) + Mathf.Pow((x1.z - x2.z), 2)));
    }

    IEnumerator attendre(int t)
    {
        yield return new WaitForSeconds(t);
    }


    // Update is called once per frame
    void Update()
    {
        geneList = GameObject.FindGameObjectsWithTag("gene"); //regulariser la nomenclature!!!
        proList = GameObject.FindGameObjectsWithTag("promotor");
        envNegList = GameObject.FindGameObjectsWithTag("Environment-");
        envPosList = GameObject.FindGameObjectsWithTag("Environment+");
        repreList = GameObject.FindGameObjectsWithTag("Represor");
        ehanList = GameObject.FindGameObjectsWithTag("Activator");

        List<GameObject> activPro;
        List<GameObject> activGen;
        activPro = selectActivPro(proList, repreList, ehanList, envNegList, envPosList);
        //Debug.Log(activPro.Count);
        activGen = selectActivGene(activPro, geneList);
        //Debug.Log(activGen.Count);

        //posGeneA = geneA.transform.position;
        //posPromABC = promoter.transform.position;
        //if (!Input.GetMouseButton(0))
        //{
        //    if ((int)d(posGeneA, Centre) > 148 && (int)d(posGeneA, Centre) < 152 && (int)d(posPromABC, Centre) > 148 && (int)d(posPromABC, Centre) < 152)
        //    {
        //        placeGene = Mathf.Atan2((posGeneA.x - Centre.x) / 150, (posGeneA.y - Centre.y) / 150);
        //        placeProm = Mathf.Atan2((posPromABC.x - Centre.x) / 150, (posPromABC.y - Centre.y) / 150);
        //        //Debug.Log(Mathf.Atan2((posGeneA.x - Centre.x) / 150, (posGeneA.y - Centre.y) / 150));
        //        Debug.Log(placeGene - placeProm);
        //        if (placeGene > placeProm && placeGene - placeProm < 1.1)
        //        {
        //            anim.SetBool("emission", true);
        //            transform.rotation = Quaternion.Euler(0, 0, -(placeGene / Mathf.PI) * 180);
        //        }
        //    }
        //}
        if (!Input.GetMouseButton(0))
        {
            //oui ba on fait ce qu'on peut
            //if (activGen.Count > 0)
            //{
                //Bool("emission", true);
                //next.SetActive(true);
            //}
        }

    }

    private bool onVector(GameObject objet)
    {
        if ((int)d(objet.transform.position, Centre) > 148 && (int)d(objet.transform.position, Centre) < 152)
        {
            return (true);
        }
        else
        {
            return (false);
        }
    }

    private List<GameObject> selectActivPro(GameObject[] Prom, GameObject[] Ehan, GameObject[] Repre, GameObject[] EnvNeg, GameObject[] EnvPos)
        //selectionne les promoteurs actifs
    {
        List<GameObject> activPro = new List<GameObject>();
        List<string> activator = new List<string>();
        activator = selectNameActiv(Ehan, Prom, EnvPos);
        List<string> represor = new List<string>();
        represor = selectName(Repre, EnvNeg);
        for (int i = 0; i < Prom.Length; i++)
        {
            string nom = Prom[i].GetComponent<objet1>().nom;

            ////SQL/////
            string p = "basedd.sqlite";
            string filepath = Application.persistentDataPath + "/" + p;

            string connection = "URI=file:" + filepath;

            SqliteConnection dbconn = new SqliteConnection(connection);

            dbconn.Open(); //Open connection to the database.
            IDbCommand dbcmd = dbconn.CreateCommand();

            string sqlQuery = "SELECT need " + "FROM Brick WHERE name == " + "'" + nom + "'";
            // FAUX, il faut gérer les virgules !!!!!!
            dbcmd.CommandText = sqlQuery;
            IDataReader reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                //int value = reader.GetInt32(0);
                string need = reader.GetString(0); 
                //int rand = reader.GetInt32(2);
                if (need == "no")
                {
                    ok = true;
                }
                else if(!activator.Contains(need))
                {
                    ok = false;
                }
            }
            reader.Close();
            reader = null;
            

            sqlQuery = "SELECT name " + "FROM Brick WHERE target == " + "'" + nom + "' " + "AND type == 'represor'";
            // FAUX, il faut gérer les virgules !!!!!!

            dbcmd.CommandText = sqlQuery;
            reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                //int value = reader.GetInt32(0);
                string repre = reader.GetString(0);
                //int rand = reader.GetInt32(2);
                if (represor.Contains(repre))
                {
                    ok = false;
                }
            }

            if (ok)
            {
                activPro.Add(Prom[i]);
            }

            reader.Close();
            reader = null;
            dbcmd.Dispose();
            dbcmd = null;
            dbconn.Close();
            dbconn = null;
            ////SQL//////

        }
        return (activPro);
    }

    private List<string> selectNameActiv(GameObject[] Ehan, GameObject[] Prom, GameObject[] Env)
        // renvoie la lste des nom de tout les objets en présence succeptibles d'activer un truc
    {
        List<string> noms = new List<string>();
        for (int i = 0; i < Ehan.Length; i++)
        {
            noms.Add(Ehan[i].GetComponent<objet1>().nom);
        }
        for (int i = 0; i < Prom.Length; i++)
        {
            noms.Add(Prom[i].GetComponent<objet1>().nom);
        }
        for (int i = 0; i < Env.Length; i++)
        {
            noms.Add(Env[i].GetComponent<objet1>().nom);
        }

        return (noms);
    }

    private List<string> selectName(GameObject[] Repre, GameObject[] Repre2)
    // renvoie la lste des nom de tout les objets en présence succeptibles d'inactiver un truc
    {
        List<string> noms = new List<string>();
        for (int i = 0; i < Repre.Length; i++)
        {
            noms.Add(Repre[i].GetComponent<objet1>().nom);
        }
        for (int i = 0; i < Repre2.Length; i++)
        {
            noms.Add(Repre2[i].GetComponent<objet1>().nom);
        }
        return (noms);
    }

    private List<string> selectName2(List<GameObject> Repre)
    // renvoie la lste des nom de tout les objets en présence succeptibles d'inactiver un truc
    {
        List<string> noms = new List<string>();
        for (int i = 0; i < Repre.Count; i++)
        {
            noms.Add(Repre[i].GetComponent<objet1>().nom);
        }
        return (noms);
    }

    private GameObject findP (List<GameObject> liste, string nom)
    {
        for (int i = 0; i<liste.Count; i++)
        {
            if(liste[i].GetComponent<objet1>().nom == nom)
            {
                return (liste[i]);
            }
        }
        return (liste[0]); // par default
    }



    private List<GameObject> selectActivGene(List<GameObject> activPro, GameObject[] geneList)
    {
        List<GameObject> genesok = new List<GameObject>();

        for (int i = 0; i < geneList.Length; i++)
        {
            string nom = geneList[i].GetComponent<objet1>().nom;
            ////SQL/////
            string p = "basedd.sqlite";
            string filepath = Application.persistentDataPath + "/" + p;

            string connection = "URI=file:" + filepath;

            SqliteConnection dbconn = new SqliteConnection(connection);

            dbconn.Open(); //Open connection to the database.
            dbconn.Open(); //Open connection to the database.
            IDbCommand dbcmd = dbconn.CreateCommand();

            string sqlQuery = "SELECT need " + "FROM Brick WHERE name == " + "'" + nom + "'";
            // FAUX, il faut gérer les virgules !!!!!!
            dbcmd.CommandText = sqlQuery;
            IDataReader reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                //int value = reader.GetInt32(0);
                string need = reader.GetString(0);
                //int rand = reader.GetInt32(2);

                int j = 0;
                for (int k = 0; k < need.Length; k++)
                {
                    car = ',';
                    if (need[k].Equals(car))
                    {
                        if (selectName2(activPro).Contains(need.Substring(j, k - j)))
                        {
                            GameObject Promoteur = findP(activPro, need.Substring(j, k - j));
                            placeGene = Mathf.Atan2((geneList[i].transform.position.x - Centre.x) / 150, (geneList[i].transform.position.y - Centre.y) / 150);
                            placeProm = Mathf.Atan2((Promoteur.transform.position.x - Centre.x) / 150, (Promoteur.transform.position.y - Centre.y) / 150);
                            if (onVector(Promoteur) & onVector(geneList[i]) & placeGene > placeProm & placeGene - placeProm < 1.1)
                            {
                                genesok.Add(geneList[i]);
                            }
                        }
                        // else ?
                        
                        j = k + 2;
                    }
                }
                if (selectName2(activPro).Contains(need.Substring(j, need.Length - j)))
                {
                    GameObject Promoteur = findP(activPro, need.Substring(j, need.Length - j));
                    placeGene = Mathf.Atan2((geneList[i].transform.position.x - Centre.x) / 150, (geneList[i].transform.position.y - Centre.y) / 150);
                    placeProm = Mathf.Atan2((Promoteur.transform.position.x - Centre.x) / 150, (Promoteur.transform.position.y - Centre.y) / 150);
                    if (onVector(Promoteur) & onVector(geneList[i]) & placeGene > placeProm & placeGene - placeProm < 1.1)
                    {
                        genesok.Add(geneList[i]);
                    }
                }
            }

            reader.Close();
            reader = null;
            dbcmd.Dispose();
            dbcmd = null;
            dbconn.Close();
            dbconn = null;
            ////SQL//////
        }

        
        return (genesok);
    }
}
