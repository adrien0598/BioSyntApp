using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objet1 : MonoBehaviour
{

    public Vector2 MousePosition;
    public Collider2D col;
    public Vector3 rotation;
    public float distance;
    public Vector3 CentreVecteur;
    public float rot;
    public Collider2D VectCol;
    public float rayon;
    public Vector3 PosIni;
    public Camera cam;
    public string nom;
    public SpriteRenderer spriteRenderer;

    public Sprite theSprite;


    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        cam = Camera.main;
        col = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetMouseButtonDown(1))
        //{
            //transform.position = PosIni;
            //FAIRE DETRUIRE PLUTOT
        //}
    }

    private float d(Vector3 x1, Vector3 x2)
    {
        return (Mathf.Sqrt(Mathf.Pow((x1.x - x2.x),2) + Mathf.Pow((x1.y - x2.y),2) + Mathf.Pow((x1.z - x2.z),2)));
    }

    private void OnMouseDrag()
    {
        MousePosition = Input.mousePosition;
        //Debug.Log(cam.ScreenToWorldPoint(MousePosition));
        Vector2 WorldP = new Vector2(cam.ScreenToWorldPoint(MousePosition).x, cam.ScreenToWorldPoint(MousePosition).y);
        transform.position = WorldP;
        distance = d(MousePosition, CentreVecteur);
        rot = Mathf.Atan2((WorldP.x - CentreVecteur.x) / distance, (WorldP.y - CentreVecteur.y) / distance);
        transform.rotation = Quaternion.Euler(0, 0,-(rot/Mathf.PI)*180);
        //transform.Rotate(0, 0, 1);
    }

    private void OnMouseUp()
    {
        MousePosition = Input.mousePosition;
        //Debug.Log(cam.ScreenToWorldPoint(MousePosition));
        Vector2 WorldP = new Vector2(cam.ScreenToWorldPoint(MousePosition).x, cam.ScreenToWorldPoint(MousePosition).y);
        distance = d(WorldP, CentreVecteur);
        if (distance < 190 && distance > 90)
        {
            //go to d == 150
            //Debug.Log("Distance au centre :");
            transform.position = (((new Vector3(WorldP.x, WorldP.y, 0) - CentreVecteur)*rayon)/distance) + CentreVecteur;
            //Debug.Log(d(transform.position, CentreVecteur));
        }
    }

    public Vector3 GetPosIni()
    {
        return (PosIni);
    }

    public void SetSprite(string spName)
    {
        Sprite sprite = Resources.Load<Sprite>(spName);
        spriteRenderer.sprite = sprite;
    }

    public void SetName(string name)
    {
        nom = name;
        //comme ça après il suffit pour chaque objet de faire GetComponent<objet1>().nom
    }

    public string GetName()
    {
        return (nom);
    }


}
