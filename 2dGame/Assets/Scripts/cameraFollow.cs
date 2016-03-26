using UnityEngine;
using System.Collections;

public class cameraFollow : MonoBehaviour {

    public float xMax;
    public float yMax;
    public float xMin;
    public float yMin;

    private Transform target;

    // Use this for initialization
    void Start () {
        target = GameObject.Find("Brawler").transform;
	}
	
	// Update is called once per frame
	void Update () {

        transform.position = new Vector3 (Mathf.Clamp(target.position.x, xMin, xMax), Mathf.Clamp(target.position.y, yMin, yMax),transform.position.z);
	}
}
