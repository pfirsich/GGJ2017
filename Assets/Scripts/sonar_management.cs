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

        InvokeRepeating("emitSonarFoward", 0.0f, 1000.0f);
    }

    void Update () {
        if(Input.GetButton("ping")) {
            emitSonar(new Vector4(0.0f, 0.0f, -1.0f, 0.0f));
        }

        float dt = Time.deltaTime;
        for(int i = 0; i < MAX_SONAR_COUNT; ++i) {
            sonarRadiuses[i] += dt * sonarGrowthSpeed;
        }

        sonarMat.SetFloat("_SonarRadius", sonarRadiuses[0]);
        sonarMat.SetVector("_SonarDirection", sonarDirections[0]);

        sonarMat.SetFloat("_SonarRadius1", sonarRadiuses[1]);
        sonarMat.SetVector("_SonarDirection1", sonarDirections[1]);

        sonarMat.SetFloat("_SonarRadius2", sonarRadiuses[2]);
        sonarMat.SetVector("_SonarDirection2", sonarDirections[2]);

        sonarMat.SetFloat("_SonarRadius3", sonarRadiuses[3]);
        sonarMat.SetVector("_SonarDirection3", sonarDirections[3]);

        sonarMat.SetFloat("_SonarRadius4", sonarRadiuses[4]);
        sonarMat.SetVector("_SonarDirection4", sonarDirections[4]);

        sonarMat.SetFloat("_SonarRadius5", sonarRadiuses[5]);
        sonarMat.SetVector("_SonarDirection5", sonarDirections[5]);

        sonarMat.SetFloat("_SonarRadius6", sonarRadiuses[6]);
        sonarMat.SetVector("_SonarDirection6", sonarDirections[6]);

        sonarMat.SetFloat("_SonarRadius7", sonarRadiuses[7]);
        sonarMat.SetVector("_SonarDirection7", sonarDirections[7]);
    }
}
