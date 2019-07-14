using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaAdjust : MonoBehaviour {

	public Material goMaterial;
	public float alphaValue = 1;
	private float timeToWait = 1;
	public bool loading = true;
	public bool restarted = false;


	// Use this for initialization
	void Awake () {
		Time.timeScale = 1.0f;
		loading = true;
		StartCoroutine ("WaitToFade", timeToWait);
		goMaterial.color = new Color (0.25f, 0.239f, 0.239f, 1.0f);
	}
	
	// Update is called once per frame
	void Update () {		
		if (loading == true ){
			return;
		}

		alphaValue -= Time.deltaTime / 5;	
		goMaterial.color = new Color (goMaterial.color.r, goMaterial.color.g, goMaterial.color.b, alphaValue);


		if (alphaValue <= 0){
			Destroy (gameObject);
		}

	}

	IEnumerator WaitToFade(float wait){
		
		yield return new WaitForSeconds (wait);
			loading = false;
	}
}
