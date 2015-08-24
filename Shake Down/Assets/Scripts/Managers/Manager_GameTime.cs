using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using AdrienSerializables;

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
	
	public static event Action<DayStateClass> OnChangeDayState;
	
	private static float FULL_DAY_IN_SECONDS = 86400.0f;
	private static float HALF_DAY_IN_SECONDS = 43200.0f;
	
	[SerializeField] private float endOfDay = 0.0f, startOfDay = 0.0f;
	
	[System.Serializable]
	public struct DayStateClass
	{
		public string name;
		public float exitThreshold;
		public int maxCarAmount;
		public int maxCitizenAmount;
	}
	
	[SerializeField] private DayStateClass morning, afternoon, evening, night;
	
	private float dayTimer = 0.0f;
	[SerializeField] private float timeMultiplier = 0.0f;
	private int[] timeInHours = new int[3];
	private bool isPaused = false;
	private bool isAM = true;
	private bool hasDayStarted = false, hasDayEnded = false;
	private bool isRestActive = true;
	
	private float timeToAdd = 0.0f;
	private int[] timeToAddInHours = new int[3];
	private int elapsedDays = 1;
	private string currentDayState = "Night";
	private Enums.DayOfTheWeek _currentDayOfTheWeek = Enums.DayOfTheWeek.Sun;
	public Enums.DayOfTheWeek currentDayOfTheWeek { get { return _currentDayOfTheWeek; } }
	
	private List<NightActivities.NightActivity> activitiesList = new List<NightActivities.NightActivity>();
	
	private void Start()
	{
		InvokeRepeating ("CheckForTimeOfDay", 0.25f, 0.15f);
		SavingKeysContainer.OnSaveGame += HandleOnSaveGame;
		SavingKeysContainer.OnLoadGame += HandleOnLoadGame;
		
		activitiesList = GetComponent<NightActivities> ().nightActivitiesList;
	}
	
	private void HandleOnSaveGame (string _ID)
	{
		MySerializables.GameTime gameTime = new MySerializables.GameTime (currentDayState, currentDayOfTheWeek.ToString(), dayTimer, timeToAdd, elapsedDays, isAM);
		BinarySerialization.SaveToPlayerPrefs (_ID + SavingKeysContainer.GAME_TIME_DATA, gameTime);
	}
	
	private void HandleOnLoadGame (string _ID)
	{
		MySerializables.GameTime gameTime = (MySerializables.GameTime)BinarySerialization.LoadFromPlayerPrefs (_ID + SavingKeysContainer.GAME_TIME_DATA);
		currentDayState = gameTime.currentDayState;
		_currentDayOfTheWeek = (Enums.DayOfTheWeek)Enum.Parse (typeof(Enums.DayOfTheWeek), gameTime.currentDayOfTheWeek);

		dayTimer = gameTime.dayTimer;
		timeToAdd = gameTime.timeToAdd;
		elapsedDays = gameTime.dayCount;
		isAM = gameTime.isAM;
	}
	
	private void CheckForTimeOfDay()
	{
		if(dayTimer >= startOfDay && !hasDayStarted)
		{
			hasDayStarted = true;
			isRestActive = false;
		}
		if(dayTimer >= endOfDay && !hasDayEnded)
		{
			hasDayEnded = true;
			isRestActive = true;
		}
		
		if(dayTimer >= night.exitThreshold && currentDayState != morning.name && dayTimer <= morning.exitThreshold)
		{
			currentDayState = morning.name;
			if(OnChangeDayState != null)
				OnChangeDayState(morning);
			
			int newDayIndex = (int)currentDayOfTheWeek + 1;
			if(newDayIndex > 6)
				newDayIndex = 0;
			_currentDayOfTheWeek = (Enums.DayOfTheWeek)newDayIndex;
		}
		if(dayTimer >= morning.exitThreshold && currentDayState != afternoon.name)
		{
			currentDayState = afternoon.name;
			if(OnChangeDayState != null)
				OnChangeDayState(afternoon);
		}
		if(dayTimer >= afternoon.exitThreshold && currentDayState != evening.name)
		{
			currentDayState = evening.name;
			if(OnChangeDayState != null)
				OnChangeDayState(evening);
		}
		if(dayTimer >= evening.exitThreshold && currentDayState != night.name)
		{
			currentDayState = night.name;
			if(OnChangeDayState != null)
				OnChangeDayState(night);
		}
	}
	
	private void Update()
	{
		if(!isPaused)
		{
			dayTimer += Time.deltaTime * timeMultiplier;
			if (dayTimer >= HALF_DAY_IN_SECONDS && isAM)
			{
				isAM = false;
			}
			if(dayTimer >= FULL_DAY_IN_SECONDS && !isAM)
			{
				hasDayEnded = false;
				hasDayStarted = false;
				isAM = true;
				dayTimer = 0.0f;
				++elapsedDays;
				
			}
		}
		timeInHours = AdrienUtils.ConvertToTime (dayTimer);
		if(timeToAdd > 0.0f)
		{
			timeToAddInHours = AdrienUtils.ConvertToTime(timeToAdd);
			timeToAdd -= Time.deltaTime * 7200.0f;
			dayTimer += Time.deltaTime * 7200.0f;
		}
		if (timeToAdd < 0.0f)
			timeToAdd = 0.0f;
		if(timeToAdd == 0.0f && isPaused)
			isPaused = false;
	}
	private void OnGUI()
	{
		if(isAM)
			GUI.Box(new Rect(35.0f, 35.0f, 165.0f, 25.0f), "Day " + elapsedDays.ToString() + " -- " + timeInHours[0].ToString() + "h:" + timeInHours[1].ToString() + "m:" + timeInHours[2].ToString() + "s" + (isAM ? " AM" : " PM"));
		else
			GUI.Box(new Rect(35.0f, 35.0f, 165.0f, 25.0f), "Day " + elapsedDays.ToString() + " -- " + (timeInHours[0] - 12).ToString() + "h:" + timeInHours[1].ToString() + "m:" + timeInHours[2].ToString() + "s" + (isAM ? " AM" : " PM"));
		
		GUI.Box (new Rect (220.0f, 35.0f, 100.0f, 25.0f), currentDayOfTheWeek + " " + currentDayState);
		
		if(isRestActive)
		{
			GUI.Box (new Rect (35.0f, 75.0f, 285.0f, 500.0f), "END OF DAY");
			
			float timeToRest = 0.0f;
			if(FULL_DAY_IN_SECONDS - dayTimer < FULL_DAY_IN_SECONDS - endOfDay)
				timeToRest = FULL_DAY_IN_SECONDS - dayTimer + startOfDay;
			else
				timeToRest = startOfDay - dayTimer;
			
			int[] timeToRestInTime = AdrienUtils.ConvertToTime(timeToRest);
			
			if(GUI.Button(new Rect(57.5f, 100.0f, 240.0f, 60.0f), "Rest" + "\n" + "\n" + "(" + timeToRestInTime[0] + "h:" + timeToRestInTime[1] + "m" + ")"))
			{
				isRestActive = false;
				TimeLeap(timeToRest);
			}
			
			GUI.Label(new Rect(150.0f, 175.0f, 100.0f, 25.0f), "Activities");
			
			for (int i = 0; i < activitiesList.Count; ++i) 
			{
				
				// CHECK FOR ACTIVITY AVAILABILITY
				
				if(activitiesList[i].availabilityStart < HALF_DAY_IN_SECONDS)
				{
					if(activitiesList[i].availabilityStart > dayTimer || activitiesList[i].AvailabilityEnd < dayTimer)
					{
						GUI.color = Color.gray;
						activitiesList[i].isAvailable = false;
					}
				}
				else if(activitiesList[i].availabilityStart > HALF_DAY_IN_SECONDS)
				{
					if(dayTimer > HALF_DAY_IN_SECONDS)
					{
						if(activitiesList[i].availabilityStart > dayTimer)
						{
							GUI.color = Color.gray;
							activitiesList[i].isAvailable = false;
						}
						if(activitiesList[i].AvailabilityEnd > HALF_DAY_IN_SECONDS)
						{
							if(activitiesList[i].AvailabilityEnd < dayTimer)
							{
								GUI.color = Color.gray;
								activitiesList[i].isAvailable = false;
							}
						}
					}
					else
					{
						if(activitiesList[i].AvailabilityEnd > HALF_DAY_IN_SECONDS)
						{
							GUI.color = Color.gray;
							activitiesList[i].isAvailable = false;
						}
						else if(activitiesList[i].AvailabilityEnd < HALF_DAY_IN_SECONDS)
						{
							if(activitiesList[i].AvailabilityEnd < dayTimer)
							{
								GUI.color = Color.gray;
								activitiesList[i].isAvailable = false;
							}
						}
					}
				}
				
				int[] durationInTime = AdrienUtils.ConvertToTime(activitiesList[i].duration);
				int[] startInTime = AdrienUtils.ConvertToTime(activitiesList[i].availabilityStart);
				int[] endInTime = AdrienUtils.ConvertToTime(activitiesList[i].AvailabilityEnd);
				
				string startAmPm = "AM";
				if(startInTime[0] > AdrienUtils.ConvertToTime(startOfDay)[0] + 1)
				{
					startAmPm = "PM";
					startInTime[0] -= 12;
				}
				string endAmPm = "AM";
				if(endInTime[0] > AdrienUtils.ConvertToTime(startOfDay)[0] + 1)
				{
					endAmPm = "PM";
					endInTime[0] -= 12;
				}
				
				if(GUI.Button(new Rect(75.0f, 220.0f + (i * 75.0f), 200.0f, 55.0f), activitiesList[i].name + "\n" + "(" + durationInTime[0] + "h:" + durationInTime[1] + "m" + ")" 
				              + "\n" + startInTime[0] + startAmPm + "  ~  " + endInTime[0] + endAmPm))
				{
					if(activitiesList[i].isAvailable)
						TimeLeap(activitiesList[i].duration);
				}
				
				GUI.color = Color.white;
				activitiesList[i].isAvailable = true;
			}
		}
		
		if(timeToAdd > 0.0f)
		{
			int[] timeToAddInHours = AdrienUtils.ConvertToTime(timeToAdd);
			GUI.Label(new Rect(55.0f, 15.0f, 100.0f, 25.0f), timeToAddInHours[0].ToString() + "h:" + timeToAddInHours[1].ToString() + "m:" + timeToAddInHours[2].ToString() + "s");
		}
	}
	
	public void PauseGameTime(bool _value)
	{
		isPaused = _value;
	}
	
	public void TimeLeap(float _value)
	{
		timeToAdd += _value;
	}
	
	public void TimeLeap(int _hours, int _mins, int _seconds)
	{
		isPaused = true;
		timeToAdd += _hours * 3600.0f + _mins * 60.0f + _seconds;
	}
	
	private void OnDestroy()
	{
		SavingKeysContainer.OnSaveGame -= HandleOnSaveGame;
	}
}