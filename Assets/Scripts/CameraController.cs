using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
    [SerializeField] GameObject player;
    [SerializeField] float approachSpeed = 10.0f;
    [SerializeField] float targetDistance = 10.0f;
    [SerializeField] float lookAhead = 50.0f;
    [SerializeField] float lookAtSpeed = 10.0f;
    [SerializeField] float backOffset = 20.0f;
    private Vector3[] lastPlayerPos;
    private const int lastPlayerPosCount = 10;
    private int nextPlayerPosIndex = 0;

	float yOffset = 7;

	// Use this for initialization
	void Start () {
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



		Vector3 rel = (player.transform.position - forward * backOffset + new Vector3(0, yOffset, 0)) - transform.position;
        transform.position += rel.normalized * (rel.magnitude - targetDistance);// * approachSpeed * Time.deltaTime;

        //Vector3 targetPosition = player.transform.position - forward * backOffset;
        //transform.position += (targetPosition - transform.position) * Time.deltaTime * approachSpeed;

        transform.LookAt(player.transform.position + forward * lookAhead);
    }
}
