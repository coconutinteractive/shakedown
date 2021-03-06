﻿using UnityEngine;
using System.Collections;

public class CrossWalk : MonoBehaviour 
{
	[SerializeField] private bool hasStreetLight = true;

	public enum CrosswalkState
	{
		CWS_Green = 0,
		CWS_Orange,
		CWS_Red
	}

	[SerializeField] private float timeToGreen = 0.0f, timeToOrange = 0.0f, timeToRed = 0.0f;
	private float curTimeToGreen = 0.0f, curTimeToOrange = 0.0f, curTimeToRed = 0.0f;

	[SerializeField] private GameObject greenLightObj = null, orangeLightObj = null, redLightObj = null;

	[SerializeField] private CrosswalkState _currentCWState = CrosswalkState.CWS_Green;
	public CrosswalkState currentCWState{get{return _currentCWState;}}

	[SerializeField] private CrosswalkTrigger myCrossWalkTrigger = null;


	private void Start()
	{
		if (!hasStreetLight)
			return;

		curTimeToGreen = timeToGreen;
		curTimeToOrange = timeToOrange;
		curTimeToRed = timeToRed;

		if(_currentCWState == CrosswalkState.CWS_Green)
		{
			greenLightObj.SetActive(true);
			orangeLightObj.SetActive(false);
			redLightObj.SetActive(false);
		}
		if(_currentCWState == CrosswalkState.CWS_Red)
		{
			greenLightObj.SetActive(false);
			orangeLightObj.SetActive(false);
			redLightObj.SetActive(true);
		}

		myCrossWalkTrigger = transform.GetChild (0).gameObject.GetComponent<CrosswalkTrigger> ();
		myCrossWalkTrigger.UpdateState(currentCWState);
		StartCoroutine (ChangeState ());
	}

	private IEnumerator ChangeState()
	{
		while(true)
		{
			while(_currentCWState == CrosswalkState.CWS_Green)
			{
				curTimeToOrange -= Time.deltaTime;
				if(curTimeToOrange <= 0.0f)
				{
					_currentCWState = CrosswalkState.CWS_Orange;
					curTimeToRed = timeToRed;
					greenLightObj.SetActive(false);
					orangeLightObj.SetActive(true);
					myCrossWalkTrigger.UpdateState(currentCWState);
					break;
				}
				yield return null;
			}

			while(_currentCWState == CrosswalkState.CWS_Orange)
			{
				curTimeToRed -= Time.deltaTime;
				if(curTimeToRed <= 0.0f)
				{
					_currentCWState = CrosswalkState.CWS_Red;
					curTimeToGreen = timeToGreen;
					orangeLightObj.SetActive(false);
					redLightObj.SetActive(true);
					myCrossWalkTrigger.UpdateState(currentCWState);
					break;
				}
				yield return null;
			}

			while(_currentCWState == CrosswalkState.CWS_Red)
			{
				curTimeToGreen -= Time.deltaTime;
				if(curTimeToGreen <= 0.0f)
				{
					_currentCWState = CrosswalkState.CWS_Green;
					curTimeToOrange = timeToOrange;
					redLightObj.SetActive(false);
					greenLightObj.SetActive(true);
					myCrossWalkTrigger.UpdateState(currentCWState);
					break;
				}
				yield return null;
			}

			yield return null;
		}
	}
}
