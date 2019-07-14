using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour {

	public float lifeTime = 2;

	public ParticleSystem explosionHit;
	public ParticleSystem explosionMiss;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		lifeTime -= Time.deltaTime;

		if (lifeTime <= 0){
			Instantiate (explosionHit, transform.position, transform.rotation);
			Destroy (gameObject);
		}
	}

	void OnCollisionEnter(Collision hit){

		GameObject collidedWith = hit.gameObject;

		if (hit.gameObject.CompareTag("Player") || hit.gameObject.CompareTag("AI")){
			Instantiate (explosionHit, transform.position, transform.rotation);
		}else {
			Instantiate (explosionMiss, transform.position, transform.rotation);
			GameManager.Instance.soundSource.PlayOneShot (GameManager.Instance.missSound);
		}

		Destroy (gameObject);
	}
}
