using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Manager_AI : MonoBehaviour 
{
	#region Singleton
	public static Manager_AI Instance 
	{
		get 
		{
			if(instance == null)
				instance = FindObjectOfType(typeof(Manager_AI)) as Manager_AI;
			
			return instance;
		}
		set 
		{
			instance = value;
		}
	}
	private static Manager_AI instance;
	#endregion

	[SerializeField] private List<GameObject> housesList = new List<GameObject> ();
	[SerializeField] private List<GameObject> policeStationsList = new List<GameObject> ();

	[SerializeField] private GameObject citizenPrefab = null, policemanPrefab = null;

	private void Start()
	{
		foreach (GameObject curHouse in housesList)
		{
			GameObject newCitizen = GameObject.Instantiate(citizenPrefab, curHouse.transform.position + curHouse.transform.forward * 3.0f - curHouse.transform.up * 0.6f, curHouse.transform.rotation) as GameObject;
			newCitizen.GetComponent<AICitizen>().Initialize(curHouse.GetComponent<HouseTrigger>().cameraPoint, curHouse);
		}
		foreach (GameObject curPoliceStation in policeStationsList) 
		{
			GameObject newPoliceman = GameObject.Instantiate(policemanPrefab, curPoliceStation.transform.position + curPoliceStation.transform.forward * 3.0f - curPoliceStation.transform.up * 0.6f, curPoliceStation.transform.rotation) as GameObject;
			newPoliceman.GetComponent<AIPoliceman>().Initialize(curPoliceStation.GetComponent<HouseTrigger>().cameraPoint, curPoliceStation);
		}
	}

	public void MoveToPoliceStation(GameObject _objectToMove)
	{
		GameObject targetPoliceStation = policeStationsList [UnityEngine.Random.Range (0, policeStationsList.Count)];
		_objectToMove.transform.position = targetPoliceStation.transform.position + targetPoliceStation.transform.forward * 3.0f - targetPoliceStation.transform.up * 0.6f;
		//_objectToMove.transform.eulerAngles = targetPoliceStation.GetComponent<HouseTrigger> ().cameraPoint.transform.eulerAngles;
		_objectToMove.GetComponent<AIPoliceman> ().Initialize (targetPoliceStation.GetComponent<HouseTrigger> ().cameraPoint, targetPoliceStation);
	}

}
