using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Manager_StaticData : MonoBehaviour
{
	[SerializeField] private TextAsset worldDataText;
	[SerializeField] private TextAsset dialogueDataText;
	[SerializeField] private TextAsset scenarioDataText;
	[SerializeField] private TextAsset localizationDataText;
	
	static private JSONObject _worldData;
	static private JSONObject _dialogueData;
	static private JSONObject _scenarioData;
	static private JSONObject _localizationData;
	private Dictionary<string, JSONObject> _startingInventories = new Dictionary<string, JSONObject>();
	
	private void populateData()
	{
		_worldData = new JSONObject (worldDataText.ToString ());
		_dialogueData = new JSONObject (dialogueDataText.ToString ());
		_scenarioData = new JSONObject (scenarioDataText.ToString ()); 
		_localizationData = removeTabHeadersAsKeys(new JSONObject (localizationDataText.ToString()));
		GenerateItemsFromJSON();
		Debug.Log ("ITEMS GENERATED: " + Item_Root.items.Count);
		GenerateShopInventoriesFromJSON();
		Debug.Log ("SHOP INVENTORIES GENERATED: " + Resources_InventoryShop.inventories.Count);
		GenerateStartingInventoriesFromJSON();
		Debug.Log ("STARTING INVENTORIES GENERATED: " + Resources_Inventory.inventories.Count);
		Dialogue_Script.SetupDialogueOptionsFromJSON(_dialogueData);

		LoadScenario();
	}
	#region Generate Items
	private void GenerateItemsFromJSON()
	{
		for (int i = 0; i < _worldData["Items"].Count; i++)
		{
			JSONObject entry = _worldData["Items"][i];
			switch (entry["type"].str)
			{
			case "item_weapon_melee": { 	new Item_Weapon_Melee	(entry["id"].str, int.Parse(entry["price"].str), int.Parse(entry["strength"].str), int.Parse(entry["presence"].str), int.Parse(entry["opinion"].str), int.Parse(entry["energy"].str)); break; }
			case "item_weapon_gun": { 		new Item_Weapon_Gun		(entry["id"].str, int.Parse(entry["price"].str), int.Parse(entry["strength"].str), int.Parse(entry["presence"].str), int.Parse(entry["opinion"].str), int.Parse(entry["energy"].str)); break; }
			case "item_gear_clothes": { 	new Item_Gear_Clothes	(entry["id"].str, int.Parse(entry["price"].str), int.Parse(entry["strength"].str), int.Parse(entry["presence"].str), int.Parse(entry["opinion"].str), int.Parse(entry["energy"].str)); break; }
			case "item_gear_shoes": { 		new Item_Gear_Shoes		(entry["id"].str, int.Parse(entry["price"].str), int.Parse(entry["strength"].str), int.Parse(entry["presence"].str), int.Parse(entry["opinion"].str), int.Parse(entry["energy"].str)); break; }
			case "item_gear_pin": { 		new Item_Gear_Pin		(entry["id"].str, int.Parse(entry["price"].str), int.Parse(entry["strength"].str), int.Parse(entry["presence"].str), int.Parse(entry["opinion"].str), int.Parse(entry["energy"].str)); break; }
			case "item_gear_neck": { 		new Item_Gear_Neck		(entry["id"].str, int.Parse(entry["price"].str), int.Parse(entry["strength"].str), int.Parse(entry["presence"].str), int.Parse(entry["opinion"].str), int.Parse(entry["energy"].str)); break; }
			case "item_gear_ring": { 		new Item_Gear_Ring		(entry["id"].str, int.Parse(entry["price"].str), int.Parse(entry["strength"].str), int.Parse(entry["presence"].str), int.Parse(entry["opinion"].str), int.Parse(entry["energy"].str)); break; }
			case "item_gear_wrist": { 		new Item_Gear_Wrist		(entry["id"].str, int.Parse(entry["price"].str), int.Parse(entry["strength"].str), int.Parse(entry["presence"].str), int.Parse(entry["opinion"].str), int.Parse(entry["energy"].str)); break; }
			case "item_decay_hair": { 		new Item_Decay_Hair		(entry["id"].str, int.Parse(entry["price"].str), int.Parse(entry["strength"].str), int.Parse(entry["presence"].str), int.Parse(entry["opinion"].str), int.Parse(entry["energy"].str), int.Parse(entry["duration"].str)); break; }
			case "item_decay_flower": { 	new Item_Decay_Flower	(entry["id"].str, int.Parse(entry["price"].str), int.Parse(entry["strength"].str), int.Parse(entry["presence"].str), int.Parse(entry["opinion"].str), int.Parse(entry["energy"].str), int.Parse(entry["duration"].str)); break; }
			case "item_consumable": { 		new Item_Consumable		(entry["id"].str, int.Parse(entry["price"].str), int.Parse(entry["strength"].str), int.Parse(entry["presence"].str), int.Parse(entry["opinion"].str), int.Parse(entry["energy"].str)); break; }
			case "item_instant": { 			new Item_Instant		(entry["id"].str, int.Parse(entry["price"].str), int.Parse(entry["strength"].str), int.Parse(entry["presence"].str), int.Parse(entry["opinion"].str), int.Parse(entry["energy"].str)); break; }
			default: {
				break; }
			}
		}
	}
	#endregion

	private void GenerateShopInventoriesFromJSON()
	{
		for (int i = 0; i < _worldData["ShopInventories"].Count; i++)
		{
			JSONObject entry = _worldData["ShopInventories"][i];

			JSONObject item1o = entry["item1"];
			JSONObject item2o = entry["item2"];
			JSONObject item3o = entry["item3"];
			JSONObject item4o = entry["item4"];

			Item_Root item1 = null;
			Item_Root item2 = null;
			Item_Root item3 = null;
			Item_Root item4 = null;
			
			if(item1o != null) { item1 = Item_Root.GetItemByID(item1o.str); }
			if(item2o != null) { item2 = Item_Root.GetItemByID(item2o.str); }
			if(item3o != null) { item3 = Item_Root.GetItemByID(item3o.str); }
			if(item4o != null) { item4 = Item_Root.GetItemByID(item4o.str); }
			
			new Resources_InventoryShop(entry["id"].str, item1, item2, item3, item4);
		}
	}

	private void GenerateStartingInventoriesFromJSON()
	{
		for (int i = 0; i < _worldData["StartingInventories"].Count; i++)
		{
			JSONObject entry = _worldData["StartingInventories"][i];

			JSONObject gunInfo		 = entry["gun"];
			JSONObject meleeInfo	 = entry["melee"];
			JSONObject clothesInfo	 = entry["clothes"];
			JSONObject shoesInfo	 = entry["shoes"];
			JSONObject pinInfo		 = entry["pin"];
			JSONObject neckInfo		 = entry["neck"];
			JSONObject ringInfo		 = entry["ring"];
			JSONObject wristInfo	 = entry["wrist"];
			JSONObject hairInfo		 = entry["hair"];
			JSONObject flowerInfo	 = entry["flower"];

			Item_Weapon_Gun gun = null;
			Item_Weapon_Melee melee = null;
			Item_Gear_Clothes clothes = null;
			Item_Gear_Shoes shoes = null;
			Item_Gear_Pin pin = null;
			Item_Gear_Neck neck = null;
			Item_Gear_Ring ring = null;
			Item_Gear_Wrist wrist = null;
			Item_Decay_Hair hair = null;
			Item_Decay_Flower flower = null;

			if(gunInfo != null) {		gun = (Item_Weapon_Gun)			Item_Root.GetItemByID(gunInfo.str); }
			if(meleeInfo != null) {		melee = (Item_Weapon_Melee)		Item_Root.GetItemByID(meleeInfo.str); }
			if(clothesInfo != null) {	clothes = (Item_Gear_Clothes)	Item_Root.GetItemByID(clothesInfo.str);}
			if(shoesInfo != null) {		shoes = (Item_Gear_Shoes)		Item_Root.GetItemByID(shoesInfo.str);}
			if(pinInfo != null) {		pin = (Item_Gear_Pin)			Item_Root.GetItemByID(pinInfo.str);}
			if(neckInfo != null) {		neck = (Item_Gear_Neck)			Item_Root.GetItemByID(neckInfo.str);}
			if(ringInfo != null) {		ring = (Item_Gear_Ring)			Item_Root.GetItemByID(ringInfo.str);}
			if(wristInfo != null) {		wrist = (Item_Gear_Wrist)		Item_Root.GetItemByID(wristInfo.str);}
			if(hairInfo != null) {		hair = (Item_Decay_Hair)		Item_Root.GetItemByID(hairInfo.str);}
			if(flowerInfo != null) {	flower	= (Item_Decay_Flower)	Item_Root.GetItemByID(flowerInfo.str);}
			
			Resources_Inventory inventory = new Resources_Inventory (entry["id"].str, gun, melee, clothes, shoes, pin, neck, ring, wrist, hair, flower);
		}
	}

	private void LoadScenario()
	{
		// TODO: Implement Player Profile Settings
		ProfileSettings profile = new ProfileSettings();
		profile.gender = Enums.Gender.female;
		profile.image = "DefaultFace";
		profile.name = "ProfileName";
		
		// Hard coded to load scenario_01;
		Scenario_Script.SetupScenarioFromJSON("scenario_01",_scenarioData, profile);
	}
	
	private JSONObject removeTabHeadersAsKeys(JSONObject jsonObject)
	{
		JSONObject data = new JSONObject();
		data.keys =  new List<string>();
		for (int i = 0; i < jsonObject.Count; i++)
		{
			for (int j = 0; j < jsonObject[i].Count; j++)
			{
				data.Add(jsonObject[i][j]);
				data.keys.Add (jsonObject[i].keys[j]);
			}
		}
		return data;
	}

	static public string ButtonKeyFromDialogueKey(string dialogueKey)
	{
		return _dialogueData[dialogueKey].GetField("button_text").ToString();
	}

	static public string getLocalizationTextFromKey(string key, Enums.Language locLanguage)
	{
		string locText = "";
		if (_localizationData.keys.IndexOf (key) == -1)
		{
			return key;
		}
		else
		{
			JSONObject obj = _localizationData.list[_localizationData.keys.IndexOf(key)];
			if(obj[locLanguage.ToString ()]) { locText = obj[locLanguage.ToString()].ToString (); }
			else {locText = key; }
			return locText;
		}
	}

	private bool tempB = false;
	private Dialogue_Prompt activePrompt;
	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			if(!tempB)
			{
				tempB = true;
				populateData();
				DialogueInterface.Instance.PopulateButtonActionDict();
				Debug.Log (" ====================== DATA POPULATED ====================== ");
			}
		}
	}
}