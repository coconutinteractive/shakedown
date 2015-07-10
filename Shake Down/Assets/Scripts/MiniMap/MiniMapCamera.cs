using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class MiniMapCamera : MonoBehaviour 
{
	private GameObject playerObj = null;
	[SerializeField] private float followSpeed = 0.0f, scaleSpeed = 0.0f, zoomSpeed = 0.0f, minZoom = 0.0f, maxZoom = 0.0f;
	[SerializeField] private GameObject plusObj = null, minusObj = null;
	private Camera myCamera = null;
	private float baseX = 0.6f, baseY = 0.0f, baseW = 0.4f, baseH = 0.5f; 
	private float maxX = 0.0f, maxY = 0.0f, maxW = 1.0f, maxH = 1.0f; 
	private bool isScaling = false;
	public bool _isScaling{get{ return isScaling; }}

	public enum ZoomLevel
	{
		Zoom_Hidden,
		Zoom_Small,
		Zoom_Big,
		Zoom_FullScreen
	}

	[System.Serializable]
	public class ZoomLevelClass
	{
		public ZoomLevelClass (ZoomLevel zoomLevel, float[] zoomValues)
		{
			this.zoomLevel = zoomLevel;
			this.zoomValues = zoomValues;
		}
		
		public ZoomLevel zoomLevel;
		public float[] zoomValues = new float[4];
	}

	[SerializeField] private List<ZoomLevelClass> zoomLevels = new List<ZoomLevelClass>();

	private ZoomLevelClass currentZoomLevel;

	private void Start()
	{
		currentZoomLevel = zoomLevels.Find (zl => zl.zoomLevel == ZoomLevel.Zoom_Small);
		playerObj = GameObject.FindGameObjectWithTag ("Player");
		myCamera = GetComponent<Camera> ();
	}

	private void FixedUpdate()
	{
		Vector3 targetVec = playerObj.transform.position;
		targetVec.y = transform.position.y;
		transform.position = Vector3.Lerp (transform.position, targetVec, Time.deltaTime * followSpeed);

		if(Manager_MouseInput.Instance.miniMapHitList.Find(mm => mm.CompareTag("MiniMap Background Trigger")) != null)
		{
			myCamera.orthographicSize -= Input.GetAxis ("Mouse ScrollWheel") * zoomSpeed;
			myCamera.orthographicSize = Mathf.Clamp(myCamera.orthographicSize, minZoom, maxZoom);
		}
	}

	public void Scale(int _mod)
	{
		if(!isScaling)
		{
			StartCoroutine (ScaleMap (_mod));
			isScaling = true;
		}
	}

	private IEnumerator ScaleMap(int _mod)
	{
		if (currentZoomLevel.zoomLevel == ZoomLevel.Zoom_FullScreen && _mod == 1)
			yield return null;
		else if (currentZoomLevel.zoomLevel == ZoomLevel.Zoom_Hidden && _mod == -1)
			yield return null;
		else
		{
			currentZoomLevel = zoomLevels [zoomLevels.IndexOf (currentZoomLevel) + _mod];
			if(currentZoomLevel.zoomLevel == ZoomLevel.Zoom_FullScreen)
			{
				plusObj.GetComponent<Renderer>().enabled = false;
				plusObj.GetComponent<Collider>().enabled = false;
			}
			else if(currentZoomLevel.zoomLevel == ZoomLevel.Zoom_Hidden)
			{
				minusObj.GetComponent<Renderer>().enabled = false;
				minusObj.GetComponent<Collider>().enabled = false;
			}
			else
			{
				plusObj.GetComponent<Renderer>().enabled = true;
				plusObj.GetComponent<Collider>().enabled = true;
				minusObj.GetComponent<Renderer>().enabled = true;
				minusObj.GetComponent<Collider>().enabled = true;
			}
				
			Rect newRect = myCamera.rect;

			while(newRect.x >= currentZoomLevel.zoomValues[0] + 0.025f || newRect.x <= currentZoomLevel.zoomValues[0] - 0.025f)
			{
				newRect.x = Mathf.Lerp (newRect.x, currentZoomLevel.zoomValues[0], Time.deltaTime * scaleSpeed);
				newRect.y = Mathf.Lerp (newRect.y, currentZoomLevel.zoomValues[1], Time.deltaTime * scaleSpeed);
				newRect.width = Mathf.Lerp (newRect.width, currentZoomLevel.zoomValues[2], Time.deltaTime * scaleSpeed);
				newRect.height = Mathf.Lerp (newRect.height, currentZoomLevel.zoomValues[3], Time.deltaTime * scaleSpeed);
				myCamera.rect = newRect;
				yield return null;
			}
			
			newRect.x = currentZoomLevel.zoomValues[0];
			newRect.y = currentZoomLevel.zoomValues[1];
			newRect.width = currentZoomLevel.zoomValues[2];
			newRect.height = currentZoomLevel.zoomValues[3];
			myCamera.rect = newRect;
			
			isScaling = false;
		}

		yield return null;
	}

}
