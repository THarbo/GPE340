using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponAgent : MonoBehaviour {

	// max speed of player, public so able to be adjusted in inspector during dev
	public float speed = 4f;

	// animator component attached to this character
	protected Animator theAnim;
	// bool to check toggle sprinting, and timer for sprinting powerup
	public bool sprinting = false;
	public bool sprintCheck;
	public float sprintTimer = 5f;
	protected float maxSprintTimer = 5f;
	// jumpheight force
	public float jump;
	// the game object RB
	public Rigidbody rb;
	// bool the check if we are on the ground
	public bool grounded = true;
	// float for charge guns
	public float shootForce = 50;


	// weapon bools TODO: add this into a weaponagent and inherit to this script
	public bool hasAssault = false;
	public bool hasHandgun = false;
	public ProjectileWeapon theWeapon;
	public GameObject theBullet;
	public GameObject theBarrel;
	public GameObject handgunSpawn;
	public GameObject rifleSpawn;

	public Text ammoText;

	void Awake(){
		if (theAnim == null) {
			theAnim = gameObject.GetComponent<Animator> ();
		}
		rb = gameObject.GetComponent<Rigidbody> ();
		if (gameObject.GetComponent<Player>()){
			ammoText = GameObject.Find ("AmmoForeground").GetComponent<Text>();
		}


	}

	// Update is called once per frame
	void Update () {
		
//		if (GameManager.Instance.isPaused){
//			return;
//		}
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

	// IK stuff, TODO: add to AI
	protected virtual void OnAnimatorIK(){
		if (!hasAssault && !hasHandgun){
			return;
		}
		if (theWeapon.rightHandIKTarget) {
			theAnim.SetIKPosition (AvatarIKGoal.RightHand, theWeapon.rightHandIKTarget.position);
			theAnim.SetIKPositionWeight (AvatarIKGoal.RightHand, 1f);
			theAnim.SetIKRotation (AvatarIKGoal.RightHand, theWeapon.rightHandIKTarget.rotation);
			theAnim.SetIKRotationWeight (AvatarIKGoal.RightHand, 1f);
		} else {
			theAnim.SetIKPositionWeight (AvatarIKGoal.RightHand, 0f);
			theAnim.SetIKRotationWeight (AvatarIKGoal.RightHand, 0f);

		}

		if (theWeapon.leftHandIKTarget) {
			theAnim.SetIKPosition (AvatarIKGoal.LeftHand, theWeapon.leftHandIKTarget.position);
			theAnim.SetIKPositionWeight (AvatarIKGoal.LeftHand, 1f);
			theAnim.SetIKRotation (AvatarIKGoal.LeftHand, theWeapon.leftHandIKTarget.rotation);
			theAnim.SetIKRotationWeight (AvatarIKGoal.LeftHand, 1f);
		} else {
			theAnim.SetIKPositionWeight (AvatarIKGoal.LeftHand, 0f);
			theAnim.SetIKRotationWeight (AvatarIKGoal.LeftHand, 0f);

		}
	}

}
