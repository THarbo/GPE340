using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cleanup : MonoBehaviour {

	private float timerDelete = 0.3f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		timerDelete -= Time.deltaTime;

		if (timerDelete <= 0f){
			Destroy (gameObject);
		}
		
	}
}
