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
<<<<<<< HEAD
	private Dictionary<string, JSONObject> _startingInventories = new Dictionary<string, JSONObject>();

=======
	
>>>>>>> origin/master
	private void populateData()
	{
		_worldData = new JSONObject (worldDataText.ToString ());
		_dialogueData = new JSONObject (dialogueDataText.ToString ());
		_scenarioData = new JSONObject (scenarioDataText.ToString ()); 
		_localizationData = removeTabHeadersAsKeys(new JSONObject (localizationDataText.ToString()));
<<<<<<< HEAD
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
=======
>>>>>>> origin/master
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
	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			populateData();
			if(!tempB)
			{
				tempB = true;
<<<<<<< HEAD
				populateData();
				Debug.Log (" ====================== DATA POPULATED ====================== ");
				//activePrompt = Dialogue_Prompt.GetPromptByName("dialogue_prompt_outsideShop");
			}
			//Dialogue_Panel_Script.panelReference.StartDialogue(Manager_Resources.player, Manager_Resources.shopkeepers["evan"], Manager_Resources.shopkeepers["evan"].buildingRef);
=======
				Dictionary<string, string> profile = new Dictionary<string, string>();
				//TODO: player profile name/gender/image/etc
				profile["gender"] = Resources_Master.GENDER_FEMALE;
				profile["portrait"] = "DefaultFace";
				Scenario_Script.SetupScenarioFromJSON("scenario_01",_scenarioData, profile);
			}
			gameObject.transform.FindChild ("DialogueRoot").FindChild("DialoguePanelMain").GetComponent<Dialogue_Script>().ClearDisplay();

>>>>>>> origin/master
		}
	}
}