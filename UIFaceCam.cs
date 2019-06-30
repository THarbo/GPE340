using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFaceCam : MonoBehaviour {

	public Camera theCam;

	// Use this for initialization
	void Awake () {
		theCam = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 targetVec = theCam.transform.position - transform.position;

		targetVec.x = targetVec.z = 0;

		transform.LookAt (theCam.transform.position - targetVec);

	}
}
