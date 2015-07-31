using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Scenario_Script
{
	static private bool scenarioExists = false;

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
			JSONObject settings_player = scenario.GetField ("player");
			JSONObject settings_shopkeepers = scenario.GetField ("shopkeeper");
			JSONObject settings_officers = scenario.GetField ("officer");
			JSONObject settings_buildings = scenario.GetField ("building");

			List<string> occupiedBuildings = new List<string>();

			int i;
			for (i = 0; i < settings_buildings.Count; i++)
			{
				JSONObject building = settings_buildings[i];

				new Resources_Building(
					//id, name, image, type, money, income, expenses, rent, payment, day, inventory
					building.GetField ("id").str,
					building.GetField ("name").str,
					building.GetField ("image").str,
					Enums.BuildingTypeFromString(building.GetField ("type").str),
					int.Parse (building.GetField ("money").str),
					int.Parse (building.GetField ("income").str),
					int.Parse (building.GetField ("expenses").str),
					int.Parse (building.GetField ("rent").str),
					int.Parse (building.GetField ("payment").str),
					Enums.DaysOfWeekFromString(building.GetField ("day").str),
					Resources_Inventory.GenerateInventoryFromJSON(
						building.GetField ("inventory").str,
						building.GetField ("id").str
					)	 
				);
			}
			for (i = 0; i < settings_player.Count; i++)
			{
				JSONObject player = settings_player[i];
				
				new Resources_Player(
					//
					profileSettings,
					Resources_Building.GetBuildingByID(player.GetField ("home").str),
					int.Parse (player.GetField ("money").str),
					int.Parse (player.GetField ("income").str),
					int.Parse (player.GetField ("expenses").str),
					int.Parse (player.GetField ("strength").str),
					int.Parse (player.GetField ("presence").str),
					int.Parse (player.GetField ("opinion").str),
					Resources_Inventory.GenerateInventoryFromJSON(
						player.GetField ("inventory").str,
						profileSettings.id
					)
				);

				occupiedBuildings.Add (player.GetField("home").str);
			}
			for (i = 0; i < settings_shopkeepers.Count; i++)
			{
				JSONObject shopkeeper = settings_shopkeepers[i];

				if(occupiedBuildings.Contains(shopkeeper.GetField("home").str))
				{
					Debug.LogError ("Whoa there, Sally. Someone done goofed. Go tell whoever was messing with the static data that " + shopkeeper.GetField("id").str + " can't set up shop in " + shopkeeper.GetField("home").str + ". Someone else is already in there! -Scott");
				}

				Resources_Shopkeeper newShopkeeper = new Resources_Shopkeeper(
<<<<<<< HEAD
					//id, name, image, gender, home, money, income, expenses, 
					//strength, respect, fear, greed, integrity, stubbornness, attitude, inventory
					shopkeeper.GetField ("id").str,
					shopkeeper.GetField ("name").str,
					shopkeeper.GetField ("image").str,
					Enums.GenderFromString(shopkeeper.GetField ("gender").str),
					Resources_Building.GetBuildingByID(shopkeeper.GetField ("home").str),
					int.Parse (shopkeeper.GetField ("money").str),
					int.Parse (shopkeeper.GetField ("income").str),
					int.Parse (shopkeeper.GetField ("expenses").str),
					int.Parse (shopkeeper.GetField ("strength").str),
					int.Parse (shopkeeper.GetField ("respect").str),
					int.Parse (shopkeeper.GetField ("fear").str),
					int.Parse (shopkeeper.GetField ("greed").str),
					int.Parse (shopkeeper.GetField ("integrity").str),
					int.Parse (shopkeeper.GetField ("stubbornness").str),
					Resources_Inventory.GenerateInventoryFromJSON(
						shopkeeper.GetField ("inventory").str,
				        shopkeeper.GetField ("id").str
					)
				);

				//Building_Script.GetBuilding(shopkeeper.GetField("home").str).RegisterShopkeeper(newShopkeeper);
				occupiedBuildings.Add (shopkeeper.GetField("home").str);
=======
					shopkeeper.GetField("id").str,
					int.Parse (shopkeeper.GetField("strength").str),
					int.Parse (shopkeeper.GetField("respect").str),
					int.Parse (shopkeeper.GetField("fear").str),
					int.Parse (shopkeeper.GetField("money").str),
					int.Parse (shopkeeper.GetField("income").str),
					int.Parse (shopkeeper.GetField("expenses").str),
					int.Parse (shopkeeper.GetField("protectionpayment").str),
					shopkeeper.GetField("gender").str,
					shopkeeper.GetField("portrait").str,
					shopkeeper.GetField("buildingid").str
					);
				occupiedBuildings.Add (shopkeeper.GetField("buildingid").str);
>>>>>>> origin/master
			}
			
			List<string> temporaryPoliceStationList = new List<string>();
			for (i = 0; i < settings_officers.Count; i++)
			{
				JSONObject officer = settings_officers[i];

				if(occupiedBuildings.Contains(officer.GetField("home").str))
				{
					Debug.LogError ("Dial it back there, toots. " + officer.GetField ("home").str + " isn't a police station. Go tell whoever was doing the static data to send officer " + officer.GetField("id").str + " someplace else! -Scott");
				}

				new Resources_Officer(
					//id, name, image, gender, home, money, income, expenses,
					//strength, respect, fear, greed, integrity, stubbornness, attitude, inventory
					officer.GetField ("id").str,
					officer.GetField ("name").str,
					officer.GetField ("image").str,
					Enums.GenderFromString(officer.GetField ("gender").str),
					Resources_Building.GetBuildingByID(officer.GetField ("home").str),
					int.Parse (officer.GetField ("money").str),
					int.Parse (officer.GetField ("income").str),
					int.Parse (officer.GetField ("expenses").str),
					int.Parse (officer.GetField ("strength").str),
					int.Parse (officer.GetField ("respect").str),
					int.Parse (officer.GetField ("fear").str),
					int.Parse (officer.GetField ("greed").str),
					int.Parse (officer.GetField ("integrity").str),
					int.Parse (officer.GetField ("stubbornness").str),
					Resources_Inventory.GenerateInventoryFromJSON(
						officer.GetField ("inventory").str,
						officer.GetField ("id").str
					)
				);

				temporaryPoliceStationList.Add (officer.GetField("home").str);
			}
			foreach(string policeStation in temporaryPoliceStationList)
			{
				occupiedBuildings.Add (policeStation);
			}

			int j;
			List<string> staticBuildingsToCheck = occupiedBuildings;
			List<string> designBuildingsToCheck = Building_Script.buildingIDs;
			for(i = designBuildingsToCheck.Count - 1; i >= 0; i--)
			{
				bool buildingFound = false;
				for(j = staticBuildingsToCheck.Count - 1; j >= 0; j--)
				{
					if(j == 4 || j == 6 || j == 10 || j == 11)
					{
						Building_Script.GetBuilding(designBuildingsToCheck[i]);
					}
					if(staticBuildingsToCheck[j] == designBuildingsToCheck[i])
					{
						staticBuildingsToCheck.RemoveAt(j);
						buildingFound = true;
					}
				}
				if(buildingFound)
				{
					designBuildingsToCheck.RemoveAt(i);
				}
			}
			foreach(string staticBuildingID in staticBuildingsToCheck)
			{ Debug.LogError ("Hey pal. " + staticBuildingID + " exists in the static data but not in the design world. I just thought you should know. -Scott"); }
			foreach(string designBuildingID in designBuildingsToCheck)
			{ Debug.LogWarning ("Hey pal. " + designBuildingID + " exists in the design world but isn't used in the static data. I just thought you should know. -Scott"); }
			scenarioExists = true;

		} else {
			Debug.LogError ("You really shoulnd't be loading a new scenario when you've already got one there, buddy. -Scott");
		}
	}
}
