using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour {

	public float lifeTime = 2;

	public ParticleSystem explosion;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		lifeTime -= Time.deltaTime;

		if (lifeTime <= 0){
			Instantiate (explosion, transform.position, transform.rotation);
			Destroy (gameObject);
		}
	}

	void OnCollisionEnter(){
		Instantiate (explosion, transform.position, transform.rotation);
		Destroy (gameObject);
	}
}
