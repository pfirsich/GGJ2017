using UnityEngine;
using System.Collections;

public class BugMovementController : MonoBehaviour
{
    public bool alive;
	// Use this for initialization
	void Awake ()
	{
	    alive = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
	    if (alive)
	    {
	        Vector3 thisToCenter = transform.parent.position - transform.position;
	        Quaternion rot = Quaternion.AngleAxis(Random.value*3, Random.onUnitSphere);
	        transform.localPosition = rot * transform.localPosition;
	        transform.LookAt(transform.parent);
	    }
	}
}
