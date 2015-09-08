using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Scenario_Script
{
	static private bool scenarioExists = false;

	const int statMin = 1;
	const int statMax = 100;
	const int moneyMin = 0;
	const int moneyMax = 1999999999;
	
	static public void ClearScenario()
	{
		Manager_Resources.ClearAllResources();
		scenarioExists = false;
	}
	
	static public void SetupScenarioFromJSON(string scenarioID, JSONObject json, ProfileSettings profileSettings)
	{
		if(!scenarioExists)
		{
			JSONObject scenario = json[scenarioID];
			JSONObject settings_buildings = scenario.GetField ("building");
			JSONObject settings_player = scenario.GetField ("player");
			JSONObject settings_shopkeepers = scenario.GetField ("shopkeeper");
			JSONObject settings_officers = scenario.GetField ("officer");

			List<string> existingBuildingResources = new List<string>();
			existingBuildingResources = GenerateBuildingResources (scenarioID, settings_buildings);

			List<string> occupiedBuildingResources = new List<string>();
			occupiedBuildingResources	= GeneratePlayerResource (occupiedBuildingResources, scenarioID, settings_player, profileSettings);
			occupiedBuildingResources	= GenerateShopkeeperResources (occupiedBuildingResources, scenarioID, settings_shopkeepers);
			occupiedBuildingResources	= GenerateOfficerResources (occupiedBuildingResources, scenarioID, settings_officers);

			HarmonizeBuildingResourcesToDesignWorld(existingBuildingResources, occupiedBuildingResources);
			HarmonizePlayerResourcesToDesignWorld();
			HarmonizeOfficerResourcesToDesignWorld();
			scenarioExists = true; 
		} else {
			Debug.LogError ("You really shoulnd't be loading a new scenario when you've already got one there, buddy. Call 'ClearScenario()' first. -Scott");
		}
	}
	// 12 methods, 4 regions

	#region Generate Resources
	static protected List<string> GenerateBuildingResources(string scenarioID, JSONObject json)
	{
		List<string> existingBuildingResources = new List<string>();
		for (int i = 0; i < json.Count; i++)
		{
			JSONObject building = json[i];
			
			new Resources_Building(
				//id, name, image, type, money, income, expenses, rent, payment, day, inventory
				building.GetField ("id").str,
				building.GetField ("name").str,
				building.GetField ("image").str,
				Enums.BuildingTypeFromStatic(building.GetField ("type").str),
				Mathf.Clamp (int.Parse (building.GetField ("money").str), moneyMin, moneyMax),
			    Mathf.Clamp (int.Parse (building.GetField ("income").str), moneyMin, moneyMax),
			    Mathf.Clamp (int.Parse (building.GetField ("expenses").str), moneyMin, moneyMax),
			    Mathf.Clamp (int.Parse (building.GetField ("rent").str), moneyMin, moneyMax),
			    Mathf.Clamp (int.Parse (building.GetField ("payment").str), moneyMin, moneyMax),
				Enums.DayOfTheWeekFromStatic(building.GetField ("day").str),
				Resources_InventoryShop.GetShopInventoryByID(building.GetField ("inventory").str)
			);
			existingBuildingResources.Add (building.GetField("id").str);
		}
		return existingBuildingResources;
	}

	static protected List<string> GeneratePlayerResource(List<string> occupiedBuildingResources, string scenarioID, JSONObject json, ProfileSettings profileSettings)
	{
		for (int i = 0; i < json.Count; i++)
		{
			JSONObject player = json[i];

			new Resources_Player(
				// profile settings, home, money, income, expenses, strength, presence, opinion, inventory
				profileSettings,
				Manager_Resources.GetBuildingByID(player.GetField ("home").str),
				Mathf.Clamp (int.Parse (player.GetField ("money").str), moneyMin, moneyMax),
				Mathf.Clamp (int.Parse (player.GetField ("income").str), moneyMin, moneyMax),
				Mathf.Clamp (int.Parse (player.GetField ("expenses").str), moneyMin, moneyMax),
				Mathf.Clamp (int.Parse (player.GetField ("strength").str), statMin, statMax),
				Mathf.Clamp (int.Parse (player.GetField ("presence").str), statMin, statMax),
				Mathf.Clamp (int.Parse (player.GetField ("opinion").str), statMin, statMax),
				Resources_Inventory.GetInventoryByID(player.GetField ("inventory").str)
			);

			if(occupiedBuildingResources.Contains(player.GetField("home").str)) {
				Debug.LogError ("Easy, Nelly. Someone messed up. Go tell whoever was messing with the static data that the player's safehouse can't be in " + player.GetField("home").str + ". Someone else is already in there! -Scott");
			} else {
				occupiedBuildingResources.Add (player.GetField("home").str);
			}
		}
		return occupiedBuildingResources;
	}

	static protected List<string> GenerateShopkeeperResources(List<string> occupiedBuildingResources, string scenarioID, JSONObject json)
	{
		for (int i = 0; i < json.Count; i++)
		{
			JSONObject shopkeeper = json[i];

			Resources_Shopkeeper newShopkeeper = new Resources_Shopkeeper(
				//id, name, image, gender, home, money, income, expenses, 
				//strength, respect, fear, greed, integrity, stubbornness, personality, inventory
				shopkeeper.GetField ("id").str,
				shopkeeper.GetField ("name").str,
				shopkeeper.GetField ("image").str,
				Enums.GenderFromString(shopkeeper.GetField ("gender").str),
				Manager_Resources.GetBuildingByID(shopkeeper.GetField ("home").str),
				Mathf.Clamp (int.Parse (shopkeeper.GetField ("money").str), moneyMin, moneyMax),
				Mathf.Clamp (int.Parse (shopkeeper.GetField ("income").str), moneyMin, moneyMax),
				Mathf.Clamp (int.Parse (shopkeeper.GetField ("expenses").str), moneyMin, moneyMax),
				Mathf.Clamp (int.Parse (shopkeeper.GetField ("strength").str), statMin, statMax),
				Mathf.Clamp (int.Parse (shopkeeper.GetField ("respect").str), statMin, statMax),
				Mathf.Clamp (int.Parse (shopkeeper.GetField ("fear").str), statMin, statMax),
				Mathf.Clamp (int.Parse (shopkeeper.GetField ("greed").str), statMin, statMax),
				Mathf.Clamp (int.Parse (shopkeeper.GetField ("integrity").str), statMin, statMax),
				Mathf.Clamp (int.Parse (shopkeeper.GetField ("stubbornness").str), statMin, statMax),
				Enums.PersonalityFromStatic (shopkeeper.GetField ("personality").str),
				Resources_Inventory.GetInventoryByID(shopkeeper.GetField ("inventory").str)
			);

			if(occupiedBuildingResources.Contains(shopkeeper.GetField("home").str)) {
				Debug.LogError ("Whoa there, Sally. Someone done goofed. Go tell whoever was messing with the static data that " + shopkeeper.GetField("id").str + " can't set up shop in " + shopkeeper.GetField("home").str + ". Someone else is already in there! -Scott");
			} else {
				occupiedBuildingResources.Add (shopkeeper.GetField("home").str);
				// to be moved to a different section
				Building_Script buildingScriptRef = Building_Script.GetBuilding(shopkeeper.GetField("home").str);
				if(buildingScriptRef != null) {
					buildingScriptRef.shopkeeper = newShopkeeper;
				}
			}
		}
		return occupiedBuildingResources;
	}

	static protected List<string> GenerateOfficerResources(List<string> occupiedBuildingResources, string scenarioID, JSONObject json)
	{
		List<string> policeStations = new List<string>();
		for (int i = 0; i < json.Count; i++)
		{
			JSONObject officer = json[i];
			
			new Resources_Officer(
				//id, name, image, gender, home, money, income, expenses,
				//strength, respect, fear, greed, integrity, stubbornness, personality, inventory
				officer.GetField ("id").str,
				officer.GetField ("name").str,
				officer.GetField ("image").str,
				Enums.GenderFromString(officer.GetField ("gender").str),
				Manager_Resources.GetBuildingByID(officer.GetField ("home").str),
				Mathf.Clamp (int.Parse (officer.GetField ("money").str), moneyMin, moneyMax),
				Mathf.Clamp (int.Parse (officer.GetField ("income").str), moneyMin, moneyMax),
				Mathf.Clamp (int.Parse (officer.GetField ("expenses").str), moneyMin, moneyMax),
				Mathf.Clamp (int.Parse (officer.GetField ("strength").str), statMin, statMax),
				Mathf.Clamp (int.Parse (officer.GetField ("respect").str), statMin, statMax),
				Mathf.Clamp (int.Parse (officer.GetField ("fear").str), statMin, statMax),
				Mathf.Clamp (int.Parse (officer.GetField ("greed").str), statMin, statMax),
				Mathf.Clamp (int.Parse (officer.GetField ("integrity").str), statMin, statMax),
				Mathf.Clamp (int.Parse (officer.GetField ("stubbornness").str), statMin, statMax),
				Enums.PersonalityFromStatic (officer.GetField ("personality").str),
				Resources_Inventory.GetInventoryByID(officer.GetField ("inventory").str)
				);
			
			if(occupiedBuildingResources.Contains(officer.GetField("home").str)) {
				Debug.LogError ("Dial it back there, toots. " + officer.GetField ("home").str + " isn't a police station. Go tell whoever was doing the static data to send officer " + officer.GetField("id").str + " someplace else! -Scott");
			} else if (!policeStations.Contains(officer.GetField("home").str)) {
				policeStations.Add (officer.GetField("home").str);
			}
		}
		occupiedBuildingResources.AddRange (policeStations);
		return occupiedBuildingResources;
	}
	#endregion

	#region Harmonize Static Data To Design Land
	static protected void HarmonizeBuildingResourcesToDesignWorld(List<string> existing, List<string> occupied)
	{
		MakeSureAllExistingResourcesAreOccupiedBuildings(existing, occupied);
		MakeSureAllOccupiedBuildingsHaveExistingResources(existing, occupied);
		MakeSureAllExistingResourcesHaveDesignBuildings(existing);
		MakeSureAllDesignBuildingsHaveExistingResources(existing);
		SetReferencesBetweenResourcesAndDesignBuildings(existing);
	}

	static protected void MakeSureAllExistingResourcesAreOccupiedBuildings(List<string> existing, List<string> occupied)
	{
		for (int i = 0; i < existing.Count; i++) {
			string exist = existing[i];
			bool found = false;
			for (int j = 0; j < occupied.Count; j++) {
				string occupy = occupied[j];
				if(exist == occupy) {
					found = true;
					break;
				} 
			}
			if (!found) {
				Debug.LogError ("BUILDING HARMONY ISSUE: The Building Resource '" + exist + "' is declared in data but is unoccupied.");
			}
		}
	}
	static protected void MakeSureAllOccupiedBuildingsHaveExistingResources(List<string> existing, List<string> occupied)
	{
		for (int i = 0; i < occupied.Count; i++) {
			string occupy = occupied[i];
			bool found = false;
			for (int j = 0; j < existing.Count; j++) {
				string exist = existing[j];
				if(exist == occupy) {
					found = true;
					break;
				} 
			}
			if (!found) {
				Debug.LogError ("BUILDING HARMONY ISSUE: The Building Resource '" + occupy + "' is trying to be occupied in data, but isn't declared in data.");
			}
		}
	}
	
	static protected void MakeSureAllExistingResourcesHaveDesignBuildings(List<string> existing)
	{
		for (int i = 0; i < existing.Count; i++) {
			string exist = existing[i];
			bool found = false;
			for (int j = 0; j < Building_Script.buildingIDs.Count; j++) {
				string script = Building_Script.buildingIDs[j];
				if(exist == script) {
					Manager_Resources.GetBuildingByID(exist).building = Building_Script.GetBuilding(script);
					found = true;
					break;
				} 
			}
			if (!found) {
				Debug.LogError ("BUILDING HARMONY ISSUE: The Building Resource '" + exist + "' is declared in data but does not exist in design land.");
			}
		}
	}

	static protected void MakeSureAllDesignBuildingsHaveExistingResources(List<string> existing)
	{
		for (int i = 0; i < Building_Script.buildingIDs.Count; i++) {
			string script = Building_Script.buildingIDs[i];
				bool found = false;
			for (int j = 0; j < existing.Count; j++) {
				string exist = existing[j];
				if(exist == script) {
					Building_Script.GetBuilding(script).resources = Manager_Resources.GetBuildingByID(exist);
					found = true;
					break;
				}
			}
			if (!found) {
				Debug.LogError ("BUILDING HARMONY ISSUE: The Building Script '" + script + "' exists in design land, but is not declared in data.");
			}
		}
	}

	static protected void SetReferencesBetweenResourcesAndDesignBuildings(List<string> existing)
	{
		for (int i = 0; i < existing.Count; i++) {
			string exist = existing[i];
			for (int j = 0; j < Building_Script.buildingIDs.Count; j++) {
				string script = Building_Script.buildingIDs[j];
				if(exist == script) {
					Manager_Resources.GetBuildingByID(exist).building = Building_Script.GetBuilding(script);
					Building_Script.GetBuilding(script).resources = Manager_Resources.GetBuildingByID(exist);
				} 
			}
		}
	}
	#endregion

	#region Harmonize Static Data To Design Land
	static protected void HarmonizePlayerResourcesToDesignWorld()
	{
		NPC_Spawner.Instance.SpawnPlayer(Manager_Resources.player);
	}
	#endregion

	#region Harmonize Static Data To Design Land
	static protected void HarmonizeOfficerResourcesToDesignWorld()
	{
		for (int k = 0; k < Manager_Resources.officers.Count; k++)
		{
			NPC_Spawner.Instance.SpawnOfficer(Manager_Resources.officers[k]);
		}
	}
	#endregion
}