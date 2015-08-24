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

		Dialogue_Script.SetupDialogueOptionsFromJSON(_dialogueData);

		LoadScenario();
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