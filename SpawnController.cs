using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour {

	private int spawnPointCount;
	public GameObject[] theSpawns;

	[SerializeField]
	private int maxToSpawn = 20;

	public int totalSpawned = 0;

	// Use this for initialization
	void Awake () {
		spawnPointCount = gameObject.transform.childCount;
		theSpawns = new GameObject[spawnPointCount];
//		for (int i = 0; i >= spawnPointCount; i++) {
//			GameObject currentChild = transform.GetChild (i).gameObject;
//			theSpawns [i] = currentChild;
//		}
			
	}

	void Start(){
		for (int i = 0; i < spawnPointCount; i++) {
			GameObject currentChild = transform.GetChild (i).gameObject;
			theSpawns [i] = currentChild;
		}
		foreach (GameObject spawn in theSpawns) {
			spawn.GetComponent<AISpawn>().InvokeRepeating ("SpawnEnemy", 0f, spawn.GetComponent<AISpawn>().spawnDelay);
		}
	}
	
	// Update is called once per frame
	void Update () {

		foreach (GameObject spawn in theSpawns) {
			if (totalSpawned >= maxToSpawn) {
				spawn.GetComponent<AISpawn>().CancelInvoke ("SpawnEnemy");
			}
		}

		}
}
