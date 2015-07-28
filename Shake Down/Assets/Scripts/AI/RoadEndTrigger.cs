using UnityEngine;
using System.Collections;

public class RoadEndTrigger : MonoBehaviour 
{
	[SerializeField] private GameObject camPoint = null;
	private GameObject childSpawner = null;

	private void Awake()
	{
		childSpawner = transform.GetChild (0).gameObject;
	}

	public void SpawnCar(GameObject _carToSpawn)
	{
		Vector3 spawnPos = childSpawner.transform.position;
		//spawnPos.x = transform.position.x + (Random.Range (-transform.localScale.x * 0.5f, transform.localScale.x * 0.5f));
		_carToSpawn.transform.position = spawnPos;

		Quaternion spawnRot = camPoint.transform.rotation;
		spawnRot.x = 0.0f;
		spawnRot.z = 0.0f;
		_carToSpawn.transform.rotation = spawnRot;
		_carToSpawn.SetActive (true);
		_carToSpawn.GetComponent<AICarParent> ().enabled = true;
		_carToSpawn.GetComponent<AICarParent> ().currentCamPoint = camPoint;
	}
}
