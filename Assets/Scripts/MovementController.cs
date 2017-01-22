using UnityEngine;
using System.Collections;
using System.Net.Sockets;

public class MovementController : MonoBehaviour
{
    //private float minSpeed = 15.0f;
    [SerializeField] float maxSpeed;
    [SerializeField] float acceleration;
    [SerializeField] float _ZAccel;
    [SerializeField] float riseSpeedLimit;
    private Rigidbody rigidBody;

	void Start ()
	{
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.velocity = new Vector3(0.0f, 0.0f, maxSpeed / 10.0f);
    }

    void clampVelocity() {
        float speed = rigidBody.velocity.magnitude;
        Debug.Log(rigidBody.velocity.magnitude);
        //if(speed < minSpeed) rigidBody.velocity *= minSpeed / speed;
        if(speed > maxSpeed) rigidBody.velocity *= maxSpeed / speed;
    }

    // Update is called once per frame
    void Update ()
    {
        clampVelocity();
        transform.LookAt(transform.position + rigidBody.velocity);
    #if !UNITY_STANDALONE_WIN && UNITY_HAS_GOOGLEVR && !UNITY_EDITOR
        if (GvrController.ClickButtonDown)
        {
            sonar_management.instance.emitSonar(transform.forward);
        }
    #endif
    }

    void FixedUpdate() // before internal physics update
    {
        Vector3 move = Vector3.zero;
        clampVelocity();
        transform.LookAt(transform.position + rigidBody.velocity);

        float speed = rigidBody.velocity.magnitude;
        float xyAccel = acceleration * speed;

#if !UNITY_STANDALONE_WIN && UNITY_HAS_GOOGLEVR && !UNITY_EDITOR
        Vector3 controllerForward = GameObject.FindGameObjectWithTag("GameController").transform.forward;
        move = controllerForward;
        move *= xyAccel;
        move.z = _ZAccel;
#else
        move = new Vector3(Input.GetAxis("Horizontal") * xyAccel, Input.GetAxis("Vertical") * xyAccel, _ZAccel);
#endif
        if (Mathf.Abs(rigidBody.velocity.y) > riseSpeedLimit * maxSpeed)
        {
            if (move.y > 0.0 && rigidBody.velocity.y > 0.0) move.y = 0;
            if (move.y < 0.0 && rigidBody.velocity.y < 0.0) move.y = 0;
        }
        Vector3 v = rigidBody.velocity;
        rigidBody.velocity = new Vector3(v.x, Mathf.Clamp(v.y, -riseSpeedLimit * v.magnitude, riseSpeedLimit * v.magnitude), v.z);
        rigidBody.AddRelativeForce(move);
    }

    void OnCollisionEnter(Collision collision)	{
		if (collision.transform.gameObject != gameObject) {
			Debug.LogFormat("HIT {0}", collision.transform.gameObject.name);
			rigidBody.velocity /= 2F;
		}
	}
}
