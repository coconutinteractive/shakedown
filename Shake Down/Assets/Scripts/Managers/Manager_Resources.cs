using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Manager_Resources : MonoBehaviour 
{
	// Master Dictionary of Resource instances
	static private Dictionary<string, Resources_Master> referenceIDs = new Dictionary<string, Resources_Master>();

	public static void ClearAllResources()
	{
		referenceIDs = new Dictionary<string, Resources_Master> ();
		_shopkeepers = new Dictionary<string, Resources_Shopkeeper> ();
		_officers = new Dictionary<string, Resources_Officer> ();
		_player = null;
	}

	// Player
	[SerializeField] static private Resources_Player _player;
	static public Resources_Player player { get { return _player; } }
	static public void NewPlayer (Resources_Player value)
	{
		if (IDIsUnique (value.referenceID)) {
			_player = value; 
			referenceIDs.Add (value.referenceID, value);
		} else {
			Debug.LogError ("Attempted to create a player when a player already exists"); 
		}
	}

	// Shopkeepers
	[SerializeField] static private Dictionary<string, Resources_Shopkeeper> _shopkeepers = new Dictionary<string, Resources_Shopkeeper>();
	static public Dictionary<string, Resources_Shopkeeper> shopkeepers { get { return _shopkeepers; } }
	static public void NewShopkeeper (Resources_Shopkeeper value) 
	{
		if (IDIsUnique (value.referenceID)) {
			_shopkeepers.Add (value.referenceID, value);
			referenceIDs.Add (value.referenceID, value);
		} else {
			Debug.LogError ("Attempted to create a shopkeeper with the existing ID '" + value.referenceID + "'.");
		}
	} 

	// Officers
	[SerializeField] static private Dictionary<string, Resources_Officer> _officers = new Dictionary<string, Resources_Officer>();
	static public Dictionary<string, Resources_Officer> officers { get { return _officers; } }
	static public void NewOfficer (Resources_Officer value)
	{
		if (IDIsUnique (value.referenceID)) {
			_officers.Add (value.referenceID, value);
			referenceIDs.Add (value.referenceID, value);
		} else {
			Debug.LogError ("Attempted to create an officer with the existing ID '" + value.referenceID + "'.");
		}
	}

	static private bool IDIsUnique(string value)
	{	if (referenceIDs.ContainsKey (value)) 
		{	return false;
		}	return true;
	}

}