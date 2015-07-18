using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class SavingKeysContainer
{
	public static event Action<string> OnSaveGame;
	public static event Action<string> OnLoadGame;

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

	[System.Serializable]
	public struct GameSave
	{
		public string saveID;
	}

	public static GameSave SAVED_GAME_1, SAVED_GAME_2, SAVED_GAME_3;

	public static void InitializeSavedGames()
	{
		SAVED_GAME_1.saveID = "1_";
		SAVED_GAME_2.saveID = "2_";
		SAVED_GAME_3.saveID = "3_";
	}

	public static void SaveEvent(string _ID)
	{
		OnSaveGame (_ID);
	}
	public static void LoadEvent(string _ID)
	{
		OnLoadGame (_ID);
	}
}
