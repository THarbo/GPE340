using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialManipulation : MonoBehaviour {

	private Renderer rend;
	private float scrollingSpd = 1f;
	private Pickup thePickup;

	// Use this for initialization
	void Awake () {
		rend = GetComponent<Renderer> ();
		thePickup = GetComponent<Pickup> ();
	}
	
	// Update is called once per frame
	void Update () {
		float offset = Time.time * scrollingSpd;

		if (thePickup.pickupType == Pickup.PickupType.SPEED ){
		rend.material.mainTextureOffset = new Vector2 (0,offset);
		}

		if (thePickup.pickupType == Pickup.PickupType.HEALTH ){			
			rend.material.mainTextureOffset = new Vector2 (0,offset * -1);
		}

		if (thePickup.pickupType == Pickup.PickupType.ARMOR ){
			rend.material.mainTextureOffset = new Vector2 (offset, 0);
		}
	}
}
