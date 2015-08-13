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
		//PopulateStartingInventories();
		LoadScenario();
	}
	
	private void PopulateStartingInventories()
	{
		/*for (int i = 0; i < _worldData["StartingInventories"].Count; i++)
		{

		}
		*/
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
				//data.keys.Add (jsonObject[i].keys[j]);
				data.Add(jsonObject[i][j]);
				data.keys.Add (jsonObject[i].keys[j]);
			}
		}
		return data;
	}
	
	static public string getLocalizationTextFromKey(string key, Enums.Language locLanguage)
	{
		string locText = "";
		JSONObject obj = _localizationData.list[_localizationData.keys.IndexOf(key)];
		locText = obj[locLanguage.ToString()].ToString ();
		return locText;
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
				Debug.Log (" ====================== DATA POPULATED ====================== ");
			}
		}
	}
}