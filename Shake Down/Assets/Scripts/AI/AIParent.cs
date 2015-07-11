using UnityEngine;
using System.Collections;

public class AIParent : MonoBehaviour 
{
	[SerializeField] protected float timeToDespawn = 0.0f;
	[SerializeField] protected int triggersHitAmount = 0;
	[SerializeField] protected float moveSpeed = 0.0f;
	[SerializeField] protected float triggerRefusalChance = 0.0f;
	[SerializeField] protected int shoppingAmount = 0;
	[SerializeField] protected float shoppingChance = 0.0f;
	[SerializeField] protected float shoppingTime = 0.0f;
	[SerializeField] protected float sleepingTime = 0.0f;

	protected int direction = 1;
	protected bool canMove = true;
	protected GameObject currentCamPoint = null;
	protected float currentTimeToDespawn = 0.0f;
	protected int currentShoppingAmount = 0;

	virtual protected void Start()
	{
		currentTimeToDespawn = timeToDespawn;
	}

	virtual protected void Update()
	{
		if(canMove)
		{
			transform.position += (transform.right * moveSpeed * Time.deltaTime) * direction;
			currentTimeToDespawn -= Time.deltaTime;
		}
	}

	virtual protected void OnTriggerEnter(Collider c)
	{
		if(c.CompareTag("Corner Trigger"))
		{
			//Turn Corner
			--triggersHitAmount;
		}
		if(c.CompareTag("Door Trigger"))
		{
			//random chance to enter shop
			if(currentShoppingAmount < 1 || triggersHitAmount <= 0)
				return;

			--triggersHitAmount;
			canMove = false;
			if(Random.Range(0, 101) < shoppingChance)
			{
				//enter Shop
				StartCoroutine(EnterBuilding(c, false));
				--currentShoppingAmount;
			}
		}
		if(c.CompareTag("Cross Street Trigger"))
		{
			//random chance to cross street
			--triggersHitAmount;
		}
		if(c.CompareTag("House Trigger"))
		{
			//check if time to go back home
			--triggersHitAmount;
			if(triggersHitAmount <= 0 || timeToDespawn <= 0.0f)
			{
				canMove = false;
				StartCoroutine(EnterBuilding(c, true));
			}
		}
	}

	virtual protected IEnumerator EnterBuilding(Collider _trigger, bool _isHome)
	{
		Vector3 targetVec = _trigger.gameObject.transform.position + _trigger.gameObject.transform.forward * 3.0f;
		targetVec.y = transform.position.y;
		
		while(Vector3.Distance(transform.position, targetVec) > 0.15f)
		{
			transform.position = Vector3.Lerp(transform.position, targetVec, Time.deltaTime * moveSpeed * 0.45f);
			yield return null;
		}

		if (_isHome)
			yield return new WaitForSeconds (shoppingTime + Random.Range (-shoppingTime * 0.5f, shoppingTime * 0.5f));
		else
			yield return new WaitForSeconds (sleepingTime);

		StartCoroutine (ExitBuilding (_trigger, _isHome));

		yield return null;
	}

	virtual protected IEnumerator ExitBuilding(Collider _trigger, bool _isHome)
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
			currentTimeToDespawn = timeToDespawn;
			currentShoppingAmount = shoppingAmount;
		}

		direction = RandomDirection ();
		canMove = true;
		
		yield return null;
	}

	virtual protected int RandomDirection()
	{
		if (Random.Range (1, 3) == 1)
			return 1;

		return -1;
	}
}