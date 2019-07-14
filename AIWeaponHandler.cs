using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWeaponHandler : WeaponAgent {

	// create array of weapons, need to work to get amount of weapons @ runtime and populate
	public GameObject[] weaponToSpawn = new GameObject[2];

	// changes to shooting logic so that the enemy doesnt machinegun the player to death instantly
	public float timeToShoot = 3.0f;
	private float maxTime = 3.0f;

	public bool hasShot = true;

	// get nav component and use its info for targeting
	private AINav theNav;

	// range logic, TODO: add this to the weapon script per lessons
	public float maxRange;

	// Use this for initialization
	protected override void Awake () {	
		base.Awake ();	
		//thePlayer = GameObject.FindGameObjectWithTag ("Player");
		theNav = GetComponent<AINav> ();
		// weapon spawn logic TODO: create better way of getting the spawn point in order to avoid confusing developers if they rearrange children
		Transform spawnPoint = transform.GetChild (transform.childCount - 1);
		GameObject randomWeapon = Instantiate (weaponToSpawn [Random.Range (0, 2)], spawnPoint.position, spawnPoint.rotation);
		theWeapon = randomWeapon.GetComponent<ProjectileWeapon>();
		randomWeapon.transform.parent = spawnPoint;
		if (theWeapon.theType == ProjectileWeapon.WeaponType.HANDGUN) {
			hasHandgun = true;
			theAnim.SetInteger (("WeaponType"), 1);
		} else {
			theAnim.SetInteger (("WeaponType"), 2);
			hasAssault = true;
		}
	}
	
	// Update is called once per frame
	void Update () {	

		if(theNav.thePlayer == null){
			return;
		}

		// get the position of the player to look at only while in line of sight
		Debug.DrawRay (theBarrel.transform.position, (theNav.thePlayer.transform.position - theBarrel.transform.position) + theNav.thePlayer.transform.up);
		RaycastHit hit;
		if (Physics.Raycast (theBarrel.transform.position, (theNav.thePlayer.transform.position - theBarrel.transform.position) + theNav.thePlayer.transform.up, out hit)) {
			
			theBarrel.transform.LookAt (hit.point);
			if (hit.collider.gameObject.CompareTag ("Player")) {

				Vector3 targetPoint = new Vector3 (hit.point.x, transform.position.y, hit.point.z) - transform.position;
				Quaternion targetRot = Quaternion.LookRotation(targetPoint, Vector3.up);
				transform.rotation = Quaternion.Slerp (transform.rotation, targetRot, Time.deltaTime * 4.0f);

				// shooting logic
				if (Vector3.Distance (transform.position, theNav.thePlayer.transform.position) <= maxRange && timeToShoot == 3.0f) {
					theWeapon.PullTrigger ();
					hasShot = true;
				}				
			}
			if (hasShot == true) {
				timeToShoot -= Time.deltaTime;
			}

			if (timeToShoot <= 0) {					
				hasShot = false;
				theWeapon.hasFired = false;
				timeToShoot = maxTime;

			}
		}
	}
}

