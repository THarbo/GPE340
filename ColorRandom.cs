using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorRandom : MonoBehaviour {

	private Color desiredColor;
	private Color startingColor;
	private bool randomizing = false;

	// Use this for initialization
	void Start () {
		//theMat = gameObject.GetComponent<Material> ();
		startingColor = gameObject.GetComponent<Text>().color;
		desiredColor = startingColor;
	}
	
	// Update is called once per frame
	void Update () {

		if (randomizing) {
			gameObject.GetComponent<Text>().color = Color.Lerp (gameObject.GetComponent<Text>().color, desiredColor, Mathf.PingPong(Time.time, 5)/50);
		}

		if (startingColor != desiredColor) {
			randomizing = true;
		} else {
			randomizing = false;
		}

		desiredColor = new Color ( Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1);

	





	}
}
