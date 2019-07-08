using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimAtMouse : MonoBehaviour
{

	public GameObject theBarrel;

	private void Update()
	{
		if (GameManager.Instance.isPaused){
			return;
		}

		// create a plane in the upward direction centered on the player
		Plane plane = new Plane (Vector3.up, transform.position);
		// create a raycast from the screen to the moust position
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		// float variable to hold hit info
		float distance;
		// if there is a collision between the screen and the direction of the ray...
		if (plane.Raycast (ray, out distance))
		{
			// ... then turn the character toward that collision
			transform.LookAt(ray.GetPoint (distance));
		}

		Ray barrelRay = Camera.main.ScreenPointToRay (Input.mousePosition);
		// float variable to hold hit info
		//float distance;
		RaycastHit hit;
		// if there is a collision between the screen and the direction of the ray...
		if (Physics.Raycast (barrelRay, out hit))
		{
			theBarrel.transform.LookAt (hit.point);
		}
	}
}