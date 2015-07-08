using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour 
{
	public enum PossibleAction
	{
		Action_EnterShop,
		Action_TurnCorner,
		Action_CrossStreet,
		Action_Talk,

		Action_None
	}

	[SerializeField] private PossibleAction currentPossibleAction = PossibleAction.Action_None;

	[SerializeField] private float moveSpeed = 0.0f, turnSpeed = 0.0f;
	private Rigidbody myRigidbody = null;
	private Camera myCamera = null;

	private Vector3 facingRightVec = Vector3.zero, facingUpVec = Vector3.zero;

	private void Start()
	{
		myRigidbody = GetComponent<Rigidbody> ();
		myCamera = Camera.main;
		facingRightVec = transform.eulerAngles;
		facingUpVec = transform.eulerAngles - new Vector3 (0.0f, 90.0f, 0.0f);
	}

	private void FixedUpdate()
	{
		myRigidbody.velocity = transform.TransformDirection(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"))) * moveSpeed;
		Debug.Log ("HAHA! I'm hiding this here Adrien!");
	}

	private void OnTriggerEnter(Collider c)
	{
		if (c.CompareTag ("Corner Trigger")) 
		{
			currentPossibleAction = PossibleAction.Action_TurnCorner;
		}
		if(c.CompareTag("Door Trigger"))
	    {
			currentPossibleAction = PossibleAction.Action_EnterShop;
		}
		if(c.CompareTag("Cross Street Trigger"))
		{
			currentPossibleAction = PossibleAction.Action_CrossStreet;
		}
		if(c.CompareTag("Talk Trigger"))
		{
			currentPossibleAction = PossibleAction.Action_Talk;
		}
	}

	private void OnTriggerExit(Collider c)
	{
		if (c.CompareTag ("Corner Trigger")) 
		{
			currentPossibleAction = PossibleAction.Action_TurnCorner;
		}
		if(c.CompareTag("Door Trigger"))
		{
			currentPossibleAction = PossibleAction.Action_EnterShop;
		}
		if(c.CompareTag("Cross Street Trigger"))
		{
			currentPossibleAction = PossibleAction.Action_CrossStreet;
		}
		if(c.CompareTag("Talk Trigger"))
		{
			currentPossibleAction = PossibleAction.Action_Talk;
		}
	}
}
