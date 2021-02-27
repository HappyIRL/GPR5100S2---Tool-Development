using UnityEngine;
using UnityEditor;

public class SelectAllOfTag : ScriptableWizard
{
	public string searchTag = "Your tag here";

	[MenuItem("Tools/Select all of tag...")]
	static void CreateWizard()
	{
		ScriptableWizard.DisplayWizard<SelectAllOfTag>("Select all of tag...", "Make selection");
	}

	void OnWizardCreate()
	{
		GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(searchTag);
		Selection.objects = gameObjects;	
	}
}
