﻿using UnityEngine;
using System.Collections;

public class sonar_management : MonoBehaviour {
    [SerializeField] GameObject bat;

    public static sonar_management instance;

    public Material sonarMat;
    public float sonarGrowthSpeed;

    private const int MAX_SONAR_COUNT = 8;
    private float[] sonarRadiuses;
    private Vector4[] sonarDirections;
    private Vector4[] sonarPositions;
    private int nextSonarIndex = 0;

	private AudioSource audio;

    public void emitSonar(Vector4 direction) {
        Debug.Log(nextSonarIndex);
        sonarRadiuses[nextSonarIndex] = 0.0f;
        sonarDirections[nextSonarIndex] = direction;
        Vector3 v = bat.transform.position;
        Vector4 pos = new Vector4(v.x, v.y, v.z, 1.0f);
        sonarPositions[nextSonarIndex] = pos;
        nextSonarIndex += 1;
        if(nextSonarIndex >= MAX_SONAR_COUNT) nextSonarIndex = 0;
    }

    void emitSonarFoward() {
        emitSonar(new Vector4(0.0f, 0.0f, -1.0f, 0.0f));
    }

    void Start ()
    {
        instance = this;
        sonarRadiuses = new float[MAX_SONAR_COUNT];
        sonarDirections = new Vector4[MAX_SONAR_COUNT];
        sonarPositions = new Vector4[MAX_SONAR_COUNT];
        for(int i = 0; i < MAX_SONAR_COUNT; ++i) {
            sonarRadiuses[i] = 0;
            sonarDirections[i] = new Vector4(0.0f, 0.0f, -1.0f, 0.0f);
        }
		audio = bat.GetComponent<AudioSource>();

        InvokeRepeating("emitSonarFoward", 0.0f, 1000.0f);
    }

    void Update () {
        if(Input.GetButtonDown("ping")) {
            emitSonar(bat.transform.forward);
			audio.Play();
        }

        float dt = Time.deltaTime;
        for(int i = 0; i < MAX_SONAR_COUNT; ++i) {
            sonarRadiuses[i] += dt * sonarGrowthSpeed;

            string suffix = i.ToString();
            if(i == 0) suffix = "";
            sonarMat.SetFloat("_SonarRadius" + suffix, sonarRadiuses[i]);
            sonarMat.SetVector("_SonarDirection" + suffix, sonarDirections[i]);
            sonarMat.SetVector("_SonarPosition" + suffix, sonarPositions[i]);
        }
    }
}
