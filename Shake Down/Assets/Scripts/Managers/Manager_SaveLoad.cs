using UnityEngine;
using System.Collections;
using AdrienSerializables;

public class Manager_SaveLoad : MonoBehaviour 
{
	#region Singleton
	private static Manager_SaveLoad instance;
	
	public static Manager_SaveLoad Instance 
	{
		get 
		{
			if(instance == null)
			{
				instance = FindObjectOfType(typeof(Manager_SaveLoad)) as Manager_SaveLoad;
				DontDestroyOnLoad(instance.gameObject);
			}
			
			return instance;
		}
		set 
		{
			instance = value;
		}
	}
	#endregion
	
	public int loadNewGame = 0;
	public bool displaySaveUI = true;
	public bool isMainMenu = true;
	
	private void Awake()
	{
		//persistent singleton
		if(instance == null)
		{
			instance = this;
			DontDestroyOnLoad(this);
		}
		else
		{
			if(this != instance)
				Destroy(this.gameObject);
		}
		
		if (BinarySerialization.LoadFromPlayerPrefs (SavingKeysContainer.SAVE1_ID) == null)
			BinarySerialization.SaveToPlayerPrefs (SavingKeysContainer.SAVE1_ID, false);
		
		if (BinarySerialization.LoadFromPlayerPrefs (SavingKeysContainer.SAVE2_ID) == null)
			BinarySerialization.SaveToPlayerPrefs (SavingKeysContainer.SAVE2_ID, false);
		
		if (BinarySerialization.LoadFromPlayerPrefs (SavingKeysContainer.SAVE3_ID) == null)
			BinarySerialization.SaveToPlayerPrefs (SavingKeysContainer.SAVE3_ID, false);
	}
	
	private void OnGUI()
	{
		if (!displaySaveUI)
			return;
		
		if(isMainMenu)
		{
			GUI.Box (new Rect (150.0f, 150.0f, 430.0f, 225.0f), "");
			for (int i = 0; i < 3; ++i) 
			{
				string addString = "";
				if(i == 0)
					addString = (bool)(BinarySerialization.LoadFromPlayerPrefs(SavingKeysContainer.SAVE1_ID)) ? "(Taken)" : "(Empty)";
				else if(i == 1)
					addString = (bool)(BinarySerialization.LoadFromPlayerPrefs(SavingKeysContainer.SAVE2_ID)) ? "(Taken)" : "(Empty)";
				else if(i == 2)
					addString = (bool)(BinarySerialization.LoadFromPlayerPrefs(SavingKeysContainer.SAVE3_ID)) ? "(Taken)" : "(Empty)";
				
				GUI.Box (new Rect (165.0f, 165.0f + i*70.0f, 270.0f, 55.0f), "Save " + (i+1).ToString() + addString);
				if(addString.StartsWith("(Taken)"))
				{
					if(BinarySerialization.LoadFromPlayerPrefs ((i+1).ToString() + "_" + SavingKeysContainer.TIME_ELAPSED) != null)
					{
						int[] timeValue = AdrienUtils.ConvertToTime((float)BinarySerialization.LoadFromPlayerPrefs((i+1).ToString() + "_" + SavingKeysContainer.TIME_ELAPSED));
						GUI.Label(new Rect(195.0f, 195.0f + i*70.0f, 270.0f, 55.0f), timeValue[0].ToString () + "h:" + timeValue[1].ToString() + "m:" + timeValue[2].ToString() + "s");
						MySerializables.GameTime gameTime = ((MySerializables.GameTime)BinarySerialization.LoadFromPlayerPrefs((i+1).ToString() + "_" + SavingKeysContainer.GAME_TIME_DATA));
						
						GUI.Label(new Rect(285.0f, 195.0f + i*70.0f, 270.0f, 55.0f), "Day " + gameTime.dayCount + ", " + gameTime.currentDayOfTheWeek + " " + gameTime.currentDayState); 
					}
					
					if(GUI.Button(new Rect(450.0f, 165.0f + i*70.0f, 50.0f, 55.0f), "LOAD"))
					{
						loadNewGame	= (i+1);
						Invoke("StartLoadedGame", 0.05f);
						displaySaveUI = false;
						isMainMenu = false;
						Application.LoadLevel("Scene01_AdrienMovementTest");
					}
					if(GUI.Button(new Rect(515.0f, 165.0f + i*70.0f, 50.0f, 55.0f), "DEL"))
					{
						string id = (i+1).ToString() + "_";
						SavingKeysContainer.DeleteSave(id);
					}
				}
				else
				{
					if(GUI.Button(new Rect(450.0f, 165.0f + i*70.0f, 115.0f, 55.0f), "NEW"))
					{
						loadNewGame	= (i+1);
						displaySaveUI = false;
						isMainMenu = false;
						Application.LoadLevel("Scene01_AdrienMovementTest");
					}
				}
			}
		}
		else
		{
			GUI.Box (new Rect (150.0f, 150.0f, 430.0f, 250.0f), "");
			GUI.Box (new Rect (165.0f, 165.0f, 300.0f, 70.0f), "Save " + loadNewGame.ToString() + ": Current");
			if(GUI.Button(new Rect(482.5f, 165.0f, 80.0f, 70.0f), "OVERRIDE"))
			{
				SavingKeysContainer.SaveEvent(loadNewGame.ToString() + "_");
				displaySaveUI = false;
			}
			
			if(GUI.Button(new Rect(165.0f, 250.0f, 400.0f, 35.0f), "SAVE AND QUIT"))
			{
				SavingKeysContainer.SaveEvent(loadNewGame.ToString() + "_");
				isMainMenu = true;
				Application.LoadLevel("Scene02_LoadGame");
			}
		}
	}
	
	private void StartLoadedGame()
	{
		if(loadNewGame != 0)
		{
			string id = loadNewGame.ToString() + "_";
			SavingKeysContainer.LoadEvent(id);
		}
	}
	
}