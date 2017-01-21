using UnityEngine;
using System.Collections;

public class sonar_management : MonoBehaviour {
    public Material sonarMat;
    public float sonarGrowthSpeed;

    private const int MAX_SONAR_COUNT = 8;
    private float[] sonarRadiuses;
    private Vector4[] sonarDirections;
    private int nextSonarIndex = 0;

    void emitSonar(Vector4 direction) {
        sonarRadiuses[nextSonarIndex] = 0.0f;
        sonarDirections[nextSonarIndex] = direction;
        nextSonarIndex += 1;
        if(nextSonarIndex >= MAX_SONAR_COUNT) nextSonarIndex = 0;
    }

    void emitSonarFoward() {
        emitSonar(new Vector4(0.0f, 0.0f, -1.0f, 0.0f));
    }

    void Start () {
        sonarRadiuses = new float[MAX_SONAR_COUNT];
        sonarDirections = new Vector4[MAX_SONAR_COUNT];
        for(int i = 0; i < MAX_SONAR_COUNT; ++i) {
            sonarRadiuses[i] = 0;
            sonarDirections[i] = new Vector4(0.0f, 0.0f, -1.0f, 0.0f);
        }

        InvokeRepeating("emitSonarFoward", 0.0f, 4.0f);
    }

    void Update () {
        if(Input.GetButton("ping")) {
            emitSonar(new Vector4(0.0f, 0.0f, -1.0f, 0.0f));
        }

        float dt = Time.deltaTime;
        for(int i = 0; i < MAX_SONAR_COUNT; ++i) {
            sonarRadiuses[i] += dt * sonarGrowthSpeed;
        }
        sonarMat.SetFloatArray("_SonarRadius", sonarRadiuses);
        sonarMat.SetVectorArray("_SonarDirection", sonarDirections);
    }
}
