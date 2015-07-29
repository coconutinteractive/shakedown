using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Building_Script : MonoBehaviour
{
	static private GameObject dialoguePanelReference;
	[SerializeField] private string buildingID;
	static private Dictionary<string, Building_Script> _buildings = new Dictionary<string, Building_Script>();
	static public Building_Script GetBuilding(string buildingID) { return _buildings[buildingID]; }
	static private List<string> _buildingIDs = new List<string>();
	static public List<string> buildingIDs { get { return _buildingIDs; } }

	private bool _buildingRobbed;
	public bool buildingRobbed { get { return _buildingRobbed; } }
	private bool _buildingVandalized;
	public bool buildingVandalized { get { return _buildingVandalized; } }

	private Resources_Shopkeeper _shopkeeper;
	public Resources_Shopkeeper shopkeeper { get { return _shopkeeper; } }

	void Start()
	{
		if(!registerBuilding())
			Debug.LogError ("Hey. Yo, buddy. There's a duplicate in the Building IDs. You're gonna want to change one of the " + buildingID + "'s to something else. -Scott");
	}

	private bool registerBuilding()
	{
		if(buildingIDs.Contains (buildingID)) {
			return false;
		} else { 
			buildingIDs.Add (buildingID);
			_buildings.Add (buildingID, this.GetComponent<Building_Script>());
			return true;
		}
	}

	public void RegisterShopkeeper(Resources_Shopkeeper shopkeeper)
	{
		_shopkeeper = shopkeeper;
	}

	public void enterBuilding()
	{

	}

	public void vandalizeBuilding()
	{
		//TODO: add reference to vandal
		_buildingVandalized = true;
	}

	public void robBuilding()
	{
		//TODO: add reference to robber
		_buildingRobbed = true;
	}
}