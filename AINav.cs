using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(Animator))]
public class AINav : MonoBehaviour {


	private Animator theAnim;

	private NavMeshAgent theAgent;

	public GameObject thePlayer;

	// Use this for initialization
	void Awake () {
		theAgent = GetComponent<NavMeshAgent> ();
		thePlayer = GameObject.FindGameObjectWithTag ("Player");
		theAnim = gameObject.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {

		Vector3 input = theAgent.desiredVelocity;
		input = transform.InverseTransformDirection (input);
		theAnim.SetFloat ("Horizontal", input.x);
		theAnim.SetFloat ("Vertical", input.z);

		if (thePlayer != null){
			theAgent.SetDestination (thePlayer.transform.position);
		}

		if (thePlayer == null){
			thePlayer = GameObject.FindGameObjectWithTag ("Player");
		}


	}

	private void OnAnimatiorMove(){
		theAgent.velocity = theAnim.velocity;
	}
}
