using UnityEngine;
using System.Collections;
using System;


public class Manager_GameTime : MonoBehaviour
{
	#region Singleton
	public static Manager_GameTime Instance 
	{
		get 
		{
			if(instance == null)
				instance = FindObjectOfType(typeof(Manager_GameTime)) as Manager_GameTime;
			
			return instance;
		}
		set 
		{
			instance = value;
		}
	}
	private static Manager_GameTime instance;
	#endregion

	public static event Action OnMorningEvent, OnAfternoonEvent, OnEveningEvent, OnNightEvent;

	[System.Serializable]
	public struct DayStateClass
	{

	}

	[SerializeField] private DayStateClass morning, afternoon, evening, night;

	private float dayTimer = 0.0f;
	[SerializeField] private float timeMultiplier = 0.0f;
	private int[] timeInHours = new int[3];
	private bool isPaused = false;

	private float timeToAdd = 0.0f;
	private int[] timeToAddInHours = new int[3];

	private void Update()
	{
		if(!isPaused)
		{

			dayTimer += Time.deltaTime * timeMultiplier;
			if (dayTimer >= 86400.0f)
				dayTimer = 0.0f;
		}
		timeInHours = AdrienUtils.ConvertToTime (dayTimer);
		if(timeToAdd > 0.0f)
		{
			timeToAddInHours = AdrienUtils.ConvertToTime(timeToAdd);
			timeToAdd -= Time.deltaTime * timeMultiplier * 2.0f;
		}
	}

	private void OnGUI()
	{
		GUI.Box(new Rect(35.0f, 35.0f, 100.0f, 25.0f), timeInHours[0].ToString() + "h:" + timeInHours[1].ToString() + "m:" + timeInHours[2].ToString() + "s");
		if(timeToAdd > 0.0f)
		{
			GUI.Label(new Rect(35.0f, 55.0f, 100.0f, 25.0f), timeToAdd)
		}
	}

	public void PauseGameTime(bool _value)
	{
		isPaused = _value;
	}

	public void TimeLeap(float _value)
	{
		dayTimer += 
	}

	public void TimeLeap(int _hours, int _mins, int _seconds)
	{
		
	}

}
