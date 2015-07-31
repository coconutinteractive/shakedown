using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Utilities
{
	// Looks for gender text formatted as {male/female} and replaces with either male or female only text.
	static public string ParseStringForGender(string unparsedText, string gender)
	{
		int i = 0;
		for (i = 0; i < unparsedText.Length; i++)
		{
			if (unparsedText.Substring(i, 1) == "{")
			{
				int j;
				int indexOfBreak = 0;
				for (j = i+1; j < unparsedText.Length; j++)
				{
					if (unparsedText.Substring(j, 1) == "/")
					{
						indexOfBreak = j;
					}
					else if (unparsedText.Substring(j, 1) == "}")
					{
						break;
					}
				}
				if (indexOfBreak > 0)
				{
					string newText = "";
					newText += unparsedText.Substring (0, i);
					if(gender == Resources_Master.GENDER_MALE)
					{
						newText += unparsedText.Substring (i+1, indexOfBreak - (i+1));
					}
					else if(gender == Resources_Master.GENDER_FEMALE)
					{
						newText += unparsedText.Substring (indexOfBreak+1, j - (indexOfBreak+1));
					}
					newText += unparsedText.Substring (j+1, unparsedText.Length - (j+1));
					unparsedText = Utilities.ParseStringForGender(newText, gender);
				}
			}
		}
		return unparsedText;
	}

	static public Material GetMaterialFromID(string id)
	{
		return Resources.Load(id, typeof(Material)) as Material;
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
