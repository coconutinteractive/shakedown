using UnityEngine;
using System.Collections;

public class AIPoliceman : AIParent 
{
	protected Resources_Officer _resources;
	public Resources_Officer resources { get { return _resources; } set { _resources = value; } }

	private GameObject _currentStation = null;
	public GameObject currentStation{get{return _currentStation;}set{_currentStation = value;}}

	protected override void Start ()
	{
		PlayerMovement.onJayWalking += HandleonJayWalking;;
		base.Start ();
	}

	private void HandleonJayWalking ()
	{
		GameObject potentialPlayerObj = visibleObjects.Find (vo => vo.CompareTag("Player"));
		if(potentialPlayerObj)
		{
			potentialPlayerObj.GetComponent<PlayerMovement>().Apprehend();
			currentState = CurrentState.CS_Apprehending;
			StartCoroutine(MoveTowards(potentialPlayerObj));
		}
	}

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
			yield return new WaitForSeconds (sleepingTime);
		}

		Manager_AI.Instance.MoveToPoliceStation (gameObject);
		
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
		
		direction = RandomDirection ();
		currentState = CurrentState.CS_Walking;
		
		yield return null;
	}

	protected override void UpdateVisibleObjects ()
	{
		visibleObjects.Clear ();
		if (currentState != CurrentState.CS_Walking)
			return;

		RaycastHit[] hits = Physics.SphereCastAll(transform.position + transform.up * 0.5f + transform.right * direction, 1.0f, transform.right * direction, 10.0f, visionMask);
		if(hits.Length > 0)
		{
			foreach (RaycastHit hit in hits)
			{
				visibleObjects.Add(hit.collider.gameObject);
			}
		}

		Debug.DrawLine (transform.position + transform.up * 0.5f + transform.right * direction, transform.position + (transform.right * direction) * 10.0f, Color.red, 0.15f);
	}

	protected override void Update ()
	{
		base.Update ();

	}

	private IEnumerator MoveTowards(GameObject _target)
	{
		Vector3 targetVec = _target.gameObject.transform.position;
		targetVec.y = transform.position.y;

		while(Vector3.Distance(transform.position, targetVec) > 0.5f)
		{
			transform.position = Vector3.Lerp(transform.position, targetVec, Time.deltaTime * moveSpeed * 2.0f);
			yield return null;
		}
		yield return new WaitForSeconds(talkingTime);

		currentState = CurrentState.CS_Walking;
	}
}
