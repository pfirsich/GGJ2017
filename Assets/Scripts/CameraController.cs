using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
    [SerializeField] GameObject player;
    [SerializeField] float approachSpeed = 10.0f;
    [SerializeField] float targetDistance = 10.0f;
    [SerializeField] float lookAhead = 50.0f;
    [SerializeField] float lookAtSpeed = 10.0f;
    [SerializeField] Vector3 lookAtPosition;
    private Vector3[] lastPlayerPos;
    private const int lastPlayerPosCount = 10;
    private int nextPlayerPosIndex = 0;

	// Use this for initialization
	void Start () {
        lookAtPosition = player.transform.position;
        lastPlayerPos = new Vector3[lastPlayerPosCount];
	}

	// Update is called once per frame
	void Update () {
    }

    void FixedUpdate() {
        lastPlayerPos[nextPlayerPosIndex] = player.transform.forward;
        nextPlayerPosIndex += 1;
        if(nextPlayerPosIndex >= lastPlayerPosCount) nextPlayerPosIndex = 0;
        Vector3 forward = new Vector3(0.0f, 0.0f, 0.0f);
        for(int i = 0; i < lastPlayerPosCount; ++i) {
            forward += lastPlayerPos[i];
        }
        forward /= lastPlayerPosCount;

        Vector3 rel = player.transform.position - transform.position;
        transform.position += rel.normalized * (rel.magnitude - targetDistance);// * approachSpeed * Time.deltaTime;
        transform.LookAt(player.transform.position + forward * lookAhead);
    }
}
