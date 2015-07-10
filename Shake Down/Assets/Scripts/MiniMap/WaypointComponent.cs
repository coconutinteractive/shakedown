using UnityEngine;
using System.Collections;

public class WaypointComponent : MonoBehaviour 
{
	[SerializeField] private GameObject waypointPrefab = null;
	public static GameObject currentWaypoint;

	public void CreateWaypoint()
	{
		Destroy (currentWaypoint);
		currentWaypoint = GameObject.Instantiate (waypointPrefab, transform.position + transform.up, transform.rotation) as GameObject;
		GameObject.FindGameObjectWithTag ("Help Arrow").GetComponent<HelpArrow> ().Activate (currentWaypoint);
	}

	public void DestroyWaypoint()
	{
		Destroy (currentWaypoint);
		GameObject.FindGameObjectWithTag ("Help Arrow").GetComponent<HelpArrow> ().Deactivate ();
	}

}
