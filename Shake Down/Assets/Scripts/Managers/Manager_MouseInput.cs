using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class Manager_MouseInput : MonoBehaviour 
{
	#region Singleton
	public static Manager_MouseInput Instance 
	{
		get 
		{
			if(instance == null)
				instance = FindObjectOfType(typeof(Manager_MouseInput)) as Manager_MouseInput;
			
			return instance;
		}
		set 
		{
			instance = value;
		}
	}
	private static Manager_MouseInput instance;
	#endregion

	[SerializeField] private Camera mainCamera = null, miniMapCamera = null, UICamera = null;

	[SerializeField] private LayerMask mainCamMask, miniMapCamMask, uiCamMask;

	public List<Collider> mainHitList = new List<Collider>();
	public List<Collider> miniMapHitList = new List<Collider>();
	public List<Collider> uiHitList = new List<Collider>();
	
	private void Update()
	{
		RaycastHit[] hits = Physics.RaycastAll (mainCamera.ScreenPointToRay (Input.mousePosition), 1000.0f, mainCamMask);
		mainHitList.Clear();
		if(hits.Length > 0)
		{
			foreach (RaycastHit curHit in hits) 
				mainHitList.Add(curHit.collider);
		}

		hits = Physics.RaycastAll (miniMapCamera.ScreenPointToRay (Input.mousePosition), 1000.0f, miniMapCamMask);

		ClearText (hits);
		miniMapHitList.Clear();
		if(hits.Length > 0)
		{
			foreach (RaycastHit curHit in hits) 
				miniMapHitList.Add(curHit.collider);

			foreach (Collider curCol in miniMapHitList) 
			{
				UI_DisplayText displayTextRef = curCol.gameObject.GetComponent<UI_DisplayText>();
				if(displayTextRef != null)
					displayTextRef.DisplayText();
			}
		}

		/*hits = Physics.RaycastAll (UICamera.ScreenPointToRay (Input.mousePosition));
		uiHitList.Clear();
		if(hits.Length > 0)
		{
			foreach (RaycastHit curHit in hits) 
				uiHitList.Add(curHit.collider);
		}*/

		if(Input.GetMouseButtonDown(0))
		{
			Collider waypoint = miniMapHitList.Find(mmh => mmh.CompareTag("Waypoint"));
			if(waypoint != null)
			{
				miniMapHitList.Remove(waypoint.GetComponent<Collider>());
				Destroy (waypoint.gameObject);
				return;
			}

			foreach (Collider curCol in miniMapHitList)
			{
				WaypointComponent waypointRef = curCol.gameObject.GetComponent<WaypointComponent>();
				if(waypointRef != null)
				{
					if(WaypointComponent.currentWaypoint != null)
						waypointRef.DestroyWaypoint();

					waypointRef.CreateWaypoint();
					break;
				}
			}
		}

	}

	private void ClearText(RaycastHit[] _newHits)
	{
		List<Collider> toKeepList = new List<Collider> ();

		foreach (RaycastHit curHit in _newHits) 
			toKeepList.Add(curHit.collider);

		foreach (Collider curCol in miniMapHitList) 
		{
			if(toKeepList.Contains(curCol))
				continue;

			UI_DisplayText displayTextRef = curCol.GetComponent<UI_DisplayText>();
			if(displayTextRef != null)
				displayTextRef.HideText();
		}
	}
}
