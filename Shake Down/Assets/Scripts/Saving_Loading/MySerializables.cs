using UnityEngine;
using System.Collections;

namespace AdrienSerializables
{

[System.Serializable]
public class MySerializables
{
	[System.Serializable]
	public class V3
	{
		public float x,y,z;

		public V3 (float x, float y, float z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		public V3 (Vector3 _v)
		{
			this.x = _v.x;
			this.y = _v.y;
			this.z = _v.z;
		}

		public Vector3 _vector {get{return new Vector3(x,y,z);}}
	}

	[System.Serializable]
	public class Quat
	{
		public float x,y,z,w;
		
		public Quat (float x, float y, float z, float w)
		{
			this.x = x;
			this.y = y;
			this.z = z;
			this.w = w;
		}
		
		public Quat (Quaternion _quat)
		{
			this.x = _quat.x;
			this.y = _quat.y;
			this.z = _quat.z;
			this.w = _quat.w;
		}
		
		public Quaternion _quaternion {get{return new Quaternion(x,y,z,w);}}
	}

	[System.Serializable]
	public class GameTime
	{
		public string currentDayState, currentDayOfTheWeek;
		public float dayTimer, timeToAdd;
		public int dayCount;
		public bool isAM;

		public GameTime (string currentDayState, string currentDayOfTheWeek, float dayTimer, float timeToAdd, int dayCount, bool isAM)
		{
			this.currentDayState = currentDayState;
			this.currentDayOfTheWeek = currentDayOfTheWeek;
			this.dayTimer = dayTimer;
			this.timeToAdd = timeToAdd;
			this.dayCount = dayCount;
			this.isAM = isAM;
		}

//		public GameTime _gameTime{get{return this;}}
	}

}
}
