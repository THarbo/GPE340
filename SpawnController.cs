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

	public int maxToSpawn = 20;
	public int totalSpawned = 0;

	public Transform spawnPoint;

	public GameObject thePlayer;
	public GameObject playerPrefab;

	public bool initialSpawn = false;

	// Use this for initialization
	void Awake () {
		// init the ints for the loops
		spawnPointCount = gameObject.transform.childCount;
		playerSpawnCount = spawnPointCount; 

		// loop to get the number of each spawn
		for (int i = 0; i <= spawnPointCount - playerSpawnCount; i++) {
			GameObject currentChild = transform.GetChild (i).gameObject;
			// check if it is an AI spawn
			if (currentChild.CompareTag("Spawn")){
				// if it is then remove it from the player spawn count
				playerSpawnCount--;
			}
		}
		// init the arrays with the proper counts
		playerSpawns = new GameObject[playerSpawnCount];
		aiSpawns = new GameObject[spawnPointCount - playerSpawnCount];			
	}

	void Start(){

		//TODO: MAKE THIS NOT BE CHILD POSITION DEPENDENT, IT DOESNT WORK IF THE CHILDREN ARE REORDERED 
		// all player spawns need to be adjacent, and at end of child list

		// loop through the ai spawn list and init each index
		for (int i = 0; i < spawnPointCount - playerSpawnCount; i++) {
			GameObject currentChild = transform.GetChild (i).gameObject;
			aiSpawns [i] = currentChild;
		}
		// then start the spawning
		foreach (GameObject spawn in aiSpawns) {			
			spawn.GetComponent<AISpawn>().InvokeRepeating ("SpawnEnemy", 0f, spawn.GetComponent<AISpawn>().spawnDelay);
		}

		// loop through player spawns
		for (int i = spawnPointCount - playerSpawnCount; i < spawnPointCount; i++) {
			GameObject currentChild = transform.GetChild (i).gameObject;
			playerSpawns [(i - spawnPointCount) + playerSpawnCount] = currentChild;
		}
		// spawn the player
		if (thePlayer == null){
			RespawnPlayer ();
		}
	}
	
	// Update is called once per frame
	void Update () {

		// check for gameover condition
		foreach (GameObject spawn in aiSpawns) {
			if (totalSpawned >= maxToSpawn) {
				spawn.GetComponent<AISpawn>().CancelInvoke ("SpawnEnemy");
			}
		}

		// if the player is null and this is not the first time they have spawned
		// redundant from start I think
		if (thePlayer == null && initialSpawn == true){ 
			RespawnPlayer ();				
		}
	}

	// spawn logic
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