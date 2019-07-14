using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(Animator))]
public class AINav : MonoBehaviour {


	private Animator theAnim;
	private NavMeshAgent theAgent;
	public GameObject thePlayer;

	private float radius = 100f;
	public float timeToWalk = 0f;
	private float maxTime = 10f;

	// Use this for initialization
	void Awake () {
		theAgent = GetComponent<NavMeshAgent> ();
		thePlayer = GameObject.FindGameObjectWithTag ("Player");
		theAnim = gameObject.GetComponent<Animator> ();

	}
	
	// Update is called once per frame
	void Update () {
//		
		Vector3 input = theAgent.desiredVelocity;
		input = transform.InverseTransformDirection (input);

		// attempts at fixing the jitter of the AI
//		if (input.x <= 0.5f){
//			Debug.Log ("Stopping");
//			StartCoroutine ("WaitToMove", 1f);		
//			return;
//		}

		if (theAgent.isStopped){
			StartCoroutine ("WaitToMove", 3f);	
			return;
		}

		//ai inputs
		theAnim.SetFloat ("Horizontal", input.x);
		theAnim.SetFloat ("Vertical", input.z);
		theAnim.speed = 0.85f;


		Vector3 dir = Random.insideUnitSphere * radius;

		// destination logic
		RaycastHit hit;
		if (thePlayer != null){
			// if we see the player to go it
			if (Physics.Raycast (transform.position, (thePlayer.transform.position - transform.position) + thePlayer.transform.up, out hit)) {
				if (hit.collider.gameObject.CompareTag ("Player")) {
					theAgent.SetDestination (thePlayer.transform.position);
				} else{
					// otherwise pick a random destination and go to it every 10 seconds
					timeToWalk -= Time.deltaTime;

					if (timeToWalk <= 0 || theAgent.velocity == Vector3.zero) { // !hit.collider.gameObject.CompareTag ("Player")
						dir += transform.position;
						NavMeshHit navHit;
						if (NavMesh.SamplePosition (dir, out navHit, radius, 1)) {
							Vector3 desiredPos = navHit.position;
							theAgent.SetDestination (desiredPos);
							timeToWalk = maxTime;
						}
					}
				}			
			}
		}

		if (thePlayer == null){
			thePlayer = GameObject.FindGameObjectWithTag ("Player");
		}



	}

	private void OnAnimatiorMove(){
		theAgent.velocity = theAnim.velocity;
	}

	IEnumerator WaitToMove(float wait){
		yield return new WaitForSeconds (wait);

		if (thePlayer != null){
			//theAgent.SetDestination (thePlayer.transform.position);
		}

	}
}
