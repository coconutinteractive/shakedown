using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Manager_StaticData : MonoBehaviour
{
	public GameObject temp;
	public TextAsset worldDataText;
	public TextAsset dialogueDataText;
	public TextAsset scenarioDataText;
	public TextAsset localizationDataText;

	static public string LOC_ENGLISH_US = "englishus";
	static public string LOC_ENGLISH_UK = "englishuk";

	private JSONObject _worldData;
	private JSONObject _dialogueData;
	private JSONObject _scenarioData;
	private JSONObject _localizationData;

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
		Dictionary<string, string> profile = new Dictionary<string, string>();
		profile["gender"] = Resources_Master.GENDER_FEMALE;
		profile["portrait"] = "DefaultFace";

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

	public string getLocalizationTextFromKey(string key, string locLanguage)
	{
		string locText = "";
		JSONObject obj = _localizationData.list[_localizationData.keys.IndexOf(key)];
		locText = obj[locLanguage].ToString ();
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
				activePrompt = Dialogue_Prompt.GetPromptByName("dialogue_prompt_outsideShop");
			}
			Debug.Log (activePrompt.promptID);
			for (int i = 0; i < activePrompt.followUps.Count; i++)
			{
				Debug.Log ("---Option " + i + ": " + activePrompt.followUps[i].optionID);
			}
			int randOption = Random.Range(0, activePrompt.followUps.Count);
			Dialogue_Option opt = activePrompt.followUps[randOption];

			Debug.Log ("[" + opt.optionID + "]");

			for (int j = 0; j < opt.followUps.Count; j++)
			{
				Debug.Log ("------Prompt " + j + ": " + opt.followUps[j].promptID);
			}
			int randPrompt = Random.Range(0, opt.followUps.Count);
			activePrompt = opt.followUps[randPrompt];
			Debug.Log (activePrompt.promptID);
			//Dialogue_Panel_Script.panelReference.StartDialogue(Manager_Resources.player, Manager_Resources.shopkeepers["evan"], Manager_Resources.shopkeepers["evan"].buildingRef);
		}
	}
}