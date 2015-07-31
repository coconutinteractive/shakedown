using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameChart_Shopkeeper : MonoBehaviour
{
	#region Singleton
	public static GameChart_Shopkeeper Instance 
	{
		get 
		{
			if(instance == null)
				instance = FindObjectOfType(typeof(GameChart_Shopkeeper)) as GameChart_Shopkeeper;
			
			return instance;
		}
		set 
		{
			instance = value;
		}
	}
	private static GameChart_Shopkeeper instance;
	#endregion

	public enum Areas
	{
		Normal,
		Disdain,
		Neutral,
		Terror,
		Admiration, 
		Awe
	}

	[System.Serializable]
	public struct RangeArea
	{
		public string name;
		public Areas areaType;
		public int startFear, endFear, startRespect, endRespect;
	}

	[SerializeField] private List<RangeArea> specificAreas = new List<RangeArea>(); 
	public int maxFearValue = 100, maxRespectValue = 100;

	public Areas CheckForAreas(int _fear, int _respect)
	{
		foreach (RangeArea curArea in specificAreas) 
		{
			if(_fear > curArea.startFear && _fear < curArea.endFear)
			{
				if(_respect > curArea.startRespect && _respect < curArea.endRespect)
					return curArea.areaType;
			}
		}
		return Areas.Normal;
	}
	
	public int[] GetPosInArea(Areas _area)
	{
		RangeArea area = specificAreas.Find (sa => sa.areaType == _area);
		return new int[2]{Random.Range (area.startFear, area.endFear + 1), Random.Range (area.startRespect, area.endRespect + 1)};
	}
}

