using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllignGunLook : MonoBehaviour {

	// var to hold parent transform
	public Transform parentTrans;

	// Use this for initialization
	void Start () {
		//capture the transform
		parentTrans = gameObject.transform.parent;
	}
	
	// Update is called once per frame
	void Update () {
		//rotate the gun to look toward the rotation of the parent
		transform.rotation = parentTrans.rotation;
	}
}
