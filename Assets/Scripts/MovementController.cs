using UnityEngine;
using System.Collections;

public class MovementController : MonoBehaviour
{
    [SerializeField]
    GameObject player;

    [SerializeField]
    float speed;

    Vector3 direction;
	// Use this for initialization
	void Start ()
	{
        direction = transform.position - player.transform.position;
    }
	
	// Update is called once per frame
	void Update ()
	{
        player.transform.Translate(player.transform.forward*speed*Time.deltaTime);
	    float vertical = Input.GetAxis("Vertical");
	    float horizontal = Input.GetAxis("Horizontal");
	    Vector3 movementAxis = Vector3.up * vertical + Vector3.right * horizontal;


	    Quaternion rot = Quaternion.FromToRotation();
        transform.position = player.transform.position + rot * direction;
        transform.localRotation = rot;
    }
}
