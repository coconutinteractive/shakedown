using UnityEngine;
using System.Collections;

public class Scenario_Master
{
	public Scenario_Master()
	{
		if (Manager_Resources.player == null) {
			setup ();
		} else {
			Debug.LogError ("You can't open a scenario if one is already open.");
		}
	}

	virtual protected void setup() { }
}
