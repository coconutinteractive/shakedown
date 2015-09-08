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
	
	[SerializeField] private int maxAmountOfCitizens = 0;
	[SerializeField] private int maxAmountOfPolicemen = 0;
	[SerializeField] private List<GameObject> housesList = new List<GameObject> ();
	[SerializeField] private List<GameObject> citizensObjectPool = new List<GameObject> ();
	[SerializeField] private List<GameObject> policemanObjectPool = new List<GameObject> ();
	[SerializeField] private List<GameObject> policeStationsList = new List<GameObject> ();
	private List<GameObject> currentCitizensList = new List<GameObject> ();
	private List<GameObject> currentPolicemanList = new List<GameObject> ();

	[SerializeField] private GameObject citizenPrefab = null, policemanPrefab = null;
	
	[SerializeField] private List<GameObject> carSpawnersList = new List<GameObject>();
	public int maxAmountOfCars = 0;
	private List<GameObject> currentSpawnedCars = new List<GameObject>();
	[SerializeField] private List<GameObject> carObjectsPool = new List<GameObject>();
	
	#region ALL CAM POINTS
	[SerializeField] private List<GameObject> allCamPoints = new List<GameObject> ();
	
	#endregion
	
	static private float CITIZEN_SPAWN_DELAY = 5.0f;
	static private float POLICEMAN_SPAWN_DELAY = 5.0f;
	static private float CAR_SPAWN_DELAY = 5.0f;
	
	
	private void Start()
	{
		Manager_GameTime.OnChangeDayState += HandleOnChangeDayState;;
		
		/*foreach (GameObject curHouse in housesList)
		{
			GameObject newCitizen = GameObject.Instantiate(citizenPrefab, curHouse.transform.position + curHouse.transform.forward * 3.0f - curHouse.transform.up * 0.6f, curHouse.transform.rotation) as GameObject;
			newCitizen.GetComponent<AICitizen>().Initialize(curHouse.GetComponent<HouseTrigger>().cameraPoint, curHouse);
		}
		foreach (GameObject curPoliceStation in policeStationsList) 
		{
			GameObject newPoliceman = GameObject.Instantiate(policemanPrefab, curPoliceStation.transform.position + curPoliceStation.transform.forward * 3.0f - curPoliceStation.transform.up * 0.6f, curPoliceStation.transform.rotation) as GameObject;
			newPoliceman.GetComponent<AIPoliceman>().Initialize(curPoliceStation.GetComponent<HouseTrigger>().cameraPoint, curPoliceStation);
		}*/
		
		StartCoroutine (CitizenSpawning());
		StartCoroutine (PolicemanSpawning());
		StartCoroutine (CarSpawning());
	}
	
	
	
	public void MoveToPoliceStation(GameObject _objectToMove)
	{
		GameObject targetPoliceStation = policeStationsList [UnityEngine.Random.Range (0, policeStationsList.Count)];
		_objectToMove.transform.position = targetPoliceStation.transform.position + targetPoliceStation.transform.forward * 3.0f - targetPoliceStation.transform.up * 0.6f;
		//_objectToMove.transform.eulerAngles = targetPoliceStation.GetComponent<HouseTrigger> ().cameraPoint.transform.eulerAngles;
		_objectToMove.GetComponent<AIPoliceman> ().Initialize (targetPoliceStation.GetComponent<HouseTrigger> ().cameraPoint, targetPoliceStation);
	}
	
	private IEnumerator CitizenSpawning()
	{
		while(true)
		{
			if(currentCitizensList.Count < maxAmountOfCitizens)
			{
				RandomSpawnCitizen();
			}
			yield return new WaitForSeconds(CITIZEN_SPAWN_DELAY / (maxAmountOfCitizens - currentCitizensList.Count + 1.0f));
		}
	}
	
	private IEnumerator PolicemanSpawning()
	{
		while(true)
		{
			if(currentPolicemanList.Count < maxAmountOfPolicemen)
			{
				RandomSpawnPoliceman();
			}
			yield return new WaitForSeconds(POLICEMAN_SPAWN_DELAY / (maxAmountOfPolicemen - currentPolicemanList.Count + 1.0f));
		}
		yield return null;
	}
	
	private IEnumerator CarSpawning()
	{
		while(true)
		{
			if(currentSpawnedCars.Count < maxAmountOfCars)
			{
				RandomSpawnCar();
			}
			
			float amountToWait = (CAR_SPAWN_DELAY / (maxAmountOfCars - currentSpawnedCars.Count + 1.0f)) + 1.0f;
			Mathf.Clamp(amountToWait, 1.0f, CAR_SPAWN_DELAY * 2.0f);
			
			yield return new WaitForSeconds(amountToWait);
		}
	}
	
	public void RandomSpawnCitizen()
	{
		GameObject newCitizen = citizensObjectPool [UnityEngine.Random.Range (0, citizensObjectPool.Count)];
		AICitizen citizenRef = newCitizen.GetComponent<AICitizen> ();
		
		citizensObjectPool.Remove (newCitizen);
		currentCitizensList.Add (newCitizen);
		newCitizen.SetActive (true);
		
		if(citizenRef.currentHouse == null)
		{
			GameObject curHouse = housesList [UnityEngine.Random.Range (0, housesList.Count)];
			newCitizen.transform.position = curHouse.transform.position + curHouse.transform.forward * 3.0f - Vector3.up * 0.65f;
			newCitizen.transform.rotation = curHouse.transform.rotation;
			citizenRef.Initialize (curHouse.GetComponent<HouseTrigger>().cameraPoint, curHouse);
		}
		else
			citizenRef.Initialize (citizenRef.currentHouse.GetComponent<HouseTrigger>().cameraPoint, citizenRef.currentHouse);
		
		citizenRef.enabled = true;
	}
	
	public void MoveToCitizenObjectPool(GameObject _citizenToMove)
	{
		_citizenToMove.SetActive (false);
		_citizenToMove.GetComponent<AICitizen> ().enabled = false;
		currentCitizensList.Remove (_citizenToMove);
		citizensObjectPool.Add (_citizenToMove);
	}

	public void RandomSpawnPoliceman()
	{
		GameObject newPoliceman = policemanObjectPool [UnityEngine.Random.Range (0, policemanObjectPool.Count)];
		AIPoliceman policemanRef = newPoliceman.GetComponent<AIPoliceman> ();
		
		policemanObjectPool.Remove (newPoliceman);
		currentPolicemanList.Add (newPoliceman);
		newPoliceman.SetActive (true);
		
		if(policemanRef.currentStation == null)
		{
			GameObject curStation = policeStationsList [UnityEngine.Random.Range (0, policeStationsList.Count)];
			newPoliceman.transform.position = curStation.transform.position + curStation.transform.forward * 3.0f - Vector3.up * 0.65f;
			newPoliceman.transform.rotation = curStation.transform.rotation;
			policemanRef.Initialize (curStation.GetComponent<HouseTrigger>().cameraPoint, curStation);
		}
		else
			policemanRef.Initialize (policemanRef.currentStation.GetComponent<HouseTrigger>().cameraPoint, policemanRef.currentStation);
		
		policemanRef.enabled = true;
	}
	
	public void MoveToPolicemanObjectPool(GameObject _policemanToMove)
	{
		_policemanToMove.SetActive (false);
		_policemanToMove.GetComponent<AIPoliceman> ().enabled = false;
		currentPolicemanList.Remove (_policemanToMove);
		policemanObjectPool.Add (_policemanToMove);
	}
	
	public void RandomSpawnCar()
	{
		GameObject newCar = carObjectsPool [UnityEngine.Random.Range (0, carObjectsPool.Count)];
		carSpawnersList [UnityEngine.Random.Range (0, carSpawnersList.Count)].GetComponent<RoadEndTrigger>().SpawnCar (newCar);
		carObjectsPool.Remove (newCar);
		currentSpawnedCars.Add (newCar);
	}
	
	public void MoveToCarObjectPool(GameObject _carToMove)
	{
		_carToMove.SetActive (false);
		_carToMove.GetComponent<AICarParent> ().enabled = false;
		_carToMove.transform.position -= Vector3.up * 100.0f;
		currentSpawnedCars.Remove (_carToMove);
		carObjectsPool.Add (_carToMove);
	}
	
	public GameObject GetSavedCamPoint(int _savedID)
	{
		foreach (GameObject curCP in allCamPoints) 
		{
			CameraPoint cpRef = curCP.GetComponent<CameraPoint>();
			if(cpRef.isActive)
				cpRef.isActive = false;
		}
		return allCamPoints.Find (camPoint => camPoint.GetComponent<CameraPoint> ().localID == _savedID);
	}
	
	private void HandleOnChangeDayState (Manager_GameTime.DayStateClass obj)
	{
		maxAmountOfCars = obj.maxCarAmount;
		maxAmountOfCitizens = obj.maxCitizenAmount;
		maxAmountOfPolicemen = obj.maxPolicemanAmount;
		
		int i = maxAmountOfCitizens;
		foreach (GameObject curCitizen in currentCitizensList) 
		{
			if(i > 0)
			{
				--i;
				continue;
			}
			
			curCitizen.GetComponent<AIParent>().isGoingHome = true;
		}

		int j = maxAmountOfPolicemen;
		foreach (GameObject curPoliceman in currentPolicemanList) 
		{
			if(j > 0)
			{
				--j;
				continue;
			}
			
			curPoliceman.GetComponent<AIParent>().isGoingHome = true;
		}
	}

}