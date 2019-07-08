using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {

	public Canvas theCanvas;
	public Image pauseScreen;
	public GameObject gameOverScreen;

	public GameObject[] children;

	// Use this for initialization
	void Start () {		
		children = new GameObject[transform.childCount];
		for (int i = 0; i < transform.childCount; i++) {
			GameObject currentChild = transform.GetChild (i).gameObject;
			children [i] = currentChild;
		}

		pauseScreen.rectTransform.sizeDelta = new Vector2 (theCanvas.GetComponent<RectTransform>().rect.width, theCanvas.GetComponent<RectTransform>().rect.height);
	}
	
	// Update is called once per frame
	void Update () {
		if (GameManager.Instance.isPaused && !GameManager.Instance.gameOver) {
			foreach (GameObject obj in children) {
				obj.SetActive (false);
			}
			pauseScreen.gameObject.SetActive (true);
			pauseScreen.enabled = true;
		} else {
			foreach (GameObject obj in children) {
				obj.SetActive (true);
			}
			pauseScreen.gameObject.SetActive (false);
			pauseScreen.enabled = false;
		}

		if (!GameManager.Instance.gameOver) {
			gameOverScreen.SetActive (false);
		}
	}
}
