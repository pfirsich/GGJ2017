using UnityEngine;
using System.Collections;

public class MovementController : MonoBehaviour
{
    [SerializeField]
    GameObject player;

    [SerializeField]
    float speed;
	// Use this for initialization
	void Start ()
	{

    }
	
	// Update is called once per frame
	void Update ()
	{
        player.transform.Translate(player.transform.forward*speed*Time.deltaTime);
	    float vertical = Input.GetAxis("Vertical");
	    float horizontal = Input.GetAxis("Horizontal");

	    Vector3 movementAxis = transform.up * vertical + transform.right * horizontal;
        player.transform.localRotation = Quaternion.FromToRotation(player.transform.forward, movementAxis.normalized);
    }
}
