using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetAnimatorPosition : MonoBehaviour {

	// var to hold animator
	public Animator theAnim;

	// enum to 
	public enum Hand {
		LEFT,
		RIGHT
	}

	public Hand theHand = new Hand();


	// Use this for initialization
	void Awake () {
		theAnim = GetComponentInParent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (theHand == Hand.RIGHT) {
			transform.position = theAnim.GetBoneTransform (HumanBodyBones.RightHand).position;
			transform.rotation = theAnim.GetBoneTransform (HumanBodyBones.RightHand).rotation;
		} else {
			transform.position = theAnim.GetBoneTransform (HumanBodyBones.LeftHand).position;
			transform.rotation = theAnim.GetBoneTransform (HumanBodyBones.LeftHand).rotation;
		}

		Debug.DrawRay (transform.position, transform.forward, Color.green);

	}

}
