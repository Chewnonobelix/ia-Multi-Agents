using UnityEngine;
using System.Collections.Generic;


// Execute une foi la liste des messages, et si l'autre ne renvoi pas de message, alors on bouge et zou ! c'est qu'on est plus rapide
// agir un coup en avance

public class AgentV2 : Agent
{

    public bool strategieAgentSourd = false;

    protected override void avancerVersObjectif()
    {

        AStar.Noeud nextCase = findNextMovement();
        AStar.Noeud myCase = AStar.findCase(transform.position);

        AStar.Noeud caseFounded = myCase;

        if (nextCase == null)
            return;

        // Prepare les endroits où se poser
        List<AStar.Noeud> mouvementPossible = new List<AStar.Noeud>();
        mouvementPossible.Add(nextCase);
        mouvementPossible.Add(myCase);
        
        foreach (AStar.Noeud n in myCase.neighbours)
        {
            mouvementPossible.Add(n);
        }

     
        // Randomize les mouvements de deblocage
        for (int i = 0; i < mouvementPossible.Count; i++)
        {
            int a = Random.Range(2, mouvementPossible.Count);
            int b = Random.Range(2, mouvementPossible.Count);
            switchNoeud(mouvementPossible, a, b);
        }

       
        // recherche l'endroit viable !
        foreach (AStar.Noeud n in mouvementPossible)
        {
            GameObject agentBloquant = isColliding(n.position);
            Message messageBloquant = isPositionReserved(n.position);

            if (agentBloquant != null && agentBloquant != gameObject)
            {
                sendMessageToAgentBloquant(agentBloquant, n.position);
                caseFounded = myCase;
                
            }
            else if (messageBloquant != null)
            {
                Debug.Log(myId + "Message bloquant !!");
            }
            else
            {
                caseFounded = n;
                //Debug.Log(myId + " est bloqué. Se deplace vers " + nextCase);
                //CheminAPrendre = null;
                break;
            }
            Debug.Log(myId + " trouve que " + n.position + " n'est pas viable");
        }


        messages.Clear();
        moveTo(caseFounded.position);


    }

    private static void switchNoeud(List<AStar.Noeud> toSwitch, int pos1, int pos2)
    {
        AStar.Noeud save = toSwitch[pos1];
        toSwitch[pos1] = toSwitch[pos2];
        toSwitch[pos2] = save;
    }




    private void sendMessageToAgentBloquant(GameObject destinataire, Vector3 positionGenante)
    {
        Debug.Log(myId + " demande a " + destinataire.GetComponent<Agent>().myId + " de bouger de "+positionGenante);
        Message bougeToi = new Message();
        bougeToi.expediteur = gameObject;

        bougeToi.p.caseAQuitter.Add(positionGenante);

        destinataire.GetComponent<Agent>().postMessage(bougeToi);
    }





    protected override AStar.Noeud findNextMovementAStar(Vector3 origine, Vector3 destination)
    {
      

       // if (CheminAPrendre == null)
     //   {
            CheminAPrendre = AStar.aStar(AStar.findCase(transform.position), AStar.findCase(objectif.transform.position));
     //   }


        AStar.Noeud nextMov;
        if (CheminAPrendre.Count == 0)
        {
            Debug.Log(myId + " est arrivé");
            nextMov = AStar.findCase(transform.position);
        }
        else
        {
            if (transform.position == CheminAPrendre[0].position)
            {
                CheminAPrendre.Remove(CheminAPrendre[0]);
            }

            if (CheminAPrendre.Count == 0)
            {
                Debug.Log(myId + " est arrivé");
                nextMov = AStar.findCase(transform.position);
            }
            else
                nextMov = CheminAPrendre[0];
        }

        return nextMov;
    }

    /* Agent V0
     * protected AStar.Noeud findNextMovementDummy(Vector3 origine, Vector3 destination)
     {
         List<CheminPossible> nextMov = new List<CheminPossible>();


         nextMov.Add(new CheminPossible(origine, Vector3.Distance(origine, destination)));
      

         Vector3 nextPossibleMov = origine;
         nextPossibleMov.x += DistanceUnPas;
         nextMov.Add(new CheminPossible(nextPossibleMov, Vector3.Distance(nextPossibleMov, destination)));


         nextPossibleMov = origine;
         nextPossibleMov.x -= DistanceUnPas;
         nextMov.Add(new CheminPossible(nextPossibleMov, Vector3.Distance(nextPossibleMov, destination)));

         nextPossibleMov = origine;
         nextPossibleMov.z += DistanceUnPas;
         nextMov.Add(new CheminPossible(nextPossibleMov, Vector3.Distance(nextPossibleMov, destination)));

         nextPossibleMov = origine;
         nextPossibleMov.z -= DistanceUnPas;
         nextMov.Add(new CheminPossible(nextPossibleMov, Vector3.Distance(nextPossibleMov, destination)));

         nextMov.Sort(CheminPossible.sort);

         return nextMov;
     }*/


    public override void postMessage(Message m)
    {
        if (!strategieAgentSourd)
            messages.Add(m);
    }

}
