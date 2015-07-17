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

	private void Start()
	{
		myRigidbody = GetComponent<Rigidbody> ();
		StartCoroutine (Accelerate (maxSpeed));
	}

	private void FixedUpdate()
	{
		myRigidbody.velocity = transform.right * currentSpeed;
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

	private IEnumerator TurnCorner(GameObject _turnTriggerObj)
	{
		yield return new WaitForSeconds (/*UnityEngine.Random.Range (0.25f, 0.35f)*/ 0.4f);
	
		GameObject newCamPoint = _turnTriggerObj.GetComponent<StreetCornerTrigger>().GetNewCamPoint(_currentCamPoint);
		_currentCamPoint = newCamPoint;
		myRotation = transform.rotation;

		float timer = 0.0f;
		//while(Quaternion.Angle(myRotation, newCamPoint.transform.rotation) > 1.0f)
		//while(newCamPoint.transform.rotation.y - myRotation.y)
		while(timer < 1.5f)
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

		yield return null;
	}

	private IEnumerator Accelerate(float _targetVel)
	{
		while(currentSpeed < _targetVel - _targetVel * 0.05f)
		{
			currentSpeed = Mathf.Lerp(currentSpeed, _targetVel, accelerationTime * Time.deltaTime);
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
		while(timer < 0.75f)
		{
			timer += Time.deltaTime;
			currentSpeed = Mathf.Lerp(currentSpeed, _targetVel, accelerationTime * Time.deltaTime * 7.0f);
			yield return null;
		}
		currentSpeed = _targetVel;
		if (currentSpeed <= 0.0f)
			currentSpeed = 0.0f;
		
		yield return null;
	}
}
