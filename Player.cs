using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : WeaponAgent {

	// max speed of player, public so able to be adjusted in inspector during dev
//	public float speed = 4f;
//
	// animator component attached to this character
	//private Animator theAnim;
//	// bool to check toggle sprinting, and timer for sprinting powerup
//	public bool sprinting = false;
//	public bool sprintCheck;
//	public float sprintTimer = 5f;
	//private float maxSprintTimer = 5f;
//	// jumpheight force
//	public float jump;
//	// the game object RB
//	public Rigidbody rb;
//	// bool the check if we are on the ground
//	public bool grounded = true;
//	// float for charge guns
//	public float shootForce = 50;
//
//
//
//
//
//
//
//	// weapon bools TODO: add this into a weaponagent and inherit to this script
//	public bool hasAssault = false;
//	public bool hasHandgun = false;
//	public ProjectileWeapon theWeapon;
//	public GameObject theBullet;
//	public GameObject theBarrel;
//	public GameObject handgunSpawn;
//	public GameObject rifleSpawn;
//
//	public Text ammoText;

	protected override void  Awake(){
		// make sure not to throw null errors in main menu
		base.Awake();
		Scene currentScene = SceneManager.GetActiveScene ();
		int sceneIndex = currentScene.buildIndex;
		if (sceneIndex == 0){			
			return;
		}
		if (theAnim == null) {
			theAnim = gameObject.GetComponent<Animator> ();
		}
		rb = gameObject.GetComponent<Rigidbody> ();
		ammoText = GameObject.Find ("AmmoForeground").GetComponent<Text>();
	
	}

	// Update is called once per frame
	void Update () {
		// make sure not to throw null errors in main menu
		Scene currentScene = SceneManager.GetActiveScene ();
		int sceneIndex = currentScene.buildIndex;
		if (currentScene.buildIndex == 0){
			theAnim.speed = 0.3f;
			return;
		}

		if (GameManager.Instance.isPaused){
			return;
		}

		// TODO Normalize the speed for reach animation

		// create a V3 with the horizontal/vertical axis input slotted into their appropriate axis
		Vector3 input = new Vector3 (Input.GetAxis ("Horizontal"), 0f, Input.GetAxis ("Vertical"));
		// clamp the input to max 1
		//input = Vector3.ClampMagnitude (input, 1f);
		// multiply the input by the speed var
		input *= speed;
		// set the float of the animator to the correct x/z values from the user input
		theAnim.SetFloat ("Horizontal", input.x);
		theAnim.SetFloat ("Vertical", input.z);

		// change in speed with sprint pickup, may need to make into own script
		if (sprinting) {
			theAnim.speed = 1.5f;
			sprintCheck = true;
		} else {
			theAnim.speed = 1.0f;
			sprintCheck = false;
		}

		if (sprintCheck){
			sprintTimer -= Time.deltaTime;
		}

		if (sprintTimer <= 0){
			sprinting = false;
			sprintTimer = maxSprintTimer;
		}

		if (grounded &&	Input.GetKeyDown(KeyCode.Space)){
			rb.AddForce (Vector3.up * jump, ForceMode.Impulse);
			Debug.Log ("Jumped");
			grounded = false;

		}

		// firing logic
		if (Input.GetMouseButton(0)){
			if (hasAssault || hasHandgun) {
				theWeapon.PullTrigger ();
				theWeapon.refire -= Time.deltaTime;
				if (theWeapon.refire <= 0) {
					theWeapon.refire = 0.2f;
				}
			}
			//shootForce += Time.deltaTime;
		}

		// pistol firing logic
		if (Input.GetMouseButtonUp(0)){
			if (hasAssault || hasHandgun) {
				theWeapon.hasFired = false;
			}
//			GameObject shot = Instantiate (theBullet, theBarrel.transform.position + (theBarrel.transform.forward * 0.4499f) + (theBarrel.transform.up * 0.0084f), transform.rotation);
//			shot.GetComponent<Rigidbody> ().AddForce (theBarrel.transform.forward * shootForce, ForceMode.Impulse);
//			Debug.Log ("shooting");
//			shootForce = minShootForce;
		}

		if (theWeapon){
			ammoText.text = theWeapon.ammo.ToString ();
		}

	}

	// if grounded can jump
	void OnCollisionStay(){
		//grounded = true;
	}

	void OnCollisionEnter(Collision other){
		if (other.collider.CompareTag("Level")){
			grounded = true;
		}
	}

	 //IK stuff, TODO: add to AI
//	protected virtual void OnAnimatorIK(){
//		if (!hasAssault && !hasHandgun){
//			return;
//		}
//		if (theWeapon.rightHandIKTarget) {
//			theAnim.SetIKPosition (AvatarIKGoal.RightHand, theWeapon.rightHandIKTarget.position);
//			theAnim.SetIKPositionWeight (AvatarIKGoal.RightHand, 1f);
//			theAnim.SetIKRotation (AvatarIKGoal.RightHand, theWeapon.rightHandIKTarget.rotation);
//			theAnim.SetIKRotationWeight (AvatarIKGoal.RightHand, 1f);
//		} else {
//			theAnim.SetIKPositionWeight (AvatarIKGoal.RightHand, 0f);
//			theAnim.SetIKRotationWeight (AvatarIKGoal.RightHand, 0f);
//
//		}
//
//		if (theWeapon.leftHandIKTarget) {
//			theAnim.SetIKPosition (AvatarIKGoal.LeftHand, theWeapon.leftHandIKTarget.position);
//			theAnim.SetIKPositionWeight (AvatarIKGoal.LeftHand, 1f);
//			theAnim.SetIKRotation (AvatarIKGoal.LeftHand, theWeapon.leftHandIKTarget.rotation);
//			theAnim.SetIKRotationWeight (AvatarIKGoal.LeftHand, 1f);
//		} else {
//			theAnim.SetIKPositionWeight (AvatarIKGoal.LeftHand, 0f);
//			theAnim.SetIKRotationWeight (AvatarIKGoal.LeftHand, 0f);
//
//		}
//	}

//	public void IterateLives(int iteration){
//		livesLeft += iteration;
//	}

}
