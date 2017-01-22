using UnityEngine;
using System.Collections;

public class MovementController : MonoBehaviour
{
    //private float minSpeed = 15.0f;
    [SerializeField] float maxSpeed = 10.0f;
    [SerializeField] float acceleration;
    [SerializeField] float _ZAccel;

    private Rigidbody rigidBody;

	void Start ()
	{
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.velocity = new Vector3(0.0f, 0.0f, maxSpeed / 10.0f);
    }

    void clampVelocity() {
        float speed = rigidBody.velocity.magnitude;
        //if(speed < minSpeed) rigidBody.velocity *= minSpeed / speed;
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
        float speed = rigidBody.velocity.magnitude;
        float xyAccel = acceleration * speed;

        Vector3 move = new Vector3(Input.GetAxis("Horizontal") * xyAccel, Input.GetAxis("Vertical") * xyAccel, _ZAccel);
        if(Mathf.Abs(rigidBody.velocity.y) > 0.5f * maxSpeed) {
            if(move.y > 0.0 && rigidBody.velocity.y > 0.0) move.y = 0;
            if(move.y < 0.0 && rigidBody.velocity.y < 0.0) move.y = 0;
        }
        rigidBody.AddRelativeForce(move);
    }

	void OnCollisionEnter(Collision collision)	{
		if (collision.transform.gameObject != gameObject) {
			Debug.LogFormat("HIT {0}", collision.transform.gameObject.name);
			rigidBody.velocity /= 2F;
		}
	}
}
