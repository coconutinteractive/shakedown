using UnityEngine;
using System.Collections;

public class MiniMapBackground : MonoBehaviour
{
	private void Start()
	{
		if(GetComponent<Renderer> () != null)
			GetComponent<Renderer> ().enabled = true;
	}
}
