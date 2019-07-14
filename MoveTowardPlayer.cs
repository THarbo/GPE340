using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowardPlayer : MonoBehaviour {

	// player obj
	public GameObject thePlayer;

	// movement speed of camera
	public float speed = 1.0f;
	// camera offset, TODO: MAKE THIS NON-STATIC SO THAT IT CAN BE CHANGED
	private Vector3 offset = new Vector3(0,8,-11);


	// unused in this iteration, script failed to operate
	private CameraRotate theRotator;

	// vars for zooming/rotating
	public Vector3 targetLookAt;
	private Vector3 rotatedOffset;
	private Vector3 nonZoomedRotatedOffset;
	public bool upLook = false;
	private float timeToOrient = 0;


	void Awake(){
		// grab the rotator, unused as of week 1
		theRotator = GetComponent<CameraRotate> ();
		rotatedOffset = offset;

	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (thePlayer == null) {
			thePlayer = GameObject.FindGameObjectWithTag ("Player");
		}

		if (thePlayer != null) {

			if (Input.GetKeyDown (KeyCode.E)) {
				if (!upLook) {
					timeToOrient = 0;
					upLook = true;
				} else {
					timeToOrient = 0;
					upLook = false;
				}
			}

			if (!upLook) {
				timeToOrient += Time.deltaTime;
				targetLookAt = Vector3.Lerp (thePlayer.transform.position + (thePlayer.transform.up * 3), thePlayer.transform.position, timeToOrient);
			} else {
				timeToOrient += Time.deltaTime;
				targetLookAt = Vector3.Lerp (thePlayer.transform.position, thePlayer.transform.position + (thePlayer.transform.up * 3), timeToOrient);
			}

			// create a float that multiplies speed by time to create a smooth movement
			float movement = speed * Time.deltaTime;

			// move the camera 
			if (Input.GetKeyDown (KeyCode.LeftShift)) {
				// Get correct position
				rotatedOffset = Quaternion.LookRotation (thePlayer.transform.forward) * offset;
			}

			transform.position = Vector3.MoveTowards (transform.position, thePlayer.transform.position + rotatedOffset, movement);
			transform.LookAt (targetLookAt);

			if (timeToOrient >= 10) {
			
			}

		}


	}
	//TODO: make this work
//	void ZoomCamera(){
//		
//	}
}
