using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISpawn : MonoBehaviour {

	public GameObject theEnemy;

	[SerializeField]
	private int maxEnemies = 5;
	[SerializeField]
	public int currentEnemies = 0;
	public float spawnDelay;
	private SpawnController spawnController;


	void Awake(){
		spawnController = GetComponentInParent<SpawnController> ();
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void SpawnEnemy(){
		if (currentEnemies >= maxEnemies){
			return;
		}
		GameObject spawnedEnemy = Instantiate (theEnemy, transform.position, transform.rotation);
		currentEnemies++;
		spawnController.totalSpawned++;
		spawnedEnemy.GetComponent<Health> ().theSpawn = this;
		//spawnedEnemy.GetComponent<Health> ().onDie.AddListener (HandleEnemyDeath);
	}

	private void HandleEnemyDeath(){
		currentEnemies--;
	}
}
