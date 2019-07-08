using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Pickup : MonoBehaviour {

	public enum PickupType{
		HEALTH,
		SPEED,
		ARMOR,
		ASSAULT,
		HANDGUN
	};

	public bool respawning = true;

	private Animator theAnim;

	private float healthAmount = 50;
	private float armorAmount = 250;

	public GameObject theAssault;
	public GameObject theHandgun;

	private MeshRenderer[] theRend;
	private Collider theColl;
	//Rigidbody rb = gameObject.GetComponent<Rigidbody> ();

	[SerializeField] private PickupType _pickupType;

	public PickupType pickupType { get{ return _pickupType;}}

	// Use this for initialization
	void Start () {
		theRend = gameObject.GetComponentsInChildren<MeshRenderer> ();
		theColl = gameObject.GetComponent<Collider> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other){
		// only player can pickup
		if (other.CompareTag ("Player")) {
			// speed pickup logic
			if (pickupType == PickupType.SPEED) {
				other.gameObject.GetComponent<Player> ().sprinting = true;
			}

			// health pickup logic 
			if (pickupType == PickupType.HEALTH) {
				other.gameObject.GetComponent<Health> ().health += healthAmount;
			}

			//armor pickup logic
			if (pickupType == PickupType.ARMOR) {
				other.gameObject.GetComponent<Health> ().armor += armorAmount;
			}
		}

		if (other.CompareTag ("Player") ){ //|| other.CompareTag ("AI")
			//rb.isKinematic = true;



			//rb.isKinematic = false;

			Player thePlayer = other.GetComponent<Player>();
		
			// assault pickup logic
			if (pickupType == PickupType.ASSAULT) {
				if (!thePlayer.hasAssault) { // || !other.GetComponent<AIWeaponHandler>().hasAssault
					//replace weapon TODO: make this into an inventory item
					//TODO: Make the AI and player both be able to switch/pickup weapons
					if (thePlayer.hasHandgun) { //|| !other.GetComponent<AIWeaponHandler>().hasHandgun
						thePlayer.hasHandgun = false;
						other.GetComponentInChildren<ProjectileWeapon> ().Replace ();
					}
					// SPAWN POINT IS THE LAST CHILD!!!! Consider changing the method of finding the spawnpoint
					Transform spawnPoint = other.transform.GetChild (other.transform.childCount - 1);
					GameObject gun = Instantiate (theAssault, spawnPoint.position, spawnPoint.transform.rotation);
					gun.transform.parent = spawnPoint;
					thePlayer.hasAssault = true;
					theAnim = other.GetComponent<Animator> ();
					theAnim.SetInteger (("WeaponType"), 1);
//					if (other.GetComponent<Player> ().theWeapon){						
//						other.GetComponent<Player> ().theWeapon.Replace ();
//					}
					thePlayer.theWeapon = gun.GetComponent<ProjectileWeapon> ();
				} else {
					thePlayer.theWeapon.ammo += 30;
				}
			}

			//handgun pickup logic
			if (pickupType == PickupType.HANDGUN) {
				if (!thePlayer.hasHandgun) { //|| !other.GetComponent<AIWeaponHandler>().hasHandgun
					//replace weapon TODO: make this into an inventory item
					//TODO: Make the AI and player both be able to switch/pickup weapons
					if (thePlayer.hasAssault){  //|| !other.GetComponent<AIWeaponHandler>().hasAssault
						thePlayer.hasAssault = false;
						other.GetComponentInChildren<ProjectileWeapon> ().Replace ();
					}
					Transform spawnPoint = other.transform.GetChild (other.transform.childCount - 1);
					GameObject gun = Instantiate (theHandgun, spawnPoint.position, spawnPoint.transform.rotation);				
					gun.transform.parent = spawnPoint;
					thePlayer.hasHandgun = true;
					theAnim = other.GetComponent<Animator> ();
					theAnim.SetInteger(("WeaponType"), 2);
//					if (other.GetComponent<Player> ().theWeapon){
//						other.GetComponent<Player> ().theWeapon.Replace ();
//					}
					thePlayer.theWeapon = gun.GetComponent<ProjectileWeapon> ();

				}
				else {
					thePlayer.theWeapon.ammo += 12;
				}
				//respawn logic


			}

			if (respawning) {
				foreach (MeshRenderer rend in theRend) {
					rend.enabled = false;
				}
				theColl.enabled = false;

				StartCoroutine ("Respawn", 10.0f);
			} else {
				Destroy (gameObject);
			}
		}
	}

	IEnumerator Respawn(float spawnTime){
		yield return new WaitForSeconds (spawnTime);
		theColl.enabled = true;
		foreach (MeshRenderer rend in theRend) {
			rend.enabled = true;
		}
	}
}
