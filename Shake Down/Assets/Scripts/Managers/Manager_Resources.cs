using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Manager_Resources : MonoBehaviour
{
	static private List<Resources_Root> _referenceIDs						= new List<Resources_Root>();
	[SerializeField] static private Resources_Player _player				= null;
	[SerializeField] static private List<Resources_Shopkeeper> _shopkeepers	= new List<Resources_Shopkeeper>();
	[SerializeField] static private List<Resources_Officer> _officers 		= new List<Resources_Officer>();
	[SerializeField] static private List<Resources_Building> _buildings		= new List<Resources_Building>();

	static public List<Resources_Root> referenceIDs			{ get { return _referenceIDs; } }
	static public Resources_Player player					{ get { return _player; } }
	static public List<Resources_Shopkeeper> shopkeepers	{ get { return _shopkeepers; } }
	static public List<Resources_Officer> officers			{ get { return _officers; } }
	static public List<Resources_Building> buildings		{ get { return _buildings; } }

	static public void NewPlayer (Resources_Player value)
	{
		if (_player == null) {
			_player = value; 
			_referenceIDs.Add (value);
		} else {
			Debug.LogError ("Attempted to create a player when a player already exists"); }
	}

	#region New Resource
	static public void NewShopkeeper (Resources_Shopkeeper value) 
	{
		if (IDIsUnique (value.id)) {
			_shopkeepers.Add (value);
			_referenceIDs.Add (value);
		} else {
			Debug.LogError ("Attempted to create a shopkeeper with the existing ID '" + value.id + "'."); }
	} 
	
	static public void NewOfficer (Resources_Officer value)
	{
		if (IDIsUnique (value.id)) {
			_officers.Add (value);
			_referenceIDs.Add (value);
		} else {
			Debug.LogError ("Attempted to create an officer with the existing ID '" + value.id + "'."); }
	}

	static public void NewBuilding (Resources_Building value)
	{
		if (IDIsUnique (value.id)) {
			_buildings.Add (value);
			_referenceIDs.Add (value);
		} else {
			Debug.LogError ("Attempted to create a building with the existing ID '" + value.id + "'."); }
	}
	#endregion

	#region Get Resource By ID
	static public Resources_Shopkeeper GetShopkeeperByID(string id)
	{
		for (int i = 0; i < shopkeepers.Count; i++) {
			if (shopkeepers[i].id == id) {
				return shopkeepers[i]; } }
		Debug.LogWarning ("Uh oh. I couldn't find a shopkeeper named '" + id + "' anywhere. o_o -Scott");
		return null;
	}

	static public Resources_Officer GetOfficerByID(string id)
	{
		for (int i = 0; i < officers.Count; i++) {
			if (officers[i].id == id) {
				return officers[i]; } }
		Debug.LogWarning ("Uh oh. I couldn't find an officer named '" + id + "' anywhere. o_o -Scott");
		return null;
	}

	static public Resources_Building GetBuildingByID(string id)
	{
		for (int i = 0; i < buildings.Count; i++) {
			if (buildings[i].id == id) {
				return buildings[i]; } }
		Debug.LogWarning ("Uh oh. I couldn't find a building named '" + id + "' anywhere. o_o -Scott");
		return null;
	}
	#endregion

	static protected bool IDIsUnique(string value)
	{	
		for(int i = 0; i < _referenceIDs.Count; i++) {
			if(_referenceIDs[i].id == value) {
				return false; } }
		return true;
	}

	static public void ClearAllResources ()
	{
		_referenceIDs = new List<Resources_Root>();
		_player = null;
		_shopkeepers = new List<Resources_Shopkeeper>();
		_officers = new List<Resources_Officer>();
		_buildings = new List<Resources_Building>();
	}
}