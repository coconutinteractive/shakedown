using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour 
{
	private GameObject targetObj = null;
	private GameObject playerObj = null;
	[SerializeField] private GameObject currentCameraPoint = null;
	public GameObject curCamPoint{get{return currentCameraPoint;}}
	[SerializeField] private float moveSpeed = 0.0f;
	[SerializeField] private Vector3 cameraOffset = Vector3.zero;
	private Quaternion myRotation;

	private void Start()
	{
		playerObj = GameObject.FindGameObjectWithTag("Player");
		targetObj = playerObj;
	}

	public void MoveTransition(GameObject _camPoint)
	{
		targetObj = _camPoint;
		currentCameraPoint = _camPoint;
	}

	private void FixedUpdate()
	{
		if(targetObj != playerObj)
		{
			transform.position = Vector3.Lerp (transform.position, targetObj.transform.position, Time.deltaTime * moveSpeed * 1.5f);
			myRotation = transform.rotation;
			myRotation = Quaternion.Slerp(myRotation, targetObj.transform.rotation, moveSpeed * 0.075f);
			transform.rotation = myRotation;
			//transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, targetObj.transform.eulerAngles, Time.deltaTime * moveSpeed * 3.0f);

			if(Vector3.Distance(transform.position, targetObj.transform.position) < 0.15f)
			{
				//transform.eulerAngles = targetObj.transform.eulerAngles;
				targetObj = playerObj;
			}
		}
		else
		{
			Vector3 targetVec = targetObj.transform.position + targetObj.transform.right * (3.0f * Input.GetAxis("Horizontal")) + targetObj.transform.forward * cameraOffset.z + targetObj.transform.up * cameraOffset.y;
			transform.position = Vector3.Lerp(transform.position, targetVec, Time.deltaTime * moveSpeed);
		}


	}
}
