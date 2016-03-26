using UnityEngine;
using System.Collections;

public class parallaxing : MonoBehaviour {

    public Transform[] backgrounds; //  Array of all the backgrounds/foregrounds to parallax
    private float[] prlxScales; //proportion of camera's mvmt to move the backgrounds by
    public float smooth = 1f;        //how smooth. must be > 0

    private Transform cam;      //reference to main camera's transform
    private Vector3 previousCamPos;     //previous frame
    
    //called before Start()
    void Awake() {
        cam = Camera.main.transform;
    }
	// Use this for initialization
	void Start () {
        //Previous frame had current frame's camera position
        previousCamPos = cam.position;

        //assign parallax scales
        prlxScales = new float[backgrounds.Length];
        for (int i = 0; i < backgrounds.Length; i++){
            prlxScales[i] = backgrounds[i].position.z*-1;
        }
	}
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < backgrounds.Length; i++){
            float parallax = (previousCamPos.x - cam.position.x) * prlxScales[i];

            float backgroundTargetPosX = backgrounds[i].position.x + parallax;

            //Create target position: bg_current pos + its target x Pos
            Vector3 backgroundTargetPos = new Vector3 (backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);

            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smooth * Time.deltaTime);
        }
        //set previous camPos to cam's pos at end of frame
        previousCamPos = cam.position;
	}
}
