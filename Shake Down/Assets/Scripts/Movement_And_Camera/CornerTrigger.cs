using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CornerTrigger : MonoBehaviour
{
	[SerializeField] private bool _isAutomatic = false;
	public bool isAutomatic {get{return _isAutomatic;}}
	[SerializeField] private List<GameObject> cameraPoints = new List<GameObject>();
	private List<CameraPoint> cameraPointsRef = new List<CameraPoint>();
	[SerializeField] private bool isCrossTrigger = false;
	[SerializeField] private float _distanceToCross = 0.0f;
	public float distanceToCross{get{return _distanceToCross;}}
	public bool isImpassible = false;

	private void Start()
	{
		foreach (GameObject curCameraPoint in cameraPoints) 
			cameraPointsRef.Add(curCameraPoint.GetComponent<CameraPoint>());
	}

	public GameObject SwitchCameraPoint()
	{
		cameraPointsRef.ForEach (cp => cp.isActive = !cp.isActive);
		return cameraPointsRef.Find(cpr => cpr.isActive).gameObject;
	}

	public GameObject GetCitizenCamerapoint(GameObject _currentCamPoint)
	{
		return cameraPoints.Find (cp => cp != _currentCamPoint);
	}

	private void OnDrawGizmos()
	{
		if (cameraPoints.Count < 2)
		{
			Gizmos.color = new Color(1.0f, 0.0f, 0.0f, 0.33f);
			if(!isCrossTrigger)
				Gizmos.DrawCube (transform.position, transform.localScale * 0.5f);
			if(isCrossTrigger)
				Gizmos.DrawSphere (transform.position, 0.5f);
		}
	}

	private void OnDrawGizmosSelected()
	{
		if (cameraPoints.Count > 1)
		{
			Gizmos.color = Color.green;
			foreach (GameObject curCamPoint in cameraPoints) 
			{
				Gizmos.DrawLine(transform.position, curCamPoint.transform.position);
			}
		}
	}
}
