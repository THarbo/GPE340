﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class Health : MonoBehaviour {

	public UnityEvent onDie;

	public float health;
	public float expectedHealth;
	private float maxHealth = 500;

	public float armor;
	public float expectedArmor;
	private float maxArmor = 250;

	[SerializeField]
	private int deathPenalty;
	[SerializeField]
	private int scoreForKill;

	private bool isDead = false;

	public Image armorBar;
	public Image healthBar;
	public Text armorText;
	public Canvas theCanvas;

	public SpawnController spawnController;
	public AISpawn theSpawn;
	public RagdollHandler theRagdoll;

	private int vec2Second;

	public float spawnTimer = 5.0f;
	public float cleanUpTimer = 1.0f;

	// not happy with having all these gameobject variables for spawning
	// should move this to another script
	public GameObject handGunSpawn;
	public GameObject rifleSpawn;
	public bool hasDropped = false;

	private AIWeaponHandler aiWeaponHandler;
	private GameObject player;

	public WeightedObject[] itemDrops;

	void Awake(){	

		aiWeaponHandler = gameObject.GetComponent<AIWeaponHandler> ();
		if (gameObject.GetComponent<Player>()){
			player = gameObject;
			armorBar = GameObject.Find ("ArmorBar").GetComponent<Image>();
			healthBar = GameObject.Find ("HealthBar").GetComponent<Image>();
			armorText = GameObject.Find ("ArmorText").GetComponent<Text>();
			deathPenalty = -1;
		}
	}

	// TODO: fix hardcoding of bar width
	void Start () {	
		
		armor = 0;
		expectedArmor = 0;

		if (player) {
			health = maxHealth;
			expectedHealth = health;
			vec2Second = 48;
			GameObject[] theSpawns = GameObject.FindGameObjectsWithTag ("Spawn");
			healthBar.rectTransform.sizeDelta = new Vector2 (health, vec2Second);
			armorBar.rectTransform.sizeDelta = new Vector2 (armor, vec2Second);
			//theSpawn = theSpawns[Random.Range (0, theSpawns.Length)].GetComponent<AISpawn>();
		} else {
			health = 150;
			maxHealth = 150;
			expectedHealth = health;
			vec2Second = 100;
		}

		theRagdoll = gameObject.GetComponent<RagdollHandler> ();

		if (aiWeaponHandler) {			
			if (aiWeaponHandler.theWeapon.theType == ProjectileWeapon.WeaponType.HANDGUN) {				
				itemDrops [0].chance = 0.5;
				itemDrops [1].chance = 0;
			}
			if (aiWeaponHandler.theWeapon.theType == ProjectileWeapon.WeaponType.RIFLE) {
				itemDrops [0].chance = 0;
				itemDrops [1].chance = 0.5;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {


		// cap health to maxhealth
		if (health > maxHealth){
			health = maxHealth;
		}

		// this is kind of backwards but it  checks if our expected health differs from actual health,
		// if so it sets the expected health to be our health and adjusts the healthbar
		// its backwards so that it can accept healing
		if (expectedHealth != health) {
			expectedHealth = health;
			//if (gameObject.CompareTag ("Player") || gameObject.CompareTag("AI")) {
			healthBar.rectTransform.sizeDelta = new Vector2 (health, vec2Second);
			//}
		}

		// cap armor to maxarmor
		if (armor > maxArmor){
			armor = maxArmor;
		}

		// same check as above for expected health
		if (player) {
			if (expectedArmor != armor) {
				expectedArmor = armor;
				armorBar.rectTransform.sizeDelta = new Vector2 (armor, vec2Second);
			}
		}

		// TODO: update so that UI is not tied to the player script
		if (player) {
			if (armor > 0) {
				armorText.text = "Armor";
			} else {
				armorText.text = " ";
			}
		}

		if (health <= 0){
			if (isDead == false){
				onDie.Invoke ();
			}
			if (hasDropped == false){
				DropWeapon ();
			}

			cleanUpTimer -= Time.deltaTime;
			if (cleanUpTimer <= 0) {	
				GameManager.Instance.IterateLives (deathPenalty);
				Destroy (gameObject);
			}
		}				
	}

	// TODO: decouple the UI here so that it does not effect potential other player UI
	public void DealDamage (float damage){
		if (armor > 0) {
			armor -= damage;
			expectedArmor -= damage;
			// specifically here
			//if (gameObject.CompareTag ("Player")) {
			armorBar.rectTransform.sizeDelta = new Vector2 (armor, vec2Second);
			//}
		} else {
			health -= damage;
			expectedHealth -= damage;
			// and here
			//if (gameObject.CompareTag ("Player")) {
			healthBar.rectTransform.sizeDelta = new Vector2 (health, vec2Second);
			//}
		}
		GameManager.Instance.soundSource.PlayOneShot(GameManager.Instance.hitSound);
	}

	public void Die(){
			// ... toggle ragdoll
		theRagdoll.RagdollOn ();	
		isDead = true;
	}

	// TODO get this to work properly
//	public int Select(){
//		System.Random rnd = new System.Random();
//		int selectedIndex = System.Array.BinarySearch (itemDrops, rnd.NextDouble () * itemDrops.Length);
//
//	}

	public void DropWeapon(){
		// check if AI
		if (gameObject.CompareTag ("AI")) {
			// grab aiweaponhandler script 

			int weaponToDrop = 0;
			int itemToDrop;



			gameObject.GetComponent<AINav> ().enabled = false;
			// make sure aiweaponhandler is there ...
			if (aiWeaponHandler) {
				// ... and drop the gun, or health, or sprint
				// step 1 instantiate the right kind of weapon

				if (aiWeaponHandler.theWeapon.theType == ProjectileWeapon.WeaponType.HANDGUN) {					
					weaponToDrop = 0;
				}
				if (aiWeaponHandler.theWeapon.theType == ProjectileWeapon.WeaponType.RIFLE) {
					weaponToDrop = 1;
				}

				// these are the weights
				int dropGun = 50;
				int dropHealth = 85;
				// dont need dropSpeed since it is the higher bound
				//int dropSpeed = 100;

				int randCheck = Random.Range (0, 101);

				if (randCheck <= dropGun) {
					itemToDrop = weaponToDrop;
				} else if (randCheck > dropGun && randCheck <= dropHealth) {
					itemToDrop = 2;
				} else {
					itemToDrop = 3;
				}

				// instantiate the dropped object as a GameObject by accessing the value of itemDrops[index], which is an Object
				GameObject droppedObj = Instantiate ((GameObject)itemDrops[itemToDrop].value, transform.position + (transform.up * 0.25f), transform.rotation);

				// if it is a powerup make it small
				if (itemToDrop == 2 || itemToDrop == 3){
					droppedObj.transform.localScale = new Vector3 (0.2f, 0.2f, 0.2f);
				}

				// make sure the item does not respawn
				droppedObj.GetComponent<Pickup> ().respawning = false;


				// step two, destroy the weapon AI is holding
				aiWeaponHandler.theWeapon.Replace ();
				// disable aiWeaponHandler
				aiWeaponHandler.enabled = false;
				// toggle bool
				hasDropped = true;
			}
		} else {
			// if not AI, then you are probably a player
			// grab player script 
			Player player = gameObject.GetComponent<Player> ();
			// turn off mouse aim
			gameObject.GetComponent<AimAtMouse> ().enabled = false;
			// make sure player is there ...
			if (player && player.theWeapon){
				// ... and get rid of the gun
				// step 1 instantiate the right kind of weapon
				if(player.theWeapon.theType == ProjectileWeapon.WeaponType.HANDGUN){
					GameObject droppedWeapon = Instantiate (player.handgunSpawn, transform.position + (transform.up * 0.25f), transform.rotation);
				}
				if(player.theWeapon.theType == ProjectileWeapon.WeaponType.RIFLE){
					GameObject droppedWeapon = Instantiate (player.rifleSpawn, transform.position + (transform.up * 0.25f), transform.rotation);
				}
				// step two, destroy the weapon you are holding
				player.theWeapon.Replace ();
				// disable the player script
				player.enabled = false;
				// toggle bool
				hasDropped = true;
			}
		}
	}
}
