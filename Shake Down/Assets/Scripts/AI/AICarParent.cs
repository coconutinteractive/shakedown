using UnityEngine;
using System.Collections;

public class AICarParent : MonoBehaviour 
{
	[SerializeField] protected float maxSpeed = 0.0f, accelerationTime = 0.0f, turnSpeed = 0.0f;
	protected float currentSpeed = 0.0f;
	[SerializeField] protected float triggerRefusalChance = 0.0f;
	[SerializeField] protected GameObject _currentCamPoint = null;
	public GameObject currentCamPoint{get{return _currentCamPoint;}set{_currentCamPoint = value;}}
	protected Rigidbody myRigidbody = null;
	protected Quaternion myRotation;
	protected GameObject currentCrosswalk = null;
	protected CrosswalkTrigger currentCrosswalkScript = null;
	protected bool isTurning = false;
	protected bool cannotAccelerate = false;

	private void Start()
	{
		myRigidbody = GetComponent<Rigidbody> ();
		StartCoroutine (Accelerate (maxSpeed));
		InvokeRepeating ("LookForObstacles", 0.25f, 0.15f);
	}

	private void FixedUpdate()
	{
		myRigidbody.velocity = transform.right * currentSpeed;
	}

	private void LookForObstacles()
	{
		if (isTurning)
			return;

		//Debug.DrawLine (transform.position + transform.right, transform.position + transform.right * 5.0f, Color.cyan, 0.15f);
		RaycastHit[] hits = Physics.SphereCastAll(transform.position + transform.right, 1.0f, transform.right, 2.5f);
		if(hits.Length > 0)
		{
			foreach (RaycastHit hit in hits)
			{
				if(hit.collider.gameObject.CompareTag("Crosswalk Trigger"))
				{
					if(currentCrosswalk == hit.collider.gameObject)
						continue;

					if(currentCrosswalk == null || currentCrosswalk != hit.collider.gameObject)
						currentCrosswalk = hit.collider.gameObject;

					currentCrosswalkScript = currentCrosswalk.GetComponent<CrosswalkTrigger>();

					currentCrosswalkScript.OnGreenLight += HandleOnGreenLight;
					currentCrosswalkScript.OnOrangeLight += HandleOnOrangeLight;;
					currentCrosswalkScript.OnRedLight += HandleOnRedLight;

					if(currentCrosswalkScript.currentState == CrossWalk.CrosswalkState.CWS_Green || currentCrosswalkScript.currentState == CrossWalk.CrosswalkState.CWS_Orange)
					{
						StartCoroutine(Decelerate(0.0f));
						cannotAccelerate = true;
					}
				}
			}
		}
	}

	private void HandleOnGreenLight ()
	{
		if (currentCrosswalk)
		{
			if (!CheckDistance(currentCrosswalk.gameObject))
				return;
			cannotAccelerate = true;
			if(gameObject.activeInHierarchy)
				StartCoroutine(Decelerate(0.0f));

		}
	}
	private void HandleOnOrangeLight ()
	{
		if (currentCrosswalk)
		{
			if (!CheckDistance(currentCrosswalk.gameObject))
				return;
			cannotAccelerate = true;
			StartCoroutine(Decelerate(0.0f));
		}
	}
	private void HandleOnRedLight ()
	{
		/*if (!CheckDistance(currentCrosswalk.gameObject))
			return;*/
		cannotAccelerate = false;
		if(gameObject.activeInHierarchy)
			StartCoroutine (Accelerate (maxSpeed));
	}

	private bool CheckDistance(GameObject _object)
	{
		if (Vector3.Distance (transform.position, _object.transform.position) > 1.5f)
			return true;

		return false;
	}


	private void OnTriggerEnter(Collider c)
	{
		if(c.CompareTag("Street Corner Trigger"))
		{
			StartCoroutine(TurnCorner(c.gameObject));
			StartCoroutine(Decelerate(maxSpeed * 0.5f));
		}

		if(c.CompareTag("Street End Trigger"))
		{
			Manager_AI.Instance.MoveToCarObjectPool(gameObject);
		}
	}

	private void OnTriggerExit(Collider c)
	{
		if(c.CompareTag("Crosswalk Trigger"))
		{
			if(c.gameObject == currentCrosswalk)
			{
				currentCrosswalk.GetComponent<CrosswalkTrigger>().OnGreenLight -= HandleOnGreenLight;
				currentCrosswalk.GetComponent<CrosswalkTrigger>().OnOrangeLight -= HandleOnOrangeLight;;
				currentCrosswalk.GetComponent<CrosswalkTrigger>().OnRedLight -= HandleOnRedLight;
				currentCrosswalk = null;
				cannotAccelerate = false;
			}
		}
	}

	private IEnumerator TurnCorner(GameObject _turnTriggerObj)
	{
		isTurning = true;
		cannotAccelerate = true;
		yield return new WaitForSeconds (/*UnityEngine.Random.Range (0.25f, 0.35f)*/ 0.325f);
	
		GameObject newCamPoint = _turnTriggerObj.GetComponent<StreetCornerTrigger>().GetNewCamPoint(_currentCamPoint);
		_currentCamPoint = newCamPoint;
		myRotation = transform.rotation;

		float timer = 0.0f;
		//while(Quaternion.Angle(myRotation, newCamPoint.transform.rotation) > 1.0f)
		//while(newCamPoint.transform.rotation.y - myRotation.y)
		while(timer < 1.2f)
		{
			timer += Time.deltaTime;
			myRotation = Quaternion.Slerp (myRotation, newCamPoint.transform.rotation, Time.deltaTime * turnSpeed);
			myRotation.x = 0.0f;
			myRotation.z = 0.0f;
			transform.rotation = myRotation;
			yield return null;
		}

		myRotation = newCamPoint.transform.rotation;
		myRotation.x = 0.0f;
		myRotation.z = 0.0f;
		transform.rotation = myRotation;
		isTurning = false;
		cannotAccelerate = false;

		yield return null;
	}

	private IEnumerator Accelerate(float _targetVel)
	{
		while(currentSpeed < _targetVel - _targetVel * 0.05f)
		{
			if(!cannotAccelerate)
			{
				currentSpeed = Mathf.Lerp(currentSpeed, _targetVel, accelerationTime * Time.deltaTime);
			}
			yield return null;
		}
		currentSpeed = _targetVel;
		if (currentSpeed >= maxSpeed)
			currentSpeed = maxSpeed;

		yield return null;
	}

	private IEnumerator Decelerate(float _targetVel)
	{
		float timer = 0.0f;
		while(timer < 1.0f)
		{
			timer += Time.deltaTime;
			currentSpeed = Mathf.Lerp(currentSpeed, _targetVel, accelerationTime * Time.deltaTime * 7.0f);
			yield return null;
		}
		currentSpeed = _targetVel;
		if (currentSpeed <= 0.0f)
			currentSpeed = 0.0f;

		if(currentCrosswalk)
		{
			if(currentCrosswalk.GetComponent<CrosswalkTrigger>().currentState == CrossWalk.CrosswalkState.CWS_Red)
				cannotAccelerate = false;
		}
		
		yield return null;
	}
}
