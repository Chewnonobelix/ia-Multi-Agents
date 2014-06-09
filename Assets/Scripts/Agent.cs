using UnityEngine;
using System.Collections.Generic;


// Execute une foi la liste des messages, et si l'autre ne renvoi pas de message, alors on bouge et zou ! c'est qu'on est plus rapide
// agir un coup en avance

public abstract class Agent : MonoBehaviour {
    
    public GameObject objectif;
    public float DistanceUnPas = 1;
    public float timeForNextStep = 1;

    public float nbDePas = 0;
    public float nbDePasMinimum;
    public float score = 0;

    protected List<Message> messages = new List<Message>();

    protected float timeLastStep = 0;

    public List<AStar.Noeud> CheminAPrendre;

    static int idcount = 0;
    public int myId;



    // Update is called once per frame
    void FixedUpdate()
    {

        // Temps d'action
        if (Time.fixedTime - timeLastStep > timeForNextStep)
        {
            Debug.Log("action");
            timeLastStep = Time.fixedTime;

            avancerVersObjectif();
        }
    }

    protected abstract void avancerVersObjectif();


   /* protected class CheminPossible
    {
        public Vector3 prochaineCase;
        public float distanceRestant;

        public CheminPossible(Vector3 p, float d)
        {
            prochaineCase = p;
            distanceRestant = d;
        }

        public static int sort(CheminPossible a, CheminPossible B)
        {
            if (a.distanceRestant == B.distanceRestant)
            {
                return 0;
            }
            else if (a.distanceRestant > B.distanceRestant)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
    }*/


   // static int a = 0;

    

  //  bool actionPossible = false;
    
	// Use this for initialization
	void Start () {

       

        myId = idcount;
        idcount++;

       /* Debug.Log(a);
        Debug.Log(a + " " + AStar.findCase(transform.position).position);
        Debug.Log(a + " " + AStar.findCase(objectif.transform.position).position);
       

        CheminAPrendre = AStar.aStar(AStar.findCase(transform.position), AStar.findCase(objectif.transform.position));

        foreach (AStar.Noeud n in CheminAPrendre)
        {
            Debug.Log(a + " " + n.position);

        }
        a++;*/
        

	}

    
   



    protected GameObject isColliding(Vector3 nextMove)
    {
        // Version en 3D
        /*Vector3 thisPosition = transform.position;
        transform.position = nextMove;

        GameObject returnValue = currentCollision;
        

        transform.position = thisPosition;
        
        currentCollision = null;
        return returnValue;*/

        foreach (GameObject a in GameManager.autresAgent)
        {
            if (a.transform.position == nextMove)
                return a;
        }
        return null;
    }

    protected Message isPositionReserved(Vector3 myPosition)
    {
        foreach (Message m in messages)
        {
            foreach (Vector3 posReserved in m.p.caseAQuitter)
            {
                //Debug.Log(myId + " " + posReserved);
                //Debug.Log(myId + " " + myPosition);
                if (posReserved == myPosition)
                {
                    return m;
                }
            }
        }

        return null;
    }

    protected void moveTo(Vector3 prochainePlace)
    {
        if (transform.position != prochainePlace)
        {


            prochainePlace.x = Mathf.Round(prochainePlace.x);
            prochainePlace.y = Mathf.Round(prochainePlace.y);
            prochainePlace.z = Mathf.Round(prochainePlace.z);

            
            nbDePas++;
            transform.position = prochainePlace;
           
          
            
        }
    }



    protected AStar.Noeud findNextMovement()
    {
        return findNextMovementAStar(transform.position, objectif.transform.position);
    }


    protected abstract AStar.Noeud findNextMovementAStar(Vector3 origine, Vector3 destination);

    public virtual void postMessage(Message m)
    {
        messages.Add(m);
    }

    public  void updateArena()
    {
        CheminAPrendre = null;
    }
   

}
