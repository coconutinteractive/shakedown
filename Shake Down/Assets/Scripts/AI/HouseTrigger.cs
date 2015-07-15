using UnityEngine;
using System.Collections;

public class HouseTrigger : MonoBehaviour 
{
	[SerializeField] private GameObject _cameraPoint = null;
	public GameObject cameraPoint{get{return _cameraPoint;}set{_cameraPoint = value;}}

	public void OnDrawGizmos()
	{

	}

}
