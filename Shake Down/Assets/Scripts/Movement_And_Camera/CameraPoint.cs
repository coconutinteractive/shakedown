using UnityEngine;
using System.Collections;

public class CameraPoint : MonoBehaviour 
{
	[SerializeField] private bool _isActive = false;
	public bool isActive {get{return _isActive;} set{_isActive = value;}}

	private int _localID = 0;
	public int localID{get{return _localID;}}
	public static int currentID = 1;

	private void Start()
	{
		if(_localID == 0)
		{
			_localID = currentID;
			++currentID;
		}
	}

	private void OnDrawGizmos()
	{
		if(isActive)
		{
			Gizmos.color = new Color(0.0f, 1.0f, 0.0f, 0.33f);
			Gizmos.DrawSphere(transform.position, 1.0f);
		}
	}

	private void OnDestroy()
	{
		currentID = 1;
	}
}
