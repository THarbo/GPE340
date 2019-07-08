using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine.UI;

[CustomPropertyDrawer(typeof(WeightedObjectDrawer))]
public class WeightedObjectDrawer : PropertyDrawer {

	public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label){
		EditorGUI.BeginProperty (pos, label, prop);
		pos = EditorGUI.PrefixLabel (pos, GUIUtility.GetControlID (FocusType.Passive), label);
		var indent = EditorGUI.indentLevel;
		EditorGUI.indentLevel = 0;

		var objectRect = new Rect (pos.x, pos.y, pos.width - 40f, pos.height);
		var chanceRect = new Rect (pos.x + pos.width - 40f, pos.y, 40f, pos.height);

		EditorGUI.PropertyField (objectRect, prop.FindPropertyRelative ("value"), GUIContent.none);
		EditorGUI.PropertyField (chanceRect, prop.FindPropertyRelative ("chance"), GUIContent.none);

		EditorGUI.indentLevel = indent;
		EditorGUI.EndProperty ();
	}
}
