using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeightedObject
{
	[SerializeField, Tooltip("The object selected by this choice.")]
	public Object value;
	[SerializeField, Tooltip("The chance to select the value.")]
	public double chance = 1.0; 
}
