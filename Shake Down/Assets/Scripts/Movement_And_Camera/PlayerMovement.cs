﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class PlayerMovement : MonoBehaviour 
{
	public static event Action onJayWalking;

	[SerializeField] private string playerName = "Bacon";

	private bool canMove = true;
	[System.Serializable]
	public enum PossibleAction
	{
		Action_EnterShop,
		Action_ExitShop,
		Action_TurnCorner,
		Action_CrossStreet,
		Action_Talk,

		Action_None
	}

	[System.Serializable]
	public class AvailableAction
	{
		public AvailableAction (PossibleAction _action, KeyCode _newActionKey)
		{
			this._action = _action;
			this._ID = UnityEngine.Random.Range(0, 10000);
			this._actionKey = _newActionKey;
		}

		public AvailableAction (PossibleAction _action, KeyCode _newActionKey, GameObject _triggerObj)
		{
			this._action = _action;
			this._ID = UnityEngine.Random.Range(0, 10000);
			this._actionKey = _newActionKey;
			this.triggerObj = _triggerObj;
		}

		public PossibleAction _action = PossibleAction.Action_None;
		public int _ID = 0;
		public KeyCode _actionKey;
		public GameObject triggerObj;
	}

	private List<AvailableAction> currentAvailableActions = new List<AvailableAction>();

	[SerializeField] private float moveSpeed = 0.0f;
	private Rigidbody myRigidbody = null;
	private Camera myCamera = null;
	private CrossWalk currentCrosswalk = null;
	private bool isBeingApprehended = false;

	private void Start()
	{
		SavingKeysContainer.OnSaveGame += HandleOnSaveGame;
		SavingKeysContainer.OnLoadGame += HandleOnLoadGame;
		myRigidbody = GetComponent<Rigidbody> ();
		myCamera = Camera.main;
	}




	private void OnGUI()
	{
		if(Event.current.type == EventType.KeyUp)
		{
			AvailableAction currentAction = currentAvailableActions.Find(aa => aa._actionKey == Event.current.keyCode);
			if(currentAction != null)
			{
				Execute(currentAction);
			}
		}

		if (Input.GetKeyDown (KeyCode.Keypad1))
			SavingKeysContainer.SaveEvent (SavingKeysContainer.SAVED_GAME_1.saveID);
		if (Input.GetKeyDown (KeyCode.Keypad2))
			SavingKeysContainer.SaveEvent (SavingKeysContainer.SAVED_GAME_2.saveID);
		if (Input.GetKeyDown (KeyCode.Keypad3))
			SavingKeysContainer.SaveEvent (SavingKeysContainer.SAVED_GAME_3.saveID);

		if(Input.GetKeyDown(KeyCode.Keypad7))
			SavingKeysContainer.LoadEvent (SavingKeysContainer.SAVED_GAME_1.saveID);
		if(Input.GetKeyDown(KeyCode.Keypad8))
			SavingKeysContainer.LoadEvent (SavingKeysContainer.SAVED_GAME_2.saveID);
		if(Input.GetKeyDown(KeyCode.Keypad9))
			SavingKeysContainer.LoadEvent (SavingKeysContainer.SAVED_GAME_3.saveID);
	}

	private void FixedUpdate()
	{
		if(canMove)
			myRigidbody.velocity = transform.TransformDirection(new Vector3(Input.GetAxis("Horizontal"), 0, 0/*Input.GetAxis("Vertical")*/)) * moveSpeed;
	}

	private void Execute(AvailableAction _currentAction)
	{
		switch(_currentAction._action)
		{
			case PossibleAction.Action_TurnCorner:
			{
				if(canMove)
					TurnCorner(_currentAction);
				break;
			}
			case PossibleAction.Action_EnterShop:
			{
				if(canMove)
				{
					canMove = false;
					StartCoroutine(EnterShop (_currentAction));
				}
				break;
			}
			case PossibleAction.Action_ExitShop:
			{
				StartCoroutine(ExitShop (_currentAction));
				break;
			}
			case PossibleAction.Action_CrossStreet:
			{
				if(canMove)
				{
					if(currentCrosswalk.currentCWState == CrossWalk.CrosswalkState.CWS_Red)
					{
						if(onJayWalking != null)
							onJayWalking();
					}

					canMove = false;
					StartCoroutine (CrossStreet(_currentAction));
				}
				break;
			}
			case PossibleAction.Action_Talk:
			{
				Talk();
				break;
			}
		}
	}

	private void TurnCorner(AvailableAction _currentAction)
	{
		GameObject newCamPoint =  _currentAction.triggerObj.GetComponent<CornerTrigger> ().SwitchCameraPoint ();
		myCamera.gameObject.GetComponent<CameraScript> ().MoveTransition (newCamPoint);
		transform.eulerAngles = new Vector3 (transform.eulerAngles.x, newCamPoint.transform.eulerAngles.y, newCamPoint.transform.eulerAngles.z);
		transform.position = _currentAction.triggerObj.transform.position - new Vector3(0.0f, transform.position.y, 0.0f);
	}

	private IEnumerator EnterShop(AvailableAction _currentAction)
	{
		myRigidbody.velocity = Vector3.zero;
		currentAvailableActions.Remove(currentAvailableActions.Find(aa => aa._action == PossibleAction.Action_EnterShop));
		Vector3 targetVec = _currentAction.triggerObj.transform.position + _currentAction.triggerObj.transform.forward * 3.0f;
		targetVec.y = transform.position.y;

		while(Vector3.Distance(transform.position, targetVec) > 0.15f)
		{
			transform.position = Vector3.Lerp(transform.position, targetVec, Time.deltaTime * moveSpeed * 0.45f);
			yield return null;
		}
		
		currentAvailableActions.Add (new AvailableAction (PossibleAction.Action_ExitShop, KeyCode.S, _currentAction.triggerObj));
	}

	private IEnumerator ExitShop(AvailableAction _currentAction)
	{
		myRigidbody.velocity = Vector3.zero;
		currentAvailableActions.Remove(currentAvailableActions.Find(aa => aa._action == PossibleAction.Action_ExitShop));
		Vector3 targetVec = _currentAction.triggerObj.transform.position + _currentAction.triggerObj.transform.forward * -0.25f;
		targetVec.y = transform.position.y;
		
		while(Vector3.Distance(transform.position, targetVec) > 0.15f)
		{
			transform.position = Vector3.Lerp(transform.position, targetVec, Time.deltaTime * moveSpeed * 0.45f);
			yield return null;
		}

		canMove = true;
//		currentAvailableActions.Add (new AvailableAction (PossibleAction.Action_ExitShop, KeyCode.S, _currentAction.triggerObj));
	}

	private IEnumerator CrossStreet(AvailableAction _currentAction)
	{
		myRigidbody.velocity = Vector3.zero;
		GameObject newCamPoint =  _currentAction.triggerObj.GetComponent<CornerTrigger> ().SwitchCameraPoint ();
		myCamera.gameObject.GetComponent<CameraScript> ().MoveTransition (newCamPoint);
		transform.eulerAngles = new Vector3 (transform.eulerAngles.x, newCamPoint.transform.eulerAngles.y, newCamPoint.transform.eulerAngles.z);
		
		Vector3 targetVec = _currentAction.triggerObj.transform.position + _currentAction.triggerObj.transform.forward * _currentAction.triggerObj.GetComponent<CornerTrigger>().distanceToCross;
		targetVec.y = transform.position.y;
		
		while(Vector3.Distance(transform.position, targetVec) > 0.5f)
		{
			transform.position = Vector3.Lerp(transform.position, targetVec, Time.deltaTime * moveSpeed * 0.5f);
			if(isBeingApprehended)
			{
				yield return new WaitForSeconds(0.25f);
				break;
			}
			yield return null;
		}

		if(isBeingApprehended)
		{
			newCamPoint =  _currentAction.triggerObj.GetComponent<CornerTrigger> ().SwitchCameraPoint ();
			myCamera.gameObject.GetComponent<CameraScript> ().MoveTransition (newCamPoint);
			transform.eulerAngles = new Vector3 (transform.eulerAngles.x, newCamPoint.transform.eulerAngles.y, newCamPoint.transform.eulerAngles.z);
			targetVec = _currentAction.triggerObj.transform.position;
			targetVec.y = transform.position.y;
			while(Vector3.Distance(transform.position, targetVec) > 0.5f)
			{
				transform.position = Vector3.Lerp(transform.position, targetVec, Time.deltaTime * moveSpeed * 0.5f);
				yield return null;
			}

			yield return new WaitForSeconds(7.5f);
			isBeingApprehended = false;
		}
		
		canMove = true;
		yield return null;
	}

	private void Talk()
	{
			
	}

	private void OnTriggerEnter(Collider c)
	{
		if (c.CompareTag ("Corner Trigger")) 
		{
			if(c.gameObject.GetComponent<CornerTrigger>().isAutomatic)
			{
				if(canMove)
					TurnCorner(new AvailableAction(PossibleAction.Action_TurnCorner, KeyCode.W, c.gameObject));
			}
			else
				currentAvailableActions.Add(new AvailableAction(PossibleAction.Action_TurnCorner, KeyCode.W, c.gameObject));
		}
		if(c.CompareTag("Door Trigger"))
	    {
			currentAvailableActions.Add(new AvailableAction(PossibleAction.Action_EnterShop, KeyCode.W, c.gameObject));
		}
		if(c.CompareTag("Cross Street Trigger"))
		{
			currentAvailableActions.Add(new AvailableAction(PossibleAction.Action_CrossStreet, KeyCode.S, c.gameObject));
			currentCrosswalk = c.gameObject.GetComponent<CrossWalk>();
		}
		if(c.CompareTag("Talk Trigger"))
		{
			currentAvailableActions.Add(new AvailableAction(PossibleAction.Action_Talk, KeyCode.W));
		}
	}

	private void OnTriggerExit(Collider c)
	{
		if (c.CompareTag ("Corner Trigger")) 
		{
			currentAvailableActions.Remove(currentAvailableActions.Find(aa => aa._action == PossibleAction.Action_TurnCorner));
		}
		if(c.CompareTag("Door Trigger"))
		{
			currentAvailableActions.Remove(currentAvailableActions.Find(aa => aa._action == PossibleAction.Action_EnterShop));
		}
		if(c.CompareTag("Cross Street Trigger"))
		{
			currentAvailableActions.Remove(currentAvailableActions.Find(aa => aa._action == PossibleAction.Action_CrossStreet));
		}
		if(c.CompareTag("Talk Trigger"))
		{
			currentAvailableActions.Remove(currentAvailableActions.Find(aa => aa._action == PossibleAction.Action_Talk));
		}
	}

	public void Apprehend()
	{
		isBeingApprehended = true;
	}
	private void HandleOnSaveGame (string _ID)
	{
		BinarySerialization.SaveToPlayerPrefs(_ID + SavingKeysContainer.PLAYER_POSITION, new MySerializables.V3(transform.position));
		BinarySerialization.SaveToPlayerPrefs(_ID + SavingKeysContainer.PLAYER_ROTATION, new MySerializables.Quat(transform.rotation));
		BinarySerialization.SaveToPlayerPrefs(_ID + SavingKeysContainer.PLAYER_NAME, playerName);
		BinarySerialization.SaveToPlayerPrefs(_ID + SavingKeysContainer.PLAYER_CANMOVE, canMove);
		BinarySerialization.SaveToPlayerPrefs(_ID + SavingKeysContainer.PLAYER_CAMERA_CAMPOINT, myCamera.GetComponent<CameraScript>().curCamPoint.GetComponent<CameraPoint>().localID);
	}
	
	private void HandleOnLoadGame (string _ID)
	{
		transform.position = ((MySerializables.V3)BinarySerialization.LoadFromPlayerPrefs(_ID + SavingKeysContainer.PLAYER_POSITION))._vector;
		transform.rotation = ((MySerializables.Quat)BinarySerialization.LoadFromPlayerPrefs(_ID + SavingKeysContainer.PLAYER_ROTATION))._quaternion;
		playerName = (string)BinarySerialization.LoadFromPlayerPrefs(_ID + SavingKeysContainer.PLAYER_NAME);
		canMove = (bool)BinarySerialization.LoadFromPlayerPrefs(_ID + SavingKeysContainer.PLAYER_CANMOVE);
		
		GameObject savedCamPoint = Manager_AI.Instance.GetSavedCamPoint((int)BinarySerialization.LoadFromPlayerPrefs(_ID + SavingKeysContainer.PLAYER_CAMERA_CAMPOINT));
		myCamera.GetComponent<CameraScript>().MoveTransition(savedCamPoint);
		savedCamPoint.GetComponent<CameraPoint>().isActive = true;
	}
}
