using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
    [SerializeField] GameObject player;
    [SerializeField] float approachSpeed = 10.0f;
    [SerializeField] float targetDistance = 10.0f;

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
        Vector3 rel = player.transform.position - transform.position;
        transform.position += rel.normalized * (rel.magnitude - targetDistance) * approachSpeed * Time.deltaTime;
        transform.LookAt(player.transform.position);
	}
}
