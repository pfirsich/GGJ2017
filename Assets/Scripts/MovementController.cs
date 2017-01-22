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

    void clampVelocity() {
        float speed = rigidBody.velocity.magnitude;
        if(speed < minSpeed) rigidBody.velocity *= minSpeed / speed;
        if(speed > maxSpeed) rigidBody.velocity *= maxSpeed / speed;
    }

    // Update is called once per frame
    void Update ()
    {
        clampVelocity();
        transform.LookAt(transform.position + rigidBody.velocity);
    }

    void FixedUpdate() // before internal physics update
    {
        clampVelocity();
        transform.LookAt(transform.position + rigidBody.velocity);
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0.0f);
        rigidBody.AddRelativeForce(move * acceleration);
    }

	void OnCollisionEnter(Collision collision)	{
		if (collision.transform.gameObject != gameObject) {
			Debug.LogFormat("HIT {0}", collision.transform.gameObject.name);
			rigidBody.velocity /= 2F;
		}
	}
}
