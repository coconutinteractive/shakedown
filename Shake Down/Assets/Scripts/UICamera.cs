using UnityEngine;
using System.Collections;

public class UICamera : MonoBehaviour 
{
	private GameObject playerObj = null;
	[SerializeField] private float followSpeed = 0.0f;

	private void Start()
	{
		playerObj = GameObject.FindGameObjectWithTag ("Player");
	}

	private void FixedUpdate()
	{
		Vector3 targetVec = playerObj.transform.position;
		targetVec.y = transform.position.y;
		transform.position = Vector3.Lerp (transform.position, targetVec, Time.deltaTime * followSpeed);
	}
}
