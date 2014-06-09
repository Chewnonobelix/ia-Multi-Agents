using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
    public static GameObject[] autresAgent;
    
    
    public int sizeX = 10;
    public int sizeY = 10;

    public GameObject caseA, caseB;

    static List<AStar.Noeud> cases = new List<AStar.Noeud>();

    public static Dictionary<AStar.Noeud, Temporalite> casesReserve = new Dictionary<AStar.Noeud, Temporalite>();

    public class Temporalite
    {
        public float debut;
        public float fin;
        public GameObject reserveur;

        public bool isOccuped(Temporalite otherTemp)
        {
            if (otherTemp.debut < debut && otherTemp.fin < fin ||
                otherTemp.debut > debut && otherTemp.fin > fin)
                return true;
            else
                return false;
        }
    }

    /**
     * Renvoi le premier Noeud occupé, null si tout est bon
     */
    public static KeyValuePair<AStar.Noeud, Temporalite> reserveCases(List<KeyValuePair<AStar.Noeud, Temporalite>> casesAReserver)
    {
        foreach (KeyValuePair<AStar.Noeud, Temporalite> car in casesAReserver)
        {
            if (casesReserve.ContainsKey(car.Key) && casesReserve[car.Key].isOccuped(car.Value))
                return car;
        }

        foreach (KeyValuePair<AStar.Noeud, Temporalite> car in casesAReserver)
        {
            casesReserve.Add(car.Key,car.Value);
        }
        return new KeyValuePair<AStar.Noeud, Temporalite>();
    }

    

	// Use this for initialization
	void Start () {
        if (autresAgent == null)
            autresAgent = GameObject.FindGameObjectsWithTag("Agent");


        bool cubeA = true;
        for (int y = 0; y < sizeY; y++)
        {

            for (int x = 0; x < sizeX; x++)
            {
                Vector3 pos = new Vector3(x  - sizeX /2 , 0, y  - sizeY / 2);
                if (cubeA)
                {
                    Instantiate(caseA, pos, Quaternion.identity);
                }
                else
                {
                    Instantiate(caseB, pos, Quaternion.identity);
                }
                cubeA = !cubeA;

                
                cases.Add(new AStar.Noeud(pos, new List<AStar.Noeud>()));
            }
            

            cubeA = !cubeA;
        }

        AStar.autoCompleteNoeudNeighbour(cases,sizeX,sizeY);
	
	}

    // Update is called once per frame
    void Update()
    {
	
	}


    void OnGUI()
    {
        if (GUI.Button(new Rect(20, 400, 90, 45), "Pause"))
        {

            if (Time.timeScale == 0)
            {
                Time.timeScale = 1;
            }
            else
            {
                Time.timeScale = 0;
            }
        }

        string msgAgent = "";
        float nbPasTotaux = 0;
        float scoreGeneral = 0;
        foreach (GameObject o in autresAgent)
        {
            Agent a = o.GetComponent<Agent>();
            nbPasTotaux += a.nbDePas;
            scoreGeneral += a.score;
            msgAgent += "Agent" + a.myId + " " + o.transform.position + "\n";
            msgAgent += "NbPas/NbPasMinimum :" + a.nbDePas + "/" + a.nbDePasMinimum + "\nScore :" + a.score + "\n";
        }
        GUI.TextArea(new Rect(10, 70, 170, 40 * autresAgent.Length), msgAgent);


        GUI.TextArea(new Rect(10, 10, 110, 60), "nbCoup : " + nbPasTotaux.ToString() + "\nScoreGe : " + scoreGeneral + "\nTemps :" + (Time.fixedTime).ToString());

        

      

    }

    public static void moveGameObject(GameObject o, Vector3 pos)
    {
        Vector3 roundPosition = pos;
        roundPosition.Set(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), Mathf.RoundToInt(pos.z));

        o.transform.position = roundPosition;

        foreach (GameObject a in autresAgent)
        {
            a.GetComponent<Agent>().updateArena();
        }


    }


   public static List<AStar.Noeud> constructMap()
    {
       return cases;
    }

}
