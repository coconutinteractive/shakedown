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

	[SerializeField] private List<GameObject> carSpawnersList = new List<GameObject>();
	public int maxAmountOfCars = 0;
	private List<GameObject> currentSpawnedCars = new List<GameObject>();
	[SerializeField] private List<GameObject> carObjectsPool = new List<GameObject>();

	static private float CAR_SPAWN_DELAY = 5.0f;

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

		StartCoroutine (CarSpawning());
	}

	public void MoveToPoliceStation(GameObject _objectToMove)
	{
		GameObject targetPoliceStation = policeStationsList [UnityEngine.Random.Range (0, policeStationsList.Count)];
		_objectToMove.transform.position = targetPoliceStation.transform.position + targetPoliceStation.transform.forward * 3.0f - targetPoliceStation.transform.up * 0.6f;
		//_objectToMove.transform.eulerAngles = targetPoliceStation.GetComponent<HouseTrigger> ().cameraPoint.transform.eulerAngles;
		_objectToMove.GetComponent<AIPoliceman> ().Initialize (targetPoliceStation.GetComponent<HouseTrigger> ().cameraPoint, targetPoliceStation);
	}

	private IEnumerator CarSpawning()
	{
		while(true)
		{
			if(currentSpawnedCars.Count < maxAmountOfCars)
			{
				RandomSpawnCar();
			}
			yield return new WaitForSeconds(CAR_SPAWN_DELAY / (maxAmountOfCars - currentSpawnedCars.Count + 1.0f));
		}
	}


	public void MoveToCarObjectPool(GameObject _carToMove)
	{
		_carToMove.SetActive (false);
		_carToMove.transform.position -= Vector3.up * 100.0f;
		currentSpawnedCars.Remove (_carToMove);
		carObjectsPool.Add (_carToMove);
	}

	public void RandomSpawnCar()
	{
		GameObject newCar = carObjectsPool [UnityEngine.Random.Range (0, carObjectsPool.Count)];
		carSpawnersList [UnityEngine.Random.Range (0, carSpawnersList.Count)].GetComponent<RoadEndTrigger>().SpawnCar (newCar);
		carObjectsPool.Remove (newCar);
		currentSpawnedCars.Add (newCar);
	}

}
