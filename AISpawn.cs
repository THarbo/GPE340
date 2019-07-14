using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.Events;

public class AISpawn : MonoBehaviour {

	public GameObject theEnemy;
	public GameManager gameManager;

	[SerializeField]
	private float maxEnemies = 5;
	[SerializeField]
	public int currentEnemies = 0;
	public float spawnDelay;
	private SpawnController spawnController;
	private float modifier;
	public Slider spawnSlider;
	//public UnityEvent _onSliderChange
	 


	void Awake(){
		gameManager = FindObjectOfType<GameManager> ();
		spawnController = GetComponentInParent<SpawnController> ();
		// setting modifier here, but it would be dynamic based on enemy type 
		modifier = 1;

	}
	// Use this for initialization
	void Start () {
		spawnSlider.onValueChanged.AddListener (delegate {ChangeMaxEnemies ();});
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void SpawnEnemy(){
		if (currentEnemies >= maxEnemies){
			return;
		}
		GameObject spawnedEnemy = Instantiate (theEnemy, transform.position, transform.rotation);
		Health spawnedHealth = spawnedEnemy.GetComponent<Health> ();
		currentEnemies++;
		spawnController.totalSpawned++;
		spawnedHealth.theSpawn = this;
		gameManager.healthObjects.Add (spawnedHealth);
		spawnedEnemy.GetComponent<Health> ().onDie.AddListener (HandleEnemyDeath);
	}


		
	// makes sure the spawners know they can spawn another, also give score
	public void HandleEnemyDeath(){
		currentEnemies--;
		gameManager.IterateScore (GameManager.Instance.scorePerKill * modifier);
		GameManager.Instance.currentKilled++;
	}

	// adjusts max to spawn (used by the slider in pause menu)
	public void ChangeMaxEnemies(){
		maxEnemies = spawnSlider.value;
	}
}
