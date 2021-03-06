﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class AIParent : MonoBehaviour 
{
	[SerializeField] new protected string name = "";
	[SerializeField] protected float moveSpeed = 0.0f;
	[SerializeField] protected float triggerRefusalChance = 0.0f;
	[SerializeField] protected int shoppingAmount = 0;
	[SerializeField] protected float shoppingChance = 0.0f;
	[SerializeField] protected float shoppingTime = 0.0f;
	[SerializeField] protected float sleepingTime = 0.0f;
	[SerializeField] protected float talkingTime = 0.0f, talkingChance = 0.0f;
	
	protected bool _isGoingHome = false;
	public bool isGoingHome{get{return _isGoingHome;}set{_isGoingHome = value;}}
	
	protected int direction = 0;
	protected bool isCrossing = false;
	[SerializeField] protected GameObject currentCamPoint = null;
	protected int currentShoppingAmount = 0;
	
	protected List<GameObject> visibleObjects = new List<GameObject>();
	[SerializeField] protected LayerMask visionMask;
	
	public enum CurrentState
	{
		CS_Talking,
		CS_Shopping,
		CS_Sleeping,
		CS_Crossing,
		CS_WaitingToCross,
		CS_Interrogating,
		CS_Apprehending,
		
		CS_Walking
	}
	
	[SerializeField] protected CurrentState currentState = CurrentState.CS_Sleeping;
	
	protected CrossWalk currentCrossWalk = null;
	
	virtual protected void Start()
	{
		InvokeRepeating ("UpdateVisibleObjects", 0.15f, UnityEngine.Random.Range (0.15f, 0.25f));
	}
	
	virtual protected void UpdateVisibleObjects()
	{
		
	}
	
	virtual public void Initialize(GameObject _newCamPoint, GameObject _triggerObj)
	{
		isGoingHome = false;
		currentCamPoint = _newCamPoint;
		
		ResetDayTimers ();
		direction = RandomDirection ();
		StartCoroutine (ExitBuilding (_triggerObj.GetComponent<Collider>(), true));
		
		currentState = CurrentState.CS_Sleeping;
	}
	
	virtual protected void Update()
	{
		if(currentState == CurrentState.CS_Walking)
		{
			transform.position += (transform.right * moveSpeed * Time.deltaTime) * direction;
		}
		
	}
	
	virtual protected void OnTriggerEnter(Collider c)
	{
		if(c.CompareTag("Corner Trigger"))
		{
			if(c.GetComponent<CornerTrigger>().isImpassible)
			{
				direction *= -1;
				return;
			}
			//Turn Corner
			GameObject newCamPoint = c.GetComponent<CornerTrigger>().GetCitizenCamerapoint(currentCamPoint);
			currentCamPoint = newCamPoint;
			StartCoroutine (TurnCorner());
		}
		if(c.CompareTag("Door Trigger"))
		{
			//UnityEngine.Random chance to enter shop
			if(currentState != CurrentState.CS_Walking)
				return;
			
			if(currentShoppingAmount < 1)
			{
				return;
			}
			
			if(UnityEngine.Random.Range(0, 101) < shoppingChance)
			{
				//enter Shop
				currentState = CurrentState.CS_Shopping;
				StartCoroutine(EnterBuilding(c, false));
				--currentShoppingAmount;
			}
		}
		if(c.CompareTag("Cross Street Trigger"))
		{
			if(isCrossing)
				Invoke("StopCrossing", UnityEngine.Random.Range (0.35f, 0.55f));
			
			//UnityEngine.Random chance to cross street
			if(currentState != CurrentState.CS_Walking)
				return;
			
			
			if(UnityEngine.Random.Range (0,101) < triggerRefusalChance)
			{
				currentCrossWalk = c.gameObject.GetComponent<CrossWalk>();
				
				if(currentCrossWalk.currentCWState == CrossWalk.CrosswalkState.CWS_Green)
				{
					isCrossing = true;
					StartCoroutine(CrossStreet(c.gameObject));
				}
				else
				{
					if(!IsInvoking("WaitForGreenLight"))
					{
						InvokeRepeating("WaitForGreenLight", UnityEngine.Random.Range(0.35f, 0.55f), UnityEngine.Random.Range(0.5f, 1.0f));
					}
				}
			}
		}
		if(c.CompareTag("House Trigger"))
		{
			//check if time to go back home
			if(isGoingHome)
			{
				if(currentState == CurrentState.CS_Walking)
				{
					currentState = CurrentState.CS_Sleeping;
					StartCoroutine(EnterBuilding(c, true));
				}
			}
		}
		
		if(c.CompareTag("Citizen"))
		{
			if(UnityEngine.Random.Range(0, 101) < talkingChance && currentState == CurrentState.CS_Walking && !isGoingHome)
			{
				if(c.GetComponent<AIParent>().StartTalking())
				{
					currentState = CurrentState.CS_Talking;
					StartCoroutine(Talking());
				}
			}
		}
	}
	
	virtual protected IEnumerator EnterBuilding(Collider _trigger, bool _isHome)
	{
		yield return null;
	}
	
	virtual protected IEnumerator ExitBuilding(Collider _trigger, bool _isHome)
	{
		yield return null;
	}
	
	private IEnumerator CrossStreet(GameObject _crossingTrigger)
	{
		yield return new WaitForSeconds (UnityEngine.Random.Range(0.35f, 0.55f));
		
		currentState = CurrentState.CS_Crossing;
		
		GameObject newCamPoint =  _crossingTrigger.GetComponent<CornerTrigger> ().GetCitizenCamerapoint (currentCamPoint);
		currentCamPoint = newCamPoint;
		transform.eulerAngles = new Vector3 (transform.eulerAngles.x, newCamPoint.transform.eulerAngles.y, newCamPoint.transform.eulerAngles.z);
		
		//Vector3 targetVec = transform.forward * _crossingTrigger.GetComponent<CornerTrigger> ().distanceToCross;
		
		while(isCrossing)
		{
			transform.position += (_crossingTrigger.transform.forward * moveSpeed * Time.deltaTime);
			yield return null;
		}
		
		direction = RandomDirection ();
		currentState = CurrentState.CS_Walking;
		
		yield return null;
	}
	
	virtual protected IEnumerator TurnCorner()
	{
		yield return new WaitForSeconds (UnityEngine.Random.Range (0.125f, 0.25f));
		
		Vector3 targetEuler = currentCamPoint.transform.eulerAngles;
		targetEuler.x = transform.eulerAngles.x;
		transform.eulerAngles = targetEuler;
		
		yield return null;
	}
	
	virtual public bool StartTalking()
	{
		if(currentState == CurrentState.CS_Walking)
		{
			currentState = CurrentState.CS_Talking;
			StartCoroutine(Talking ());
			return true;
		}
		
		return false;
	}
	
	virtual protected IEnumerator Talking()
	{
		yield return new WaitForSeconds (talkingTime);
		
		currentState = CurrentState.CS_Walking;
		
		yield return null;
	}
	
	virtual protected int RandomDirection()
	{
		if (UnityEngine.Random.Range (1, 3) == 1)
			return 1;
		
		return -1;
	}
	
	virtual protected void StopCrossing()
	{
		isCrossing = false;
	}
	
	virtual protected void WaitForGreenLight()
	{
		currentState = CurrentState.CS_WaitingToCross;
		
		if(currentCrossWalk.currentCWState == CrossWalk.CrosswalkState.CWS_Green && currentState == CurrentState.CS_WaitingToCross)
		{
			isCrossing = true;
			StartCoroutine(CrossStreet(currentCrossWalk.gameObject));
			CancelInvoke("WaitForGreenLight");
		}
	}
	
	virtual protected void ResetDayTimers()
	{
		currentShoppingAmount = shoppingAmount;
		currentState = CurrentState.CS_Walking;
	}
}