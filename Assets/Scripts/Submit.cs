using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using UnityEngine.UI;
using System.IO;

public class Submit : MonoBehaviour
{
    private GameObject[] geneList;
    private GameObject[] proList;
    public List<string> envList;
    private GameObject[] repreList;
    private GameObject[] ehanList;

    public List<GameObject> genes;
    public List<GameObject> repres;
    public List<GameObject> ehans;
    public List<GameObject> pros;

    public GameObject next;
    public string goal;
    public List<string> input_th;
    public List<string> output_th;
    private char car;

    public Vector3 Centre;

    // Start is called before the first frame update
    void Start()
    {
        
        ////SQL/////
        string p = "basedd.sqlite";
        string filepath = Path.Combine(Application.persistentDataPath, p);

        string connection = "URI=file:" + filepath;
        
        IDbConnection dbconn = new SqliteConnection(connection);

        dbconn.Open(); //Open connection to the database.

        IDbCommand dbcmd = dbconn.CreateCommand();

        string sqlQuery = "SELECT goal " + "FROM Level WHERE lvl == " + CreateListLevels.lvl + " AND langue == 'FR'";

        dbcmd.CommandText = sqlQuery;

        IDataReader reader = dbcmd.ExecuteReader();

        while (reader.Read())
        {
            
            //int value = reader.GetInt32(0);
            goal = reader.GetString(0);
            //Debug.Log("but : " + goal);
            //int rand = reader.GetInt32(2);

        }

        reader.Close();

        //reader = null;

        dbcmd.Dispose();
        //dbcmd = null;

        dbconn.Close();
        //dbconn = null;
        ////SQL//////


        if (!goal.Contains(";"))
        {
            input_th.Add("no");
            output_th.Add(goal);
        }
        else
        {
            int i = 0;
            int j = -1;
            string sub;
            car = ';';
            while (j < goal.Length-1)
            {
                j++;
                if (goal[j].Equals(car))
                {
                    sub = goal.Substring(i, (j - i));
                    output_th.Add(sub.Substring(0, sub.IndexOf(":")));
                    input_th.Add(sub.Substring(sub.IndexOf(":")+1, sub.Length - sub.IndexOf(":")-1));
                    i = j+1;
                }

                if (j+1 == goal.Length)
                {
                    sub = goal.Substring(i, (j+1 - i));
                    output_th.Add(sub.Substring(0, sub.IndexOf(":")));
                    input_th.Add(sub.Substring(sub.IndexOf(":")+1, sub.Length - sub.IndexOf(":") - 1));
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Verif()
    {
        geneList = GameObject.FindGameObjectsWithTag("gene"); //regulariser la nomenclature!!!
        proList = GameObject.FindGameObjectsWithTag("promotor");
        repreList = GameObject.FindGameObjectsWithTag("Represor");
        ehanList = GameObject.FindGameObjectsWithTag("Activator");

        //fonction verification
        if (verification(input_th, output_th))
        {
            next.SetActive(true);
        }
        
    }

    public bool verification(List<string> input, List<string> output)
    {
        bool ok = true;
        int taille = input.Count;
        List<string> ou = new List<string>();

        for (int i = 0; i < taille; i++)
            //pour chaque couple input output, on test la validité du schéma.
        {
            //On remplis les équivalents list des array automatiques
            genes.Clear();
            pros.Clear();
            ehans.Clear();
            repres.Clear();
            for (int j = 0; j < geneList.Length; j++)
            {
                genes.Add(geneList[j]);
            }
            for (int j = 0; j < proList.Length; j++)
            {
                pros.Add(proList[j]);
            }
            for (int j = 0; j < ehanList.Length; j++)
            {
                ehans.Add(ehanList[j]);
            }
            for (int j = 0; j < repreList.Length; j++)
            {
                repres.Add(repreList[j]);
            } // à bouger dans la boucle de verif ?


            //nomenclature : input1,input2,input3:output1,output2; etc
            car = ',';
            int conteur = 0;
            envList.Clear();
            for (int k = 0; k < input[i].Length; k++)
            {
                if (input[i][k].Equals(car))
                {
                    envList.Add(input[i].Substring(conteur , k-conteur));
                    conteur = k;
                }
            }
            envList.Add(input[i].Substring(conteur, input[i].Length - conteur));
            Debug.Log("envlistcoutn " + envList.Count);
            Debug.Log("tour " + i + " : on vérifie que pour l'entrée " + input[i] + ", on obtient bien la sortie " + output[i]);
            //On clean les listes de briques présente en enlevant celles qui ne sont pas sur le vecteur
            clean();
            Debug.Log("étape 0");
            // On cascade les repressions
            repression();

            Debug.Log("étape 1");

            conteur = 0;
            ou.Clear();
            for (int k = 0; k < output[i].Length; k++)
            {
                if (output[i][k].Equals(car))
                {
                    ou.Add(output[i].Substring(conteur, k-conteur));
                    conteur = k+1;
                }
            }
            ou.Add(output[i].Substring(conteur, output[i].Length - conteur));
            Debug.Log("étape 2");
            //On fait une liste avec tout les gènes non présent dans output mais present dans genes pour vérif après qu'ils ne sont pas exprimées
            List<string> rebus = new List<string>();
            rebus.Clear();
            foreach (GameObject item in genes)
            {
                Debug.Log("gene present : " + item.GetComponent<objet1>().nom);
                if (!ou.Contains(item.GetComponent<objet1>().nom))
                {
                    Debug.Log("gene ajouté à rebus : " + item.GetComponent<objet1>().nom);
                    rebus.Add(item.GetComponent<objet1>().nom);
                }
            }
            if (rebus.Count > 0 & !ou[0].Equals("no"))
            {
                if (!notActivated(rebus))
                {//MAIS QUE SE PASSE  IL ?
                    Debug.Log("rentre 2");
                    return (false); //ok c'est un peu expeditif on fait pas les autres tests mais bon ça gagne masse temps
                }
            }
            Debug.Log("étape 3");

            for (int k = 0; k < ou.Count; k++)
            {
                // (On cascade les activations) correctif, on vérifie que le gène goal est bien activé (retrospectivement).
                if (!ou[k].Equals("no"))
                {
                    //attention ça part en récursif !
                    if (activation(ou[k]).Equals("yes") & isPresentgene(ou[k]))
                    {
                        //le test i a réussi
                        Debug.Log("Le test à réussi");
                        //ok = true;
                    }
                    else
                    {
                        //le test i a échoué
                        ok = false;
                        Debug.Log("Le test à échoué");
                    }
                }

                else
                {
                    //seul cas ou il faut vraiment tout vérif (pour chaque gène du problème on vérif qu'il ne s'exprime pas)
                    if (activation().Equals("no"))
                    {
                        //le test i est ok
                        Debug.Log("test ok");
                        //ok = true;
                    }
                    else
                    {
                        ok = false;
                        Debug.Log("test échoué");
                    }
                }
            }
        }
        return (ok);
    }

    private void clean()
    {
        for (int i = 0; i < geneList.Length; i++)
        {
            if (!onVector(geneList[i]))
            {
                genes.Remove(geneList[i]);
                Destroy(geneList[i]);
            }
        }
        for (int i = 0; i < proList.Length; i++)
        {
            if (!onVector(proList[i]))
            {
                pros.Remove(proList[i]);
                Destroy(proList[i]);
            }
        }
        for (int i = 0; i < repreList.Length; i++)
        {
            if (!onVector(repreList[i]))
            {
                repres.Remove(repreList[i]);
                Destroy(repreList[i]);
            }
        }
        for (int i = 0; i < ehanList.Length; i++)
        {
            if (!onVector(ehanList[i]))
            {
                ehans.Remove(ehanList[i]);
                Destroy(ehanList[i]);
            }
        }

        
    }

    private void repression()
    {
        string target = "";
        string type = "";


        //les env (input) inhibe si - dans toutes les autres listes (pro, acti, ehan, gene)

        ////SQL/////
        ///
        string p = "basedd.sqlite";
        string filepath = Application.persistentDataPath + "/" + p;

        string connection = "URI=file:" + filepath;

       
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(connection);
        dbconn.Open(); //Open connection to the database.

        for (int i = 0; i < envList.Count; i++)
        {
            if(envList[i] != "no")
            {

                IDbCommand dbcmd = dbconn.CreateCommand();
                string sqlQuery = "SELECT target, type " + "FROM Brick WHERE name == " + "'" + envList[i] + "'";
               // ENCORE UNE FOIS doit gèrer les virgules !!!! c'est bon c'est fait
                dbcmd.CommandText = sqlQuery;
                IDataReader reader = dbcmd.ExecuteReader();
                while (reader.Read())
                {
                    target = reader.GetString(0);
                    type = reader.GetString(1);

                }
                reader.Close();
                reader = null;
                dbcmd.Dispose();
                dbcmd = null;

                //prend target (tout les targets sep par virgule) et sort une liste
                // nomenclature : target1,target2,target3...
                car = ',';
                int conteur = 0;
                List<string> targList = new List<string>();

                for (int k = 0; k < target.Length; k++)
                {
                    if (target[k].Equals(car))
                    {
                        targList.Add(target.Substring(conteur, k-conteur));
                        conteur = k+1;
                    }
                }

                targList.Add(target.Substring(conteur, target.Length - conteur));

                foreach (string targ in targList)
                {
                    if (type[type.Length - 1] == '-')
                    {
                        List<GameObject> aEnlever = new List<GameObject>(); // je me demande si ça va planter quant il y aura plusieurs passages
                                                                            //on verra
                        //GENE
                        foreach (GameObject item in genes)
                        {
                            if (item.GetComponent<objet1>().nom.Equals(targ))
                            {
                                aEnlever.Add(item);
                            }
                        }
                        foreach (GameObject item in aEnlever)
                        {
                            genes.Remove(item);
                        }

                        //PRO
                        foreach (GameObject item in pros)
                        {
                            if (item.GetComponent<objet1>().nom.Equals(targ))
                            {
                                aEnlever.Add(item);
                            }
                        }
                        foreach (GameObject item in aEnlever)
                        {
                            pros.Remove(item);
                        }

                        //ACTI
                        foreach (GameObject item in ehans)
                        {
                            if (item.GetComponent<objet1>().nom.Equals(targ))
                            {
                                aEnlever.Add(item);
                            }
                        }
                        foreach (GameObject item in aEnlever)
                        {
                            ehans.Remove(item);
                        }

                        //REPRE
                        foreach (GameObject item in repres)
                        {
                            if (item.GetComponent<objet1>().nom.Equals(targ))
                            {
                                aEnlever.Add(item);
                            }
                        }
                        foreach (GameObject item in aEnlever)
                        {
                            repres.Remove(item);
                        }
                    }
                }
            }
        }


        dbconn.Close();
        dbconn = null;
        ////SQL//////
        ///

        //Les repres s'inhibent si il y a

        ////SQL/////

        p = "basedd.sqlite";
        filepath = Application.persistentDataPath + "/" + p;

        connection = "URI=file:" + filepath;

        dbconn = new SqliteConnection(connection);
        dbconn.Open(); //Open connection to the database.
        List<GameObject> enlever = new List<GameObject>();
        foreach (GameObject item in repres)
        {
            IDbCommand dbcmd = dbconn.CreateCommand();
            string sqlQuery = "SELECT target " + "FROM Brick WHERE name == " + "'" + item.GetComponent<objet1>().nom + "'";
            // ENCORE UNE FOIS doit gèrer les virgules !!!! c'est fait
            dbcmd.CommandText = sqlQuery;
            IDataReader reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                //int value = reader.GetInt32(0);
                target = reader.GetString(0);
                //int rand = reader.GetInt32(2);

            }
            reader.Close();
            reader = null;
            dbcmd.Dispose();
            dbcmd = null;

            car = ',';
            int conteur = 0;
            List<string> targList = new List<string>();

            for (int k = 0; k < target.Length; k++)
            {
                if (target[k].Equals(car))
                {
                    targList.Add(target.Substring(conteur, k-conteur));
                    conteur = k+1;
                }
            }
            targList.Add(target.Substring(conteur, target.Length-conteur));

            foreach (string targ in targList)
            {
                foreach (GameObject item2 in repres)
                {
                    if (item2.GetComponent<objet1>().nom.Equals(targ))
                    {
                        enlever.Add(item2);
                    }
                }
                foreach (GameObject item3 in enlever)
                {
                    repres.Remove(item3);
                }
            }
        }

        dbconn.Close();
        dbconn = null;
        ////SQL//////


        // Les repres inhibent dans pros, genes et ehans

        ////SQL/////
        p = "basedd.sqlite";
        filepath = Application.persistentDataPath + "/" + p;

        connection = "URI=file:" + filepath;

        dbconn = new SqliteConnection(connection);
        dbconn.Open(); //Open connection to the database.

        enlever.Clear();
        foreach (GameObject item in repres)
        {
            IDbCommand dbcmd = dbconn.CreateCommand();

            string sqlQuery = "SELECT target " + "FROM Brick WHERE name == " + "'" + item.GetComponent<objet1>().nom + "'";
            // ENCORE UNE FOIS doit gèrer les virgules !!!! c'est fait
            dbcmd.CommandText = sqlQuery;
            IDataReader reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                //int value = reader.GetInt32(0);
                target = reader.GetString(0);
                //int rand = reader.GetInt32(2);

            }
            reader.Close();
            reader = null;
            dbcmd.Dispose();
            dbcmd = null;

            car = ',';
            int conteur = 0;
            List<string> targList = new List<string>();

            for (int k = 0; k < target.Length; k++)
            {
                if (target[k].Equals(car))
                {
                    targList.Add(target.Substring(conteur, k - conteur));
                    conteur = k+1;
                }
            }
            targList.Add(target.Substring(conteur, target.Length - conteur));

            foreach (string targ in targList)
            {
                foreach (GameObject item2 in genes)
                {
                    if (item2.GetComponent<objet1>().nom.Equals(target))
                    {
                        enlever.Add(item2);
                    }
                }
                foreach (GameObject item3 in enlever)
                {
                    genes.Remove(item3);
                }

                foreach (GameObject item2 in pros)
                {
                    if (item2.GetComponent<objet1>().nom.Equals(target))
                    {
                        enlever.Add(item2);
                    }
                }
                foreach (GameObject item3 in enlever)
                {
                    pros.Remove(item3);
                }

                foreach (GameObject item2 in ehans)
                {
                    if (item2.GetComponent<objet1>().nom.Equals(target))
                    {
                        enlever.Add(item2);
                    }
                }
                foreach (GameObject item3 in enlever)
                {
                    ehans.Remove(item3);
                }
            }

                

        }

        dbconn.Close();
        dbconn = null;
        ////SQL//////

    }

    private string activation()
    {
        foreach(GameObject item in genes)
        {
            if (activation(item.GetComponent<objet1>().nom).Equals("yes"))
            {
                return ("yes");
            }
        }
        return ("no");
    }

    private bool notActivated(List<string> liste)
    {
        foreach (string item in liste)
        {
            Debug.Log(item);
            if (activation(item).Equals("yes"))
            {
                return (false);
            }
        }

        return (true);
    }

    private string activation(string brique)
    {
        string name = "";
        string type = "";
        ////SQL/////
        ///

        string p = "basedd.sqlite";
        string filepath = Application.persistentDataPath + "/" + p;

        string connection = "URI=file:" + filepath;

        SqliteConnection dbconn = new SqliteConnection(connection);

        dbconn.Open(); //Open connection to the database.
        IDbCommand dbcmd = dbconn.CreateCommand();

        string sqlQuery = "SELECT need " + "FROM Brick WHERE name == " + "'" + brique + "'";
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {
            name = reader.GetString(0);
        }
        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;

        

        //séparer le need selon les /(ou) et les ,(et)
        List<List<string>>  listName = new List<List<string>>();
        List<string> subList = new List<string>();

        //decoupage des / (ou)
        int conteur = 0;
        for (int p1 = 0; p1 < name.Length; p1++)
        {
            if (name[p1].Equals('/'))
            {

                subList.Add(name.Substring(conteur, p1 - conteur));
                conteur = p1+1;
            }
        }
        subList.Add(name.Substring(conteur, name.Length - conteur));

        List<string> subList2 = new List<string>();
        
        //découpage des , (et)
        foreach (string element in subList)
        {
            conteur = 0;
            for (int p1 = 0; p1 < element.Length; p1++)
            {
                if (element[p1].Equals(','))
                {

                    subList2.Add(element.Substring(conteur, p1 - conteur));
                    conteur = p1 + 1;
                }
            }
            subList2.Add(element.Substring(conteur, element.Length - conteur));

            listName.Add(new List<string>());
            foreach (string elem in subList2)
            {
                listName[listName.Count - 1].Add(elem);
            }
            subList2.Clear();
        }
        //on a donc listName la liste [[A,B],[C,D]] pour A,B/C,D


        //ENSUITE MODIFIER POUR QUE 9A MARCHE 5BOUCLE POUR TOU LES TEMRE FAIRE GAFE AU RETURN MEGA CHIANT 0 GERER°!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        foreach (List<string> element in listName)
        {
            string element0 = element[0]; //on simplifie sans ET pour l'instant

            connection = "URI=file:" + filepath;

            dbconn = new SqliteConnection(connection);

            dbconn.Open(); //Open connection to the database.
            dbcmd = dbconn.CreateCommand();

            sqlQuery = "SELECT type " + "FROM Brick WHERE name == " + "'" + element0 + "'"; //cherche le type du truc qu'on need
            dbcmd.CommandText = sqlQuery;
            reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                type = reader.GetString(0);
            }
            reader.Close();
            reader = null;

            dbcmd.Dispose();
            dbcmd = null;
            dbconn.Close();
            dbconn = null;

            if (element0.Equals("no"))
            {
                return ("yes");
            }
            else
            {//verifier aussi dans le cas d'un promoteurs que le gène est à coté !
                if (isPresent(element0) & !type.Equals("promotor"))
                {
                    return (activation(element0));
                }
                else if (isPresent(element0))
                {
                    GameObject promoteur = getByName(element0);
                    GameObject gene = getByName(brique);

                    if (areClose(promoteur, gene))
                    {
                        return (activation(element0));
                    }
                    else
                    {
                        //return ("no");
                    }
                }
                else
                {
                    //return ("no");
                }
            }
        }

        return ("no");
        ////SQL//////
    }

    private bool isPresent(string name) //informe sur laa presence de la brique sur le vecteur 
                                        //ET vérifie pour les promoteurs qu'ils sont bien en place Non pas faisable en fait (voir plus haut)
    {
        bool ok = false;

        foreach (GameObject item in genes)
        {
            if (item.GetComponent<objet1>().nom.Equals(name))
            {
                ok = true;
            }
        }
        foreach (GameObject item in pros)
        {
            if (item.GetComponent<objet1>().nom.Equals(name))
            {//ici vérifier la présence du gène à coté (objet précédent) infaisable enf ait
                ok = true;
            }
        }
        foreach (GameObject item in ehans)
        {
            if (item.GetComponent<objet1>().nom.Equals(name))
            {
                ok = true;
            }
        }
        foreach (string item in envList)
        {
            if (item.Equals(name))
            {
                ok = true;
            }
        }

        return (ok);
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

    private float d(Vector3 x1, Vector3 x2)
    {
        return (Mathf.Sqrt(Mathf.Pow((x1.x - x2.x), 2) + Mathf.Pow((x1.y - x2.y), 2) + Mathf.Pow((x1.z - x2.z), 2)));
    }

    private bool isPresentgene(string gene)
    {
        bool ok = false;

        foreach (GameObject item in genes)
        {
            if (item.GetComponent<objet1>().nom.Equals(gene))
            {
                ok = true;
            }
        }

        return ok;
    }

    private void debuglist(List<GameObject> liste)
    {
        foreach (GameObject item in liste)
        {
            Debug.Log(item.GetComponent<objet1>().nom.Equals(name));
        }
    }

    private GameObject getByName(string name)
    {
        foreach (GameObject item in genes)
        {
            if (item.GetComponent<objet1>().nom.Equals(name))
            {
                return (item);
            }
        }
        foreach (GameObject item in pros)
        {
            if (item.GetComponent<objet1>().nom.Equals(name))
            {
                return (item);
            }
        }
        foreach (GameObject item in ehans)
        {
            if (item.GetComponent<objet1>().nom.Equals(name))
            {
                return (item);
            }
        }
        foreach (GameObject item in repres)
        {
            if (item.GetComponent<objet1>().nom.Equals(name))
            {
                return (item);
            }
        }

        return (null);
    }

    private bool areClose(GameObject promoteur, GameObject gene) // dit si promoteur et gene sont a coté
    {
        float placeGene = Mathf.Atan2((gene.transform.position.x - Centre.x) / 150, (gene.transform.position.y - Centre.y) / 150);
        float placeProm = Mathf.Atan2((promoteur.transform.position.x - Centre.x) / 150, (promoteur.transform.position.y - Centre.y) / 150);

        if (placeGene > placeProm && placeGene - placeProm < 1.1)
        {
            return (true);
        }
        else
        {
            return (false);
        }
    }
}
