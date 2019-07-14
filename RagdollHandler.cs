using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollHandler : MonoBehaviour {

	public bool ragdolling = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {		
		if (ragdolling){
			RagdollOn ();
		}
	}

	// happens when you die
	public void RagdollOn(){
		foreach (Rigidbody rb in gameObject.GetComponentsInChildren<Rigidbody> ()) {
			// change rb settings in child, and self
			rb.isKinematic = false;
			rb.useGravity = true;
		}

		foreach (Collider collide in gameObject.GetComponentsInChildren<Collider> ()) {
			// change collider setting in child and self
			collide.enabled = true;
		}

		// reenable collider/rb settings as they were for self, so there model will fall down and not fight the animator
		gameObject.GetComponent<Collider> ().enabled = false;
		gameObject.GetComponent<Rigidbody> ().isKinematic = true;
		gameObject.GetComponent<Animator> ().enabled = false;
		ragdolling = true;
	}

	// happens if you are brought back to life?
	public void RagdollOff(){
		foreach (Rigidbody rb in gameObject.GetComponentsInChildren<Rigidbody> ()) {
			// change rb settings in child, and self
			rb.isKinematic = true;
		}

		foreach (Collider collide in gameObject.GetComponentsInChildren<Collider> ()) {
			// change collider setting in child and self
			collide.enabled = false;
		}
		// reenable collider/rb settings as they were for self, so there model will fall down and not fight the animator
		gameObject.GetComponent<Collider> ().enabled = true;
		gameObject.GetComponent<Rigidbody> ().isKinematic = false;
		gameObject.GetComponent<Animator> ().enabled = true;
		ragdolling = false;
	}
}
