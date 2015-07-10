using UnityEngine;
using System.Collections;

public class MiniMapScaleButton : MonoBehaviour 
{
	[SerializeField] private int mod = 0;
	[SerializeField] private GameObject mapCameraObj = null;
	private MiniMapCamera miniMapRef = null;

	private void OnMouseDown()
	{
		if (miniMapRef == null)
			miniMapRef = mapCameraObj.GetComponent<MiniMapCamera>();

		if (!miniMapRef._isScaling)
			miniMapRef.Scale (mod);
	}
}
