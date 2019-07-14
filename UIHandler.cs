using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.Audio;
using UnityEngine.Events;

public class UIHandler : MonoBehaviour {

	public UIHandler Instance;



	// audio 
	[SerializeField, Tooltip("Master Audio Mixer")]
	private AudioMixer audioMixer;
	[SerializeField, Tooltip("Slider value vs decibel volume curve")]
	private AnimationCurve volumeVsDecibels;
	private float startingVolume = 50;
	private float maxVolume = 100;

	// canvas sections
	public Canvas theCanvas;
	public Image pauseScreen;
	public GameObject gameOverScreen;
	public GameObject mainMenu;

	public bool winCon = false;
	public GameObject winText;
	public GameObject lossText;
	public GameObject miniMap;

	// gun UI 
	public Camera gunUICam;
	public Text gunText;
	// hold the gun UI camera start position for reset
	private Vector3 camStartPoint;

	// player
	public Player thePlayer;

	// radial menu, unused 
	public RawImage radialBack;
	public Image radialTarget;
	private Vector2 screenCenter;


	// adjustable items
	public Dropdown GFXDrop;
	public Dropdown ResDrop;
	public Slider masterSlider;
	public Slider musicSlider;
	public Slider soundSlider;

	private Resolution[] resolutionArray;
	private int[] resX;
	private int[] resY;

	public GameObject[] children;

//	 Use this for initialization
	void Start () {	

		// make sure not to throw null errors in main menu
		Scene currentScene = SceneManager.GetActiveScene ();
		int sceneIndex = currentScene.buildIndex;
		if (sceneIndex == 0){
			// init res options
			resolutionArray = Screen.resolutions;
			resX = new int[resolutionArray.Length];
			resY = new int[resolutionArray.Length];
			ResDrop.ClearOptions ();
			List<string> resolutions = new List<string> ();
			for (int i = 0; i < Screen.resolutions.Length; i++){
				resolutions.Add (string.Format ("{0} x {1}", Screen.resolutions [i].width, Screen.resolutions [i].height));
				resX [i] = Screen.resolutions [i].width;
				//Debug.Log ("resX " + i + " = " + Screen.resolutions [i].width);
				resY [i] = Screen.resolutions [i].height;
				if (Screen.currentResolution.width == Screen.resolutions [i].width && Screen.currentResolution.height == Screen.resolutions [i].height){
					//if (Screen.currentResolution.height == Screen.resolutions [i].height){
						ResDrop.value = i;
					//}
				} 
			}
			ResDrop.AddOptions (resolutions);
			// init the gfx options
			GFXDrop.ClearOptions ();
			GFXDrop.AddOptions (QualitySettings.names.ToList ());
			GFXDrop.value = QualitySettings.GetQualityLevel ();
			return;


		} 

		masterSlider.value = PlayerPrefs.GetFloat ("Master Volume", startingVolume);
		musicSlider.value = PlayerPrefs.GetFloat ("Music Volume", startingVolume);
		soundSlider.value = PlayerPrefs.GetFloat ("Sound Volume", startingVolume);

		// change render mode to overlay to avoid occlusion

		//theCanvas.renderMode = RenderMode.ScreenSpaceCamera;

		// init the vars
		camStartPoint = gunUICam.transform.position;
		StartCoroutine("GetPlayer", 0.5f);
		screenCenter = new Vector2 (Screen.width / 2, Screen.height / 2);

		children = new GameObject[transform.childCount];
		for (int i = 0; i < transform.childCount; i++) {
			GameObject currentChild = transform.GetChild (i).gameObject;
			children [i] = currentChild;
		}	

//		GameManager.Instance.gameOverEvent.AddListener (GameOverUI);
//		GameManager.Instance.pauseEvent.AddListener (PauseUI);

	}
	
	// Update is called once per frame
	void Update () {
		
		// make sure not to throw null errors in main menu
		Scene currentScene = SceneManager.GetActiveScene ();
		int sceneIndex = currentScene.buildIndex;
		if (currentScene.buildIndex == 0){
			return;
		}

		// if the game isnt paused or game over dont show the screen
//		if (GameManager.Instance.isPaused && !GameManager.Instance.gameOver) {			
//			foreach (GameObject obj in children) {
//				obj.SetActive (false);
//			}
//			pauseScreen.gameObject.SetActive (true);
//			pauseScreen.enabled = true;
//		} else {
//			theCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
//			// but if it is paused show the screen
//			foreach (GameObject obj in children) {
//				obj.SetActive (true);
//			}
//			pauseScreen.gameObject.SetActive (false);
//			pauseScreen.enabled = false;
//			mainMenu.SetActive (false);
//		}



		// as long as the game isnt over dont show the gameover screen
		//if (!GameManager.Instance.gameOver) {
		//	gameOverScreen.SetActive (false);
		//} //else {			
//			miniMap.SetActive (false);
//			GameManager.Instance.isPaused = true;
//			if (winCon) {
//				winText.SetActive (true);
//			} else {
//				lossText.SetActive (true);
//			}
		//}

		// radial menu object
		if (Input.GetKey(KeyCode.C)) {
			radialBack.enabled = true;
			if (Input.mousePosition.x < screenCenter.x && Input.mousePosition.y < screenCenter.y) {
				radialTarget.enabled = true;
				radialTarget.fillOrigin = (int)Image.Origin360.Bottom;
			}
			if (Input.mousePosition.x < screenCenter.x && Input.mousePosition.y > screenCenter.y) {
				radialTarget.enabled = true;
				radialTarget.fillOrigin = (int)Image.Origin360.Left;
			}
			if (Input.mousePosition.x > screenCenter.x && Input.mousePosition.y > screenCenter.y) {
				radialTarget.enabled = true;
				radialTarget.fillOrigin = (int)Image.Origin360.Top;
			}
			if (Input.mousePosition.x > screenCenter.x && Input.mousePosition.y < screenCenter.y) {
				radialTarget.enabled = true;
				radialTarget.fillOrigin = (int)Image.Origin360.Right;
			}
		} else {
			radialBack.enabled = false;
			radialTarget.enabled = false;
		}

		// check to see if the player is here and then update UI pieces
		if (thePlayer) {
			// if the player has no gun then stay where you are
			if (!thePlayer.hasAssault && !thePlayer.hasHandgun) {
				gunUICam.transform.position = camStartPoint;
			}

			// when player has assault, move to show assault, TODO: MAKE THIS NOT HARDCODED
			if (thePlayer.hasAssault) {
				gunUICam.transform.position = new Vector3 (camStartPoint.x + 2, camStartPoint.y, camStartPoint.z);
			}

			// when player has handgun, move to show assault, TODO: MAKE THIS NOT HARDCODED
			if (thePlayer.hasHandgun) {
				gunUICam.transform.position = new Vector3 (camStartPoint.x + 4, camStartPoint.y, camStartPoint.z);
			}

			if (thePlayer.theWeapon) {
				gunText.text = thePlayer.theWeapon.ammo.ToString ();
			} else {
				gunUICam.transform.position = camStartPoint;
				gunText.text = " ";
			}
		}
		// when player dies move back to start pos
		if (!thePlayer) {			
			gunText.text = " ";
			gunUICam.transform.position = camStartPoint;
			gunUICam = GameObject.FindGameObjectWithTag ("UICamera").GetComponent<Camera> ();
			StartCoroutine ("GetPlayer", 0.5f);
		}


	}

	// wait to get the player so there isnt a null
	IEnumerator GetPlayer(float wait){
		yield return new WaitForSeconds (wait);
		thePlayer = GameObject.FindGameObjectWithTag ("Player").GetComponent < Player> ();
	}

	public void GameOverUI(){
		gameOverScreen.SetActive (true);
		miniMap.SetActive (false);
		GameManager.Instance.isPaused = true;
		if (winCon) {
			winText.SetActive (true);
			lossText.SetActive (false);
		} else {
			lossText.SetActive (true);
			winText.SetActive (false);
		}
	}

	public void PauseUI(){
		if (GameManager.Instance.isPaused && !GameManager.Instance.gameOver) {			
			foreach (GameObject obj in children) {
				obj.SetActive (false);
			}
			pauseScreen.gameObject.SetActive (true);
			pauseScreen.enabled = true;
		} else {
			theCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
			// but if it is paused show the screen
			foreach (GameObject obj in children) {
				obj.SetActive (true);
			}
			gameOverScreen.SetActive (false);
			pauseScreen.gameObject.SetActive (false);
			pauseScreen.enabled = false;
			mainMenu.SetActive (false);
		}

		
	}

	// start game held in other scene
	public void ButtonBeginGame(){
		// go to main scene
		SceneManager.LoadScene("MainScene");
	}

	public void ButtonMainMenu(){
		GameManager.Instance.isPaused = false;
		SceneManager.LoadScene("MainMenu");
	}

	// end game
	public void ButtonQuit(){
		// turn off the game
		Application.Quit ();
		// if we are in the unity editor then this is equivalent to pressing the play button to stop the game
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#endif
	}

	//these take the value of the appropriate slider and convert to decibels
	public void AdjustMaster(){		
		audioMixer.SetFloat ("Master Volume", volumeVsDecibels.Evaluate(masterSlider.value));
	}

	public void AdjustMusic(){		
		audioMixer.SetFloat ("Music Volume", volumeVsDecibels.Evaluate(musicSlider.value));
	}

	public void AdjustSound(){		
		audioMixer.SetFloat ("Sound Volume", volumeVsDecibels.Evaluate(soundSlider.value));
	}

	// these are used in scene 1 to set the quality
	public void AdjustQuality(){
		QualitySettings.SetQualityLevel (GFXDrop.value);
	}

	public void AdjustRes(){
		Screen.SetResolution (resX[ResDrop.value], resY[ResDrop.value], true);
	}
}
