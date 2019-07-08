using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitAroundPoint : MonoBehaviour 
{
	public Transform orbitCenter;
	public float orbitSpeed = 45f;
	public Vector3 orbitAxis = Vector3.up;
	public Vector3 orbitOffset = new Vector3 (0, 8, -11);
	private float currentAngle = 0f;

	private void Update()
	{
		currentAngle += orbitSpeed * Time.deltaTime;
		Quaternion orbitRotation = Quaternion.AngleAxis (currentAngle, orbitAxis);
		orbitOffset = orbitRotation * orbitOffset;
		transform.position = orbitCenter.position + orbitOffset;
	}
}