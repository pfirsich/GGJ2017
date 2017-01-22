using UnityEngine;
using System.Collections;

public class MovementControllerVR : MonoBehaviour {
#if UNITY_ANDROID
    private float minSpeed = 15.0f;
    private float maxSpeed = 20.0f;
    [SerializeField]
    float acceleration;

    private Rigidbody rigidBody;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.velocity = new Vector3(0.0f, 0.0f, maxSpeed);
    }

    void clampVelocity()
    {
        float speed = rigidBody.velocity.magnitude;
        if (speed < minSpeed) rigidBody.velocity *= minSpeed / speed;
        if (speed > maxSpeed) rigidBody.velocity *= maxSpeed / speed;
    }

    // Update is called once per frame
    void Update()
    {
        clampVelocity();
        transform.LookAt(transform.position + rigidBody.velocity);

        if (GvrController.ClickButtonDown)
        {
            sonar_management.instance.emitSonar(transform.forward);
        }
    }

    void FixedUpdate() // before internal physics update
    {
        clampVelocity();
        transform.LookAt(transform.position + rigidBody.velocity);

        Vector3 controllerForward = GvrController.Orientation * Vector3.forward; // vector
        controllerForward *= Camera.main.nearClipPlane / Vector4.Dot(Camera.main.transform.forward, controllerForward);
        Vector3 screenSpaceController = Camera.main.WorldToScreenPoint(controllerForward + Camera.main.transform.position /*<- point!*/);
        Debug.Log(screenSpaceController);
        screenSpaceController = new Vector3(screenSpaceController.x / Screen.width * 2.0f - 1.0f,
                                        screenSpaceController.y / Screen.height * 2.0f - 1.0f,
                                        0.0f);
        //Debug.Log(screenSpaceController);

        rigidBody.AddRelativeForce(screenSpaceController * acceleration*10f);
    }
#endif
}
