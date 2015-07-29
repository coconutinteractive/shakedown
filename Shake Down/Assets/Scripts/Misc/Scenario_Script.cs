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

	static public void SetupScenarioFromJSON(string scenarioID, JSONObject json, Dictionary<string, string> profileSettings)
	{
		if(!scenarioExists)
		{
			JSONObject scenario = json[scenarioID];
			JSONObject settings_player = scenario.GetField ("player");
			JSONObject settings_shopkeepers = scenario.GetField ("shopkeeper");
			JSONObject settings_officers = scenario.GetField ("officer");

			List<string> occupiedBuildings = new List<string>();

			int i;
			for (i = 0; i < settings_player.Count; i++)
			{
				JSONObject player = settings_player[i];
				
				new Resources_Player(
					int.Parse(player.GetField ("strength").str),
					int.Parse(player.GetField ("presence").str),
					int.Parse(player.GetField ("opinion").str),
					int.Parse(player.GetField ("money").str),
					profileSettings["gender"],
					player.GetField("buildingid").str,
					profileSettings["portrait"]
					);

				occupiedBuildings.Add (player.GetField("buildingid").str);
			}
			for (i = 0; i < settings_shopkeepers.Count; i++)
			{
				JSONObject shopkeeper = settings_shopkeepers[i];

				if(occupiedBuildings.Contains(shopkeeper.GetField("buildingid").str))
				{
					Debug.LogError ("Whoa there, Sally. Someone done goofed. Go tell whoever was messing with the static data that " + shopkeeper.GetField("id").str + " can't set up shop in " + shopkeeper.GetField("buildingid").str + ". Someone else is already in there! -Scott");
				}

				Resources_Shopkeeper newShopkeeper = new Resources_Shopkeeper(
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

				newShopkeeper.buildingRef = Building_Script.GetBuilding(shopkeeper.GetField("buildingid").str);
				Building_Script.GetBuilding(shopkeeper.GetField("buildingid").str).RegisterShopkeeper(newShopkeeper);
				occupiedBuildings.Add (shopkeeper.GetField("buildingid").str);
			}
			
			List<string> temporaryPoliceStationList = new List<string>();
			for (i = 0; i < settings_officers.Count; i++)
			{
				JSONObject officer = settings_officers[i];

				if(occupiedBuildings.Contains(officer.GetField("buildingid").str))
				{
					Debug.LogError ("Dial it back there, toots. " + officer.GetField ("buildingid").str + " isn't a police station. Go tell whoever was doing the static data to send officer " + officer.GetField("id").str + " someplace else! -Scott");
				}

				new Resources_Officer(
					officer.GetField("id").str,
					int.Parse (officer.GetField("strength").str),
					int.Parse (officer.GetField("greed").str),
					int.Parse (officer.GetField("integrity").str),
					officer.GetField("gender").str,
					officer.GetField("buildingid").str,
					officer.GetField("portrait").str
				);
				temporaryPoliceStationList.Add (officer.GetField("buildingid").str);
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
