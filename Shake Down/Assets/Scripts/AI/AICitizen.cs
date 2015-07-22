using UnityEngine;
using System.Collections;

[System.Serializable]
public class AICitizen : AIParent 
{
	private GameObject _currentHouse = null;
	public GameObject currentHouse{get{return _currentHouse;}set{_currentHouse = value;}}

	protected override IEnumerator EnterBuilding (Collider _trigger, bool _isHome)
	{
		Vector3 targetVec = _trigger.gameObject.transform.position + _trigger.gameObject.transform.forward * 3.0f;
		targetVec.y = transform.position.y;
		
		while(Vector3.Distance(transform.position, targetVec) > 0.15f)
		{
			transform.position = Vector3.Lerp(transform.position, targetVec, Time.deltaTime * moveSpeed * 0.45f);
			yield return null;
		}
		
		if (!_isHome) 
		{
			yield return new WaitForSeconds (shoppingTime + UnityEngine.Random.Range (-shoppingTime * 0.5f, shoppingTime * 0.5f));
			StartCoroutine (ExitBuilding (_trigger, _isHome));
		}
		else
		{
			currentHouse = _trigger.gameObject;
			Manager_AI.Instance.MoveToCitizenObjectPool(gameObject);
		}
		
		yield return null;
	}

	protected override IEnumerator ExitBuilding (Collider _trigger, bool _isHome)
	{
		Vector3 targetVec = _trigger.gameObject.transform.position + _trigger.gameObject.transform.forward * -0.25f;
		targetVec.y = transform.position.y;
		
		while(Vector3.Distance(transform.position, targetVec) > 0.15f)
		{
			transform.position = Vector3.Lerp(transform.position, targetVec, Time.deltaTime * moveSpeed * 0.45f);
			yield return null;
		}
		
		if(_isHome)
		{
			ResetDayTimers();
		}
		
		direction = RandomDirection ();
		currentState = CurrentState.CS_Walking;

		yield return null;
	}

}
