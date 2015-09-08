using UnityEngine;
using System.Collections;

public class NPC_Spawner : MonoBehaviour 
{
	[SerializeField] private GameObject playerPrefab;
	[SerializeField] private GameObject officerPrefab;
	[SerializeField] private Transform officerPool;
	
	#region Singleton
	public static NPC_Spawner Instance {
		get { if(instance == null) {
				instance = FindObjectOfType(typeof(NPC_Spawner)) as NPC_Spawner; }
			return instance; }
		set { instance = value; }
	}
	private static NPC_Spawner instance;
	#endregion
	
	public void SpawnPlayer(Resources_Player resources)
	{
		/*
		Transform spawnPoint = resources.home.building.transform;
		GameObject player = Instantiate (playerPrefab, spawnPoint.position, spawnPoint.rotation) as GameObject;
		((PlayerMovement)player.GetComponent("PlayerMovement")).resources = resources;
		player.name = "Player";
		*/
	}

	public void SpawnOfficer(Resources_Officer resources)
	{
		/*
		Transform spawnPoint = resources.home.building.transform;
		GameObject officer = Instantiate (officerPrefab, spawnPoint.position, spawnPoint.rotation) as GameObject;
		officer.transform.SetParent(officerPool);
		((AIPoliceman)officer.GetComponent("AIPoliceman")).resources = resources;
		officer.name = "Officer " + Localization.NameCase (((AIPoliceman)officer.GetComponent("AIPoliceman")).resources.id);
		*/
	}
}