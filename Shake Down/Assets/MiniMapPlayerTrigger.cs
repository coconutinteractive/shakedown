using UnityEngine;
using System.Collections;

public class MiniMapPlayerTrigger : MonoBehaviour 
{
	private void OnTriggerEnter(Collider c)
	{
		if(c.CompareTag("Shop Icon"))
		{
			c.gameObject.GetComponent<ShopIconScript>().enabled = true;
		}
	}

	private void OnTriggerExit(Collider c)
	{
		if(c.CompareTag("Shop Icon"))
		{
			c.gameObject.GetComponent<ShopIconScript>().Disable();
		}
	}
}
