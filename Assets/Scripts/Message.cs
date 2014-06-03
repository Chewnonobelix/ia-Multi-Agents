using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Message {
    public GameObject expediteur ;

    public Performatif p = new Performatif();


    public class Performatif
    {
        public List<Vector3> caseAQuitter = new List<Vector3>();
        //Vector3 caseDestination;
    }
	
}
