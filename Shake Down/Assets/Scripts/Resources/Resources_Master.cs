using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Resources_Master
{
	private string _referenceID;
	public string referenceID { get { return _referenceID; } }

	public Resources_Master(string newID)
	{
		_referenceID = newID;
	}

	protected void DeathByLackOfResources(Resources_Master script)
	{
		Debug.LogError (script.referenceID + " is Dead.");
	}
}
