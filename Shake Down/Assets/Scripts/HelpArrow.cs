using UnityEngine;
using System.Collections;

public class HelpArrow : MonoBehaviour 
{
	private bool isActive = false;
	private GameObject target = null;

	private void Update()
	{
		if(isActive && target != null)
		{
			transform.LookAt(target.transform);
		}
		else if(target == null && isActive)
		{
			Deactivate();
		}
	}

	public void Activate(GameObject _newTarget)
	{
		isActive = true;
		target = _newTarget;
		transform.GetChild (0).gameObject.SetActive (true);
	}

	public void Deactivate()
	{
		isActive = false;
		transform.GetChild (0).gameObject.SetActive (false);
	}

}
