using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class CrosswalkTrigger : MonoBehaviour 
{
	public event Action OnGreenLight;
	public event Action OnOrangeLight;
	public event Action OnRedLight;

	private CrossWalk.CrosswalkState _currentState;
	public CrossWalk.CrosswalkState currentState{get{return _currentState;}}

	public void UpdateState(CrossWalk.CrosswalkState _newState)
	{
		_currentState = _newState;

		if(_currentState == CrossWalk.CrosswalkState.CWS_Green)
		{
			if(OnGreenLight != null)
				OnGreenLight();
		}

		if(_currentState == CrossWalk.CrosswalkState.CWS_Orange)
		{
			if(OnOrangeLight != null)
				OnOrangeLight();
		}

		if(_currentState == CrossWalk.CrosswalkState.CWS_Red)
		{
			if(OnRedLight != null)
				OnRedLight();
		}
	}

	private void OnDrawGizmos()
	{

	}
}
