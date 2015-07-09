using UnityEngine;
using System.Collections;

public class MiniMapScaleButton : MonoBehaviour 
{
	[SerializeField] private GameObject plusSprite;
	private MiniMapCamera miniMapRef = null;

	private void OnMouseDown()
	{
		if (miniMapRef == null)
			miniMapRef = transform.parent.GetComponent<MiniMapCamera> ();


		if (!miniMapRef._isScaling)
		{
			miniMapRef.Scale ();
			plusSprite.SetActive (plusSprite.activeInHierarchy ? false : true);
		}
	}

}
