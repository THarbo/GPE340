using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour {

	public Camera gunUICam;
	public Text gunText;
	public Player thePlayer;

	public RawImage radialBack;
	public Image radialTarget;

	// hold the camera start position for reset
	private Vector3 camStartPoint;

	public Vector2 screenCenter;

//	 Use this for initialization
	void Start () {		
		// init the vars
		camStartPoint = gunUICam.transform.position;
		StartCoroutine("GetPlayer", 0.5f);
		screenCenter = new Vector2 (Screen.width / 2, Screen.height / 2);
	}
	
	// Update is called once per frame
	void Update () {
		// radial menu object
		if (Input.GetKey (KeyCode.RightShift)) {
			radialBack.enabled = true;
			if (Input.mousePosition.x < screenCenter.x && Input.mousePosition.y < screenCenter.y) {
				radialTarget.enabled = true;
				radialTarget.fillOrigin = (int)Image.Origin360.Bottom;
			}
			if (Input.mousePosition.x < screenCenter.x && Input.mousePosition.y > screenCenter.y) {
				radialTarget.enabled = true;
				radialTarget.fillOrigin = (int)Image.Origin360.Left;
			}
			if (Input.mousePosition.x > screenCenter.x && Input.mousePosition.y > screenCenter.y) {
				radialTarget.enabled = true;
				radialTarget.fillOrigin = (int)Image.Origin360.Top;
			}
			if (Input.mousePosition.x > screenCenter.x && Input.mousePosition.y < screenCenter.y) {
				radialTarget.enabled = true;
				radialTarget.fillOrigin = (int)Image.Origin360.Right;
			}
		} else {
			radialBack.enabled = false;
			radialTarget.enabled = false;
		}

		if (thePlayer) {
			// if the player has no gun then stay where you are
			if (!thePlayer.hasAssault && !thePlayer.hasHandgun) {
				gunUICam.transform.position = camStartPoint;
			}

			// when player has assault, move to show assault, TODO: MAKE THIS NOT HARDCODED
			if (thePlayer.hasAssault) {
				gunUICam.transform.position = new Vector3 (camStartPoint.x + 2, camStartPoint.y, camStartPoint.z);
			}

			// when player has handgun, move to show assault, TODO: MAKE THIS NOT HARDCODED
			if (thePlayer.hasHandgun) {
				gunUICam.transform.position = new Vector3 (camStartPoint.x + 4, camStartPoint.y, camStartPoint.z);
			}

			if (thePlayer.theWeapon) {
				gunText.text = thePlayer.theWeapon.ammo.ToString ();
			} else {
				gunUICam.transform.position = camStartPoint;
				gunText.text = " ";
			}
		}
		// when player dies move back to start pos
		if (!thePlayer) {			
			gunText.text = " ";
			gunUICam.transform.position = camStartPoint;
			gunUICam = GameObject.FindGameObjectWithTag ("UICamera").GetComponent<Camera> ();
			StartCoroutine ("GetPlayer", 0.5f);
		}
	}

	IEnumerator GetPlayer(float wait){
		yield return new WaitForSeconds (wait);
		thePlayer = GameObject.FindGameObjectWithTag ("Player").GetComponent < Player> ();
	}
}
