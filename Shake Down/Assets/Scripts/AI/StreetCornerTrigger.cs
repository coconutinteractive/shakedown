using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class StreetCornerTrigger : MonoBehaviour 
{
	[SerializeField] private List<GameObject> cameraPoints = new List<GameObject>();

	public GameObject GetNewCamPoint(GameObject _curCamPoint)
	{
		List<GameObject> availableCamPoints = new List<GameObject> ();
		foreach (GameObject curCamPoint in cameraPoints)
		{
			if(curCamPoint.gameObject != _curCamPoint.gameObject)
				availableCamPoints.Add(curCamPoint);
		}

		return availableCamPoints[Random.Range (0, availableCamPoints.Count)];
	}

#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		if(cameraPoints.Count < 2)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawSphere(transform.position, 1.0f);
		}
	}

	private void OnDrawGizmosSelected()
	{
		if(cameraPoints.Count > 0)
		{
			foreach (GameObject curCamPoint in cameraPoints) 
			{
				Gizmos.color = Color.green;
				Gizmos.DrawLine(transform.position, curCamPoint.transform.position);
				Gizmos.DrawSphere(curCamPoint.transform.position, 0.5f);
			}
		}
	}

#endif
}
