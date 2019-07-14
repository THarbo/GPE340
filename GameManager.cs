using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.Events;

public class GameManager : MonoBehaviour {

	public static GameManager Instance { get; private set;}

	public GameObject thePlayer;
	public UIHandler uiHandler;

	public UnityEvent gameOverEvent;
	public UnityEvent pauseEvent;

	[SerializeField]
	private int startingLives = 3;
	[SerializeField]
	private int livesLeft;
	[SerializeField]
	private int scoreBonusLife = 1;
	[SerializeField]
	private float currentScore = 0;
	[SerializeField]
	private int bonusScoreRequired = 100;
	[SerializeField]
	private float bonusScoreTracker = 0;
	[SerializeField]
	public int scorePerKill { get; private set;}

	//audio
	public AudioSource musicSource;
	public AudioSource soundSource;
	public AudioClip handgunSound;
	public AudioClip rifleSound;
	public AudioClip missSound;
	public AudioClip hitSound;

	// pickup cleanup vars
	private List<GameObject> levelPickups;
	private List<Vector3> pickupPos = new List<Vector3>();
	private List<Quaternion> pickupRot = new List<Quaternion>();
	private List<int> pickupTypes = new List<int>();

	// ui stuff, should probably go to ui handler
	public Text livesInt;
	public Text scoreFloat;

	// game flow
	public bool isPaused = false;
	public bool gameOver = false;
	public bool gameStart = true;
	//static private bool _isPaused;

	public List<Health> healthObjects = new List<Health>();


	public SpawnController spawnController;
	public int currentKilled = 0;

	// pickup vars
	public GameObject healthPick;
	public GameObject speedPick;
	public GameObject armorPick;
	public GameObject assaultPick;
	public GameObject handgunPick;
	 

	public void Awake(){
		Instance = this;
		scorePerKill = 10;
		musicSource = GameObject.Find ("Audio Source").GetComponent<AudioSource> ();


	}

	// Use this for initialization
	void Start () {
		spawnController = GameObject.Find ("Spawner").GetComponent<SpawnController> ();
		//source.Play ();

		StartCoroutine (LateStart (1f));
		livesLeft = startingLives;

		foreach(GameObject pickup in GameObject.FindGameObjectsWithTag("Pickup")){
			//levelPickups.Add (pickup);
			pickupPos.Add (pickup.transform.position);
			pickupRot.Add (pickup.transform.rotation);
			if (pickup.GetComponent<Pickup>().pickupType == Pickup.PickupType.HEALTH){
				pickupTypes.Add(0);
			}
			if (pickup.GetComponent<Pickup>().pickupType == Pickup.PickupType.SPEED){
				pickupTypes.Add(1);
			}
			if (pickup.GetComponent<Pickup>().pickupType == Pickup.PickupType.ARMOR){
				pickupTypes.Add(2);
			}
			if (pickup.GetComponent<Pickup>().pickupType == Pickup.PickupType.ASSAULT){
				pickupTypes.Add(3);
			}
			if (pickup.GetComponent<Pickup>().pickupType == Pickup.PickupType.HANDGUN){
				pickupTypes.Add(4);
			}
		}
	}


	
	// Update is called once per frame
	void Update () {

		if (thePlayer && livesLeft <= 0){
			EndGame ();
		}

		if (currentKilled >= spawnController.maxToSpawn){
			EndGame();
			uiHandler.winCon = true;
		}

		// if you gain enough score to get a life, iterate the lives positively
		if (bonusScoreTracker >= bonusScoreRequired){
			IterateLives (scoreBonusLife);
			bonusScoreTracker = 0;
		}

		// button to toggle pause manually
		if (Input.GetKeyDown(KeyCode.P)){
			TogglePause ();
		}

		// turns down the timescale
		if (isPaused){
			Time.timeScale = 0f;
			} else {
			Time.timeScale = 1.0f;
			}

		livesInt.text = livesLeft.ToString();
		scoreFloat.text = currentScore.ToString ();
	}

	public void IterateScore (float score){
		currentScore += score;
		bonusScoreTracker += score;
	}

	public void IterateLives(int iteration){
		livesLeft += iteration;
	}

	IEnumerator LateStart(float wait){
		yield return new WaitForSeconds (wait);
		thePlayer = spawnController.thePlayer;

	}

	public void ResetBonusScore(){
		bonusScoreTracker = 0;
	}

	// this brings up the pause menu
	public void TogglePause(){
		
		isPaused = !isPaused;
		pauseEvent.Invoke ();
	}

	public void RestartGame(){
		
		if (thePlayer){
			Destroy (thePlayer);
		}

		livesLeft = startingLives;
		spawnController.totalSpawned = 0;
		// if there are any AI enemies then destroy them
		foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("AI")){
			Destroy (enemy);
		}

		// destroy all the pickups
		foreach(GameObject pickup in GameObject.FindGameObjectsWithTag("Pickup")){
			Destroy (pickup);
		}

		// set all AI spawn spawncounts to 0
		foreach(GameObject spawn in spawnController.aiSpawns){
			spawn.GetComponent<AISpawn> ().currentEnemies = 0;
		}

		// loop through the pickup counts and instantiate correct pickups
		for(int i = 0; i < pickupPos.Count; i++){
			GameObject thePickupType;
			if (pickupTypes[i] == 0){
				thePickupType = Instantiate (healthPick, pickupPos [i], pickupRot [i]);
			}
			if (pickupTypes[i] == 1){
				thePickupType = Instantiate (speedPick, pickupPos [i], pickupRot [i]);
			}
			if (pickupTypes[i] == 2){
				thePickupType = Instantiate (armorPick, pickupPos [i], pickupRot [i]);
			}
			if (pickupTypes[i] == 3){
				thePickupType = Instantiate (assaultPick, pickupPos [i], pickupRot [i]);
			}
			if (pickupTypes[i] == 4){
				thePickupType = Instantiate (handgunPick, pickupPos [i], pickupRot [i]);
			}
		}
		isPaused = false;
		gameOver = false;
		//spawnController.RespawnPlayer ();
		//TogglePause ();
	}

	public void EndGame(){		
		gameOverEvent.Invoke ();
		gameOver = true;
		isPaused = true;
	}

}
