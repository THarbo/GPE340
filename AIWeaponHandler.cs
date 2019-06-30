using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWeaponHandler : MonoBehaviour {

	// create array of weapons, need to work to get amount of weapons @ runtime and populate
	public GameObject[] weaponToSpawn = new GameObject[2];

	// weapon logic TODO: add this into a weaponagent and inherit to this script
	public bool hasAssault = false;
	public bool hasHandgun = false;
	public ProjectileWeapon theWeapon;
	public GameObject theBullet;
	public GameObject theBarrel;
	public float shootForce = 500;
	private float minShootForce = 50;

	// changes to shooting logic so that the enemy doesnt machinegun the player to death instantly
	private float timeToShoot = 3.0f;
	private float maxTime = 3.0f;

	public bool hasShot = false;

	// get nav component and use its info for targeting
	private AINav theNav;

	// testing to see if theNav.thePlayer will NOT work
	public GameObject thePlayer;

	// range logic, TODO: add this to the weapon script per lessons
	public float maxRange;


	// Use this for initialization
	void Awake () {
		thePlayer = GameObject.FindGameObjectWithTag ("Player");
		theNav = GetComponent<AINav> ();
		// weapon spawn logic TODO: create better way of getting the spawn point in order to avoid confusing developers if they rearrange children
		Transform spawnPoint = transform.GetChild (transform.childCount - 1);
		GameObject randomWeapon = Instantiate (weaponToSpawn [Random.Range (0, 2)], spawnPoint.position, spawnPoint.rotation);
		theWeapon = randomWeapon.GetComponent<ProjectileWeapon>();
		randomWeapon.transform.parent = spawnPoint;
	}
	
	// Update is called once per frame
	void Update () {	

		// get the position of the player to look at only while in line of sight
		Debug.DrawRay (theBarrel.transform.position, (thePlayer.transform.position - theBarrel.transform.position) + thePlayer.transform.up);
		RaycastHit hit;
		// if there is a collision between the screen and the direction of the ray...
		if (Physics.Raycast (theBarrel.transform.position,(thePlayer.transform.position - theBarrel.transform.position) + thePlayer.transform.up, out hit)){
			theBarrel.transform.LookAt (hit.point);
			if(hit.collider.gameObject.CompareTag("Player")){				
				// shooting logic
				if (Vector3.Distance(transform.position, thePlayer.transform.position) <= maxRange && timeToShoot == 3.0f){
					theWeapon.PullTrigger();
					hasShot = true;
				}

				if (hasShot == true){
					timeToShoot -= Time.deltaTime;
				}

				if (timeToShoot <=0){
					timeToShoot = maxTime;
				}
			}
		}
	}
}
