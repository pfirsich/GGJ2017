using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class BugSpawner : MonoBehaviour {
    [SerializeField] GameObject
    bugTemplate;

    [SerializeField]
    int numberOfBugs;
[SerializeField]
    float spawnDistance;

    public List<GameObject> bugs;

    void Awake()
    {
        bugs = new List<GameObject>();
    }
	// Use this for initialization
	IEnumerator Start ()
	{
	    while (bugs.Count < numberOfBugs)
	    {
	        yield return StartCoroutine(spawnBug());
	    }
        Debug.LogFormat("spawned {0} bugs at {1}", bugs.Count, transform.position);
	    yield return null;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator spawnBug()
    {
        GameObject go = Instantiate(bugTemplate);
        go.transform.parent = transform;
        go.transform.localPosition = Random.onUnitSphere*spawnDistance;
        go.transform.LookAt(transform);
        bugs.Add(go);
        go.GetComponent<BugMovementController>().alive = true;
        yield return null;
    }
}
