using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class NightActivities : MonoBehaviour
{
	[System.Serializable]
	public class NightActivity
	{
		public NightActivity(){}

		public string name = "";
		public float duration = 0.0f;
		public float availabilityStart = 0.0f, AvailabilityEnd = 0.0f;
		public bool isAvailable = true;
	}

	public NightActivity casino, restaurant, bar, gym;

	public List<NightActivity> nightActivitiesList = new List<NightActivity>();

	private void Awake()
	{
		nightActivitiesList.Clear ();
		nightActivitiesList.Add (casino);
		nightActivitiesList.Add (restaurant);
		nightActivitiesList.Add (bar);
		nightActivitiesList.Add (gym);
	}
}

