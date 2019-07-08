using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour {
	
	// the camera componenet will need to be added to the player spawning code eventually,
	// for now it is just the main camera in the scene
	// public var for target object, added in inspector

	[SerializeField] private Transform  player = null;
	[SerializeField] private Vector3    offset = new Vector3(0,8,-11);

		// Update is called once per frame
		void Update()
		{
			if (Input.GetKeyDown(KeyCode.LeftShift)){
				// Get correct position
				Vector3 rotatedOffset = Quaternion.LookRotation(player.forward) * offset;
				// use the position to move the camera
				transform.position = player.transform.position + rotatedOffset;
			}
		}
	}