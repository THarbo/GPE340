using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawBox : MonoBehaviour {

	public Vector3 scale;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnDrawGizmos()
	{
		Gizmos.matrix = Matrix4x4.TRS (transform.position, transform.rotation, Vector3.one);
		Gizmos.color = Color.Lerp (Color.cyan, Color.clear, 0.5f);
		Gizmos.DrawCube (Vector3.up * scale.y / 2f, scale);
		Gizmos.color = Color.cyan;
		Gizmos.DrawRay (Vector3.zero, Vector3.forward * 0.4f);
	}

}
