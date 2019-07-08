using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour {

	public int spawnPointCount;
	// these are the AI spawns
	public GameObject[] aiSpawns;
	public int playerSpawnCount;
	// these are only player spawns
	public GameObject[] playerSpawns;

	[SerializeField]
	private int maxToSpawn = 20;
	[SerializeField]
	public int totalSpawned = 0;

	public Transform spawnPoint;


	public GameObject thePlayer;
	public GameObject playerPrefab;

	public bool initialSpawn = false;

	// Use this for initialization
	void Awake () {
		//thePlayer = GameObject.FindGameObjectWithTag ("Player");
		spawnPointCount = gameObject.transform.childCount;

		playerSpawnCount = spawnPointCount; 

		// loop to get the number of each spawn
		for (int i = 0; i <= spawnPointCount - playerSpawnCount; i++) {
			GameObject currentChild = transform.GetChild (i).gameObject;
			// check if it is an AI spawn
			if (currentChild.CompareTag("Spawn")){
				playerSpawnCount--;
			}
		}
		//spawnPointCount = spawnPointCount - playerSpawnCount;
		playerSpawns = new GameObject[playerSpawnCount];
		aiSpawns = new GameObject[spawnPointCount - playerSpawnCount];


			
	}

	void Start(){
		for (int i = 0; i < spawnPointCount - playerSpawnCount; i++) {
			GameObject currentChild = transform.GetChild (i).gameObject;
			aiSpawns [i] = currentChild;
		}
		foreach (GameObject spawn in aiSpawns) {			
			spawn.GetComponent<AISpawn>().InvokeRepeating ("SpawnEnemy", 0f, spawn.GetComponent<AISpawn>().spawnDelay);
		}

		for (int i = spawnPointCount - playerSpawnCount; i < spawnPointCount; i++) {
			GameObject currentChild = transform.GetChild (i).gameObject;
			playerSpawns [(i - spawnPointCount) + playerSpawnCount] = currentChild;
		}

		if (thePlayer == null){
			RespawnPlayer ();
		}

	}
	
	// Update is called once per frame
	void Update () {

		foreach (GameObject spawn in aiSpawns) {
			if (totalSpawned >= maxToSpawn) {
				spawn.GetComponent<AISpawn>().CancelInvoke ("SpawnEnemy");
			}
		}

		// if the player is null and this is not the first time they have spawned
		if (thePlayer == null && initialSpawn == true){ 
			RespawnPlayer ();
				//thePlayer = tempPlayer;
		}
	}

	public GameObject RespawnPlayer(){
		spawnPoint = playerSpawns[Random.Range (0, playerSpawns.Length)].GetComponent<Transform>();
		thePlayer = Instantiate (playerPrefab, spawnPoint.position, spawnPoint.rotation);
		if (initialSpawn == false){
			initialSpawn = true;
		}
		GameManager.Instance.thePlayer = thePlayer;
		return thePlayer.gameObject;
	}
}