using UnityEngine;
using System.Collections;

public static class AdrienUtils 
{
	public static int[] ConvertToTime(float _timeInSeconds)
	{
		int[] timeValue = new int[3];
		float seconds = _timeInSeconds;

		while (seconds >= 3600.0f) 
		{
			timeValue[0] += 1;
			seconds -= 3600.0f;
		}

		while(seconds >= 60.0f)
		{
			timeValue[1] += 1;
			seconds -= 60.0f;
		}

		timeValue [2] = Mathf.RoundToInt (seconds);

		return timeValue;
	}
}
