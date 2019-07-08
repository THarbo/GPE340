using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScreenSizer : MonoBehaviour {

	public Canvas theCanvas;

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<Image>().rectTransform.sizeDelta = new Vector2 (theCanvas.GetComponent<RectTransform>().rect.width, theCanvas.GetComponent<RectTransform>().rect.height);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
