using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProjectileWeapon : Weapon {

	public Transform rightHandIKTarget;
	public Transform leftHandIKTarget;

	public enum WeaponType{
		HANDGUN,
		RIFLE
	}

	public GameObject parentObj;

	public bool hasFired = false;

	public WeaponType theType = new WeaponType ();
	// TODO make these interchangable
	public Player thePlayer;
	public AIWeaponHandler theAI;
	public WeaponAgent theAgent;
	public int ammo;
	private int maxAmmo;

	private AudioSource gunSource;
	public AudioClip handgunSound;
	public AudioClip rifleSound;

	public float refire = 0.2f;

	void Awake(){

		Scene currentScene = SceneManager.GetActiveScene ();
		int sceneIndex = currentScene.buildIndex;
		if (sceneIndex == 0){			
			return;
		}
		gunSource = GameObject.Find ("SoundSource").GetComponent<AudioSource>();
	}

	// Use this for initialization
	void Start () {
		if (theType == WeaponType.HANDGUN) {
			ammo = 12;
			maxAmmo = 120;
		}
		if (theType == WeaponType.RIFLE) {
			ammo = 30;
			maxAmmo = 150;
		}

		// this is a messy way to do this, ask for the animator and get the gameobject, need another way
		if (gameObject.GetComponentInParent<Animator>().gameObject.CompareTag("AI")){
			parentObj = GetComponentInParent<Animator> ().gameObject;
			theAgent = (WeaponAgent)parentObj.GetComponent<AIWeaponHandler> ();
			return;
		}

		if (gameObject.GetComponentInParent<Animator>().gameObject.CompareTag("Player")){
			parentObj = GetComponentInParent<Animator> ().gameObject;
			theAgent = (WeaponAgent)parentObj.GetComponent<Player> ();
			return;
		}


	}
	
	// Update is called once per frame
	void Update () {
		if (ammo > maxAmmo){
			ammo = maxAmmo;
		}
	}

	// logic for when the player tells the gun that the trigger is pulled
	public void PullTrigger(){
		if (ammo <=0 ){
			return;
		}
		// see comment above about messiness, ALSO need to NOT hard code the bullet spawn position and make it dynamic with the gun type
		if (theAgent) {//gameObject.GetComponentInParent<Animator> ().gameObject.CompareTag ("Player")
			if (theType == WeaponType.HANDGUN && hasFired == false) {
				GameObject shot = Instantiate (theAgent.theBullet, theAgent.theBarrel.transform.position + (theAgent.theBarrel.transform.forward * 0.4499f) + (theAgent.theBarrel.transform.up * 0.0084f), transform.rotation);
				shot.GetComponent<Rigidbody> ().AddForce (theAgent.theBarrel.transform.forward * theAgent.shootForce, ForceMode.Impulse);
				Debug.Log ("shooting");
				hasFired = true;
				GameManager.Instance.soundSource.pitch = 2;
				GameManager.Instance.soundSource.PlayOneShot (GameManager.Instance.handgunSound);
//				gunSource.clip = handgunSound;
//				gunSource.Play ();
				if (theAgent.CompareTag("Player")){
					ammo--;
				}

			}
			if (theType == WeaponType.RIFLE && refire == 0.2f) {
				GameObject shot = Instantiate (theAgent.theBullet, theAgent.theBarrel.transform.position + (theAgent.theBarrel.transform.forward * 0.4499f) + (theAgent.theBarrel.transform.up * 0.0084f), transform.rotation);
				shot.GetComponent<Rigidbody> ().AddForce (theAgent.theBarrel.transform.forward * theAgent.shootForce, ForceMode.Impulse);
				Debug.Log ("shooting");
				if (theAgent.CompareTag("Player")){
					ammo--;
				}
				GameManager.Instance.soundSource.pitch = 1;
				GameManager.Instance.soundSource.PlayOneShot(GameManager.Instance.rifleSound);
//				gunSource.clip = rifleSound;
//				gunSource.Play ();
			}
		}


//		if (gameObject.GetComponentInParent<Animator> ().gameObject.CompareTag ("AI")) {
//			if (theType == WeaponType.HANDGUN && hasFired == false) {
//				GameObject shot = Instantiate (theAI.theBullet, theAI.theBarrel.transform.position + (theAI.theBarrel.transform.forward * 0.4499f) + (theAI.theBarrel.transform.up * 0.0084f), transform.rotation);
//				shot.GetComponent<Rigidbody> ().AddForce (theAI.theBarrel.transform.forward * theAI.shootForce, ForceMode.Impulse);
//				Debug.Log ("shooting");
//				hasFired = true;
//			}
//			if (theType == WeaponType.RIFLE && refire == 0.2f) {
//				GameObject shot = Instantiate (theAI.theBullet, theAI.theBarrel.transform.position + (theAI.theBarrel.transform.forward * 0.4499f) + (theAI.theBarrel.transform.up * 0.0084f), transform.rotation);
//				shot.GetComponent<Rigidbody> ().AddForce (theAI.theBarrel.transform.forward * theAI.shootForce, ForceMode.Impulse);
//				Debug.Log ("shooting");
//
//			}
//		}

	}

	// makes it so the player switches based on pickup
	// TODO make this work as an inventory system where the player can change weapons after picking them up
	public void Replace(){	
		if (gameObject.CompareTag("Player")){	
			if (theType == WeaponType.HANDGUN){
				thePlayer.hasHandgun = false;
			}
			if (theType == WeaponType.RIFLE){
				thePlayer.hasAssault = false;
			}
		}
		Destroy (this.gameObject);
	}
}
