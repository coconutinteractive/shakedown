using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Utilities
{
	static private int _difficultyHandicap = 25;
	static public int difficultyHandicap { get { return _difficultyHandicap; } }

	static public Material GetMaterialFromID(string id)
	{
		return Resources.Load(id, typeof(Material)) as Material;
	}

	static public Sprite GetSpriteFromID(string id)
	{
		return Resources.Load(id, typeof(Sprite)) as Sprite;
	}

	static public bool FoundStringInList (string id, List<string> list)
	{
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i] == id) return true;
		}
		return false;
	}
}