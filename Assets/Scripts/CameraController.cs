using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
    [SerializeField] GameObject player;
    [SerializeField] float approachSpeed = 10.0f;
    [SerializeField] float targetDistance = 10.0f;
    [SerializeField] float lookAhead = 50.0f;
    [SerializeField] float lookAtSpeed = 10.0f;
    [SerializeField] Vector3 lookAtPosition;

	// Use this for initialization
	void Start () {
        lookAtPosition = player.transform.position;
	}

	// Update is called once per frame
	void Update () {
        float dt = Time.deltaTime;

        Vector3 rel = player.transform.position - transform.position;
        transform.position += rel.normalized * (rel.magnitude - targetDistance) * approachSpeed * Time.deltaTime;

        Vector3 lookAtTarget = player.transform.position + player.transform.forward.normalized * lookAhead;
        lookAtPosition += (lookAtTarget - lookAtPosition) * lookAtSpeed * dt;
        transform.LookAt(lookAtPosition);
	}
}
