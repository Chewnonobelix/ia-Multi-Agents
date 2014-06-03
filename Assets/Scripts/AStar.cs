using UnityEngine;

using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;


public class AStar
{

    
    public class Path
    {
        public Path()
        {
            
        }
              
        public Noeud comeFrom = null;
        public float g_score = float.MaxValue;
        public float f_score = float.MaxValue;

        public static int sort(Path A, Path B)
        {
            if (A.f_score == B.f_score)
            {
                return 0;
            }
            else if (A.f_score > B.f_score)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
    }

    public class Noeud
    {

        public Noeud(Vector3 p, List<Noeud> n)
        {
            position = p;
            neighbours = n;
        }

      
        public Vector3 position;
        public List<Noeud> neighbours;
        public float myCost = 1;



      

    }






    /**
     * Complete neighbours for a standar x*y arena
     * X first, then Y
     * ex : 0 1 2
     *      3 4 5
     * sizeX = 3
     * sizeY = 2
     * 
     * @result a completed copy of noeuds
     */ 
    public static void autoCompleteNoeudNeighbour(List<Noeud> noeuds, int sizeX, int sizeY)
    {
        /*foreach (Noeud n in noeuds)
        {
            Debug.Log(n.position);
        }*/
       
        for (int y = 0; y < sizeY; y++)
        {
            for (int x = 0; x < sizeX; x++) 
            {
                int currentIndex = (y*sizeX) + x;
                // Right Neighbour
                if (x < sizeX-1)
                {
                    noeuds[currentIndex].neighbours.Add(noeuds[currentIndex + 1]);
                }

                // Left Neighbour
                if (x > 0 && sizeX > 1)
                {
                    noeuds[currentIndex].neighbours.Add(noeuds[currentIndex - 1]);
                }

                // Top Neighbour
                if (y > 0 && sizeY > 1)
                {
                    noeuds[currentIndex].neighbours.Add(noeuds[currentIndex - sizeX]);
                }

                // Bottom Neighbour
                if (y < sizeY-1)
                {
                    noeuds[currentIndex].neighbours.Add(noeuds[currentIndex + sizeX]);
                }
            }
        }

    }

    /**
     * find the nearest Noeud from the given position
     */
    public static AStar.Noeud findCase(Vector3 pos)
    {
       
        foreach (AStar.Noeud n in GameManager.constructMap())
        {

            if (n.position.Equals(pos))
                return n;
        }
        Debug.Log("no case found " + pos);
        return null;
    }


	/* Prend trop de temps !
	public static void resetNoeud(Noeud noeud, List<Noeud> noeudFais)
	{
		noeud.comeFrom = null;
		noeud.f_score = float.MaxValue;
		noeud.g_score = float.MaxValue;
		noeudFais.Add(noeud);
		foreach(Noeud n in noeud.neighbours)
		{
			if (!noeudFais.Contains(n))
			{
				resetNoeud (n,noeudFais);
			}
		}
	}*/


    public static List<Noeud> aStar(Noeud startNoeud, Noeud goalNoeud)
    {

        if (startNoeud == null || goalNoeud == null)
        {
            Debug.Log(startNoeud + " et " + goalNoeud);
            return null;
        }


        //Noeud theStart = new Noeud(noeudStart);
       // Noeud theGoal = new Noeud(noeudGoal);


        // INITIALISATION
		//resetNoeud (theStart,new List<Noeud>());
		Dictionary<Noeud,Path>  closedset = new Dictionary<Noeud,Path> ();
		Dictionary<Noeud,Path>  openset = new Dictionary<Noeud,Path> ();




       // Noeud startPath = new Noeud(noeudStart);
        Path startPath = new Path();
        startPath.g_score = 0;
        startPath.f_score = startPath.g_score + heuristic_cost_estimate(startNoeud, goalNoeud);
        openset.Add(startNoeud,startPath);


        // START
        while (openset.Count > 0)
        {
            // the node in openset having the lowest f_score[] value, the first because it is sorted !
            Path currentPath = openset.First().Value;
            Noeud currentNoeud = openset.First().Key;


            if (currentNoeud == goalNoeud)
            {
                closedset.Add(currentNoeud, currentPath);
                return reconstruct_path(closedset,currentNoeud); // reconstruct_path(came_from, goal);
            }
           // Debug.Log(currentNoeud.position);
            foreach (Noeud n in currentNoeud.neighbours)
            {
               // Debug.Log(currentPath.place.position + " a pour voisin " + c.position);
                if (!closedset.ContainsKey(n))
                {
                    float tentative_g_score = currentPath.g_score + n.myCost;
                    float tentative_f_score = tentative_g_score + heuristic_cost_estimate(n, goalNoeud);

                    if (openset.ContainsKey(n))
                    {
                        if (tentative_f_score < openset[n].f_score)
                        {
                            openset[n].f_score = tentative_f_score;
                            openset[n].g_score = tentative_g_score;
                            openset[n].comeFrom = currentNoeud;
                        }
                    }
                    else
                    {
                        Path newPath = new Path();
                        newPath.f_score = tentative_f_score;
                        newPath.g_score = tentative_g_score;
                        newPath.comeFrom = currentNoeud;

                        openset.Add(n, newPath);
                    }
                }
            }
            /*
            int gch;
            int dr;
            int bas;
            int haut;
            CalculVoisinCase(current, gch, dr, bas, haut);
            eachNeighbor(gch);
            eachNeighbor(dr);
            eachNeighbor(bas);
            eachNeighbor(haut);
            */

            closedset.Add(openset.First().Key, openset.First().Value);
            openset.Remove(openset.First().Key);

            // TODO Optimisation sort
            List<KeyValuePair<Noeud, Path>> myList = openset.ToList();
            myList.Sort(
                delegate(KeyValuePair<Noeud, Path> A,
                KeyValuePair<Noeud, Path> B)
                {
                    if (A.Value.f_score == B.Value.f_score)
                    {
                        return 0;
                    }
                    else if (A.Value.f_score > B.Value.f_score)
                    {
                        return 1;
                    }
                    else
                    {
                        return -1;
                    }
                }
            );
            openset.Clear();
            foreach (var person in myList)
            {
                openset.Add(person.Key, person.Value);
            }
            //openset.OrderBy(Path.sort);

        }

        //reconstruct_path();

        Debug.Log("AStar no path");
        return new List<Noeud>();


    }

    


    /**
     * Return the path to follow in the right order
     */
    static List<Noeud> reconstruct_path(Dictionary<Noeud,Path> labyrinthe, Noeud current)
    {
        List<Noeud> theConvertedPath = new List<Noeud>();
        
        if (labyrinthe[current].comeFrom != null)
        {
           // Noeud test = labyrinthe[current.Value.comeFrom];
            theConvertedPath.AddRange(reconstruct_path(labyrinthe,labyrinthe[current].comeFrom));

            theConvertedPath.Add(current);
        }
        
        

		//Debug.Log (theConvertedPath.Count);
        return theConvertedPath;
    }



    // distance a vol d oiseau
    static float heuristic_cost_estimate(Noeud thestart, Noeud thegoal)
    {
        // Troncature ??
        /*int calc1 = thestart / 3;
        int calc2 = thestart % 3;
        int calc3 = thegoal / 3;
        int calc4 = thegoal % 3;
   
        return sqrt((calc3 - calc1)*(calc3 - calc1) + (calc4 - calc2)*(calc4 - calc2));*/
        return Vector3.Distance(thestart.position, thegoal.position);
    }


}
