using UnityEngine;
using System.Collections;
 
[RequireComponent(typeof(CapsuleCollider))]
 
public class AgentController : MonoBehaviour 
{
 
private Vector3 screenPoint;
private Vector3 offset;

void Start ()
{
    Debug.Log("replacement");
    Vector3 goodPostion = transform.position;
    goodPostion.x = Mathf.Round(goodPostion.x);
    goodPostion.y = Mathf.Round(goodPostion.y);
    goodPostion.z = Mathf.Round(goodPostion.z);
    transform.position = goodPostion;
}
 
void OnMouseDown()
{
    screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
 
    offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    
 
}
 
void OnMouseDrag()
{
    Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
 
Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
curPosition.y = 0;
transform.position = curPosition;
 
}

void OnMouseUp()
{

    GameManager.moveGameObject(gameObject, transform.position);
   
}


}