using UnityEngine;
using System.Collections;

public class MiniMapCamera : MonoBehaviour 
{
	private GameObject playerObj = null;
	[SerializeField] private float followSpeed = 0.0f, scaleSpeed = 0.0f;
	private Camera myCamera = null;
	private float baseX = 0.6f, baseY = 0.0f, baseW = 0.4f, baseH = 0.5f; 
	private float maxX = 0.0f, maxY = 0.0f, maxW = 1.0f, maxH = 1.0f; 
	private bool isScaledUp = false, isScaling = false;
	public bool _isScaling{get{ return isScaling; }}

	private void Start()
	{
		playerObj = GameObject.FindGameObjectWithTag ("Player");
		myCamera = GetComponent<Camera> ();
	}

	private void FixedUpdate()
	{
		Vector3 targetVec = playerObj.transform.position;
		targetVec.y = transform.position.y;
		transform.position = Vector3.Lerp (transform.position, targetVec, Time.deltaTime * followSpeed);
	}

	public void Scale()
	{
		if(!isScaling)
		{
			StartCoroutine (ScaleMap ());
			isScaling = true;
		}
	}

	private IEnumerator ScaleMap()
	{
		Rect newRect = myCamera.rect;
		if(!isScaledUp)
		{
			while(newRect.x >= maxX + 0.05f)
			{
				newRect.x = Mathf.Lerp (newRect.x, maxX, Time.deltaTime * scaleSpeed);
				newRect.y = Mathf.Lerp (newRect.y, maxY, Time.deltaTime * scaleSpeed);
				newRect.width = Mathf.Lerp (newRect.width, maxW, Time.deltaTime * scaleSpeed);
				newRect.height = Mathf.Lerp (newRect.height, maxH, Time.deltaTime * scaleSpeed);
				myCamera.rect = newRect;
				yield return null;
			}

			newRect.x = maxX;
			newRect.y = maxY;
			newRect.width = maxW;
			newRect.height = maxH;
			myCamera.rect = newRect;
			
			isScaledUp = true;
			isScaling = false;
		}
		else
		{
			while(newRect.x <= baseX - 0.05f)
			{
				newRect.x = Mathf.Lerp (newRect.x, baseX, Time.deltaTime * scaleSpeed);
				newRect.y = Mathf.Lerp (newRect.y, baseY, Time.deltaTime * scaleSpeed);
				newRect.width = Mathf.Lerp (newRect.width, baseW, Time.deltaTime * scaleSpeed);
				newRect.height = Mathf.Lerp (newRect.height, baseH, Time.deltaTime * scaleSpeed);
				myCamera.rect = newRect;
				yield return null;
			}

			newRect.x = baseX;
			newRect.y = baseY;
			newRect.width = baseW;
			newRect.height = baseH;
			myCamera.rect = newRect;
			
			isScaledUp = false;
			isScaling = false;
		}

		yield return null;
	}

}
