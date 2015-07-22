using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class SavingKeysContainer
{
	public static event Action<string> OnSaveGame;
	public static event Action<string> OnLoadGame;
	
	#region Game Save Data
	public static bool SAVE1_ISTAKEN = false;
	public static bool SAVE2_ISTAKEN = false;
	public static bool SAVE3_ISTAKEN = false;
	
	public static string SAVE1_ID = "1_";
	public static string SAVE2_ID = "2_";
	public static string SAVE3_ID = "3_";
	
	public static string TIME_STAMP = "TIME_STAMP";
	public static string TIME_ELAPSED = "TIME_ELAPSED";
	
	public static string GAME_TIME_DATA = "GAME_TIME_DATA";
	
	#endregion
	
	#region Player Data
	public static string PLAYER_POSITION = "PLAYER_POSITION";
	public static string PLAYER_ROTATION = "PLAYER_ROTATION";
	public static string PLAYER_NAME = "PLAYER_NAME";
	public static string PLAYER_CANMOVE = "PLAYER_CANMOVE";
	public static string PLAYER_CAMERA_CAMPOINT = "PLAYER_CAMERA_CAMPOINT";
	#endregion
	
	#region AI Data
	public static string BASE_CITIZEN = "CITIZEN_"; // Use this + the citizen's name to save their state to Player Prefs.
	#endregion
	
	public static void SaveEvent(string _ID)
	{
		OnSaveGame (_ID);
		BinarySerialization.SaveToPlayerPrefs(_ID, true);	
		
		if(BinarySerialization.LoadFromPlayerPrefs (_ID + TIME_ELAPSED) == null)
			BinarySerialization.SaveToPlayerPrefs (_ID + TIME_ELAPSED, PlayerMovement.elapsedTime);
		else
			BinarySerialization.SaveToPlayerPrefs (_ID + TIME_ELAPSED, (float)BinarySerialization.LoadFromPlayerPrefs (_ID + TIME_ELAPSED) + PlayerMovement.elapsedTime);
	}
	public static void LoadEvent(string _ID)
	{
		OnLoadGame (_ID);
	}
	public static void DeleteSave(string _ID)
	{
		PlayerPrefs.DeleteKey(_ID + PLAYER_POSITION);
		PlayerPrefs.DeleteKey(_ID + PLAYER_ROTATION);
		PlayerPrefs.DeleteKey(_ID + PLAYER_NAME);
		PlayerPrefs.DeleteKey(_ID + PLAYER_CANMOVE);
		PlayerPrefs.DeleteKey(_ID + PLAYER_CAMERA_CAMPOINT);
		PlayerPrefs.DeleteKey(_ID + TIME_STAMP);
		PlayerPrefs.DeleteKey(_ID + TIME_ELAPSED);
		
		BinarySerialization.SaveToPlayerPrefs(_ID, false);
	}
}