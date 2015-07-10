using UnityEngine;
using System.Collections;

public class ShopIconScript : MonoBehaviour 
{
	private GameObject playerTrigger = null;
	private Vector3 currentSize = Vector3.zero;
	private float maxDist = 16.0f;
	private Vector3 originalScale = Vector3.zero;

	private void Start()
	{
		originalScale = transform.localScale;
		playerTrigger = GameObject.FindGameObjectWithTag("Player Trigger");
		enabled = false;
	}

	private void Update()
	{
		float curDist = Vector3.Distance (transform.position, playerTrigger.transform.position);
		float curPercentage = curDist / maxDist * 100.0f;
		transform.localScale = originalScale * (curPercentage * 0.01f) + originalScale * 0.35f;
		if (transform.localScale.magnitude > originalScale.magnitude)
			transform.localScale = originalScale;
	}

	public void Disable()
	{
		transform.localScale = originalScale;
		enabled = false;
	}
}
