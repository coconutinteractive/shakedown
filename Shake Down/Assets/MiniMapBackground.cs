using UnityEngine;
using System.Collections;

public class MiniMapBackground : MonoBehaviour
{
	private void Start()
	{
		GetComponent<Renderer> ().enabled = true;
	}
}
