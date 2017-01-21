using UnityEngine;
using System.Collections;

public class MovementController : MonoBehaviour
{
    private float minSpeed = 15.0f;
    private float maxSpeed = 20.0f;
    [SerializeField] float acceleration;

    private Rigidbody rigidBody;

	void Start ()
	{
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.velocity = new Vector3(0.0f, 0.0f, maxSpeed);
    }

	// Update is called once per frame
	void Update ()
	{
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0.0f);
        rigidBody.AddRelativeForce(move * acceleration);

        float speed = rigidBody.velocity.magnitude;
        if(speed < minSpeed) rigidBody.velocity *= minSpeed / speed;
        if(speed > maxSpeed) rigidBody.velocity *= maxSpeed / speed;

        transform.LookAt(rigidBody.position + rigidBody.velocity);
    }
}
