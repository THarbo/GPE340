using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamage : MonoBehaviour {
	
	public float damage = 50f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other){
		if (other.GetComponent<Health>()){
			other.GetComponent<Health>().DealDamage(damage);
		}

		if (other.GetComponent<Rigidbody> ()) {
			other.GetComponent<Rigidbody> ().AddForce ((other.transform.position - transform.position) * damage, ForceMode.Impulse);
		}
	}
}
