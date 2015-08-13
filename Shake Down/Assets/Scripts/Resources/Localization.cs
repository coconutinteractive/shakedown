using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Reflection;

public class Localization : MonoBehaviour
{
	static private Enums.Language _currentLanguage = Enums.Language.englishus;
	static public Enums.Language currentLanguage { get { return _currentLanguage; } set { _currentLanguage = value; } }

	private const string FLAG_START = "{";
	private const string FLAG_END = "}";
	private const string FLAG_DIVISION = "/";
	private const string FLAG_HEADER = ":";

	static public string LocalizeText (string key)
	{		
		string basicText = Manager_StaticData.getLocalizationTextFromKey(key, currentLanguage);
		string parsedText = ParseString(basicText, new List<Resources_Character>(), new List<string>());
		return parsedText;
	}

	static public string LocalizeText (string key, List<Resources_Character> subjectPeople)
	{
		string basicText = Manager_StaticData.getLocalizationTextFromKey(key, currentLanguage);
		string parsedText = ParseString(basicText, subjectPeople, new List<string>());
		return parsedText;
	}

	static public string LocalizeText (string key, List<Resources_Character> subjectPeople, List<string> parameters)
	{
		parameters = parameters ?? new List<string>();
		
		string basicText = Manager_StaticData.getLocalizationTextFromKey(key, currentLanguage);
		string parsedText = ParseString(basicText, subjectPeople, parameters);
		return parsedText;
	}

	static public string LocalizeText (string key, List<string> parameters)
	{
		string basicText = Manager_StaticData.getLocalizationTextFromKey(key, currentLanguage);
		string parsedText = ParseString(basicText, new List<Resources_Character>(), parameters);
		return parsedText;
	}


	// Parses a string for the following formats, indicating a replacement needing to be made:
	// GENDER: {0:male/female} - where '0' is the subject who's gender determines the content, and 'male' and 'female' are the content options
	// CHARACTER STAT: {0:stat} - where '0' is the subject who's stats are being referende, and 'stat' is the character stat that is being returned
	// PARAMETER: {0} - where '0' is the index of a predetermined list of parameters that have been passed in when the method is called.
	static public string ParseString(string unparsedText, List<Resources_Character> subjectPeople, List<string> parameters = null)
	{
		//For debug use only
		parameters = parameters ?? new List<string>();
		subjectPeople = subjectPeople ?? new List<Resources_Character>();

		// In this method, -1 is treated as an "undefined int"
		int indexOfStart = -1;
		int indexOfId = -1;
		int indexOfDivision = -1;
		int indexOfEnd = -1;
		bool idIdentified = false;

		string newText = "";
		int newTextIndex = 0;

		for (int i = 0; i < unparsedText.Length; i++)
		{
			string charAtI = unparsedText.Substring(i,1);
			if (charAtI == FLAG_START) {
				indexOfStart 		= i;
				indexOfId 			= -1;
				indexOfDivision 	= -1;
				indexOfEnd 			= -1;
				idIdentified 		= false;
			}
			else if (charAtI == FLAG_HEADER && 
			         indexOfStart >= 0 &&
			         !idIdentified) {
				indexOfId 			= i;
				indexOfDivision 	= -1;
				indexOfEnd 			= -1;
				idIdentified 		= true;
			}
			else if (charAtI == FLAG_HEADER &&
			         indexOfId >= 0) {
				indexOfDivision 	= -1;
				indexOfEnd 			= -1;
			}
			else if (charAtI == FLAG_DIVISION &&
			         indexOfId >= 0) {
				indexOfDivision 	= i;
				indexOfEnd 			= -1;
			}
			else if (charAtI == FLAG_END) {
				indexOfEnd = i;
				newText += unparsedText.Substring (newTextIndex, indexOfStart - newTextIndex);
				newTextIndex = indexOfStart - 1;

				if(indexOfStart >= 0 ) {
					string flaggedText = unparsedText.Substring(indexOfStart + 1, indexOfEnd - indexOfStart - 1);
					if(indexOfId >= 0) {
						// Having found an index of ID means it's either a gender or a stat
						string idText = unparsedText.Substring(indexOfStart + 1, indexOfId - indexOfStart - 1);
						int id = -1;
						if (int.TryParse(idText, out id)) {
							if(id < subjectPeople.Count) {
								if(indexOfDivision >= 0) {
									// Having found a division means it's a gender flag
									newText += ReplaceFlagsWithGender (flaggedText, indexOfId - (indexOfStart + 1), indexOfDivision - (indexOfStart + 1), subjectPeople[id].gender);
								}
								else {
									// Having no division means it's a stat flag
									// TODO:
									newText += ReplaceFlagsWithStat (flaggedText, indexOfId - (indexOfStart + 1), subjectPeople[id]);
								}
							}
							else {
								// There is no character entry for the listed subject, so keep the entirety of it and move on
								Debug.LogError ("Subject number " + id + " in '" + flaggedText + "'. Make sure to pass in a reference to the person you are referring to.");
								newText += unparsedText.Substring(indexOfStart, indexOfEnd - indexOfStart + 1);
							}
						}
						else {
							// ID section is not formatted as a number, so keep the entirety of it and move on.
							Debug.LogError ("The id '" + idText + "' in '" + flaggedText + "' is not formatted as a number.");
							newText += unparsedText.Substring(indexOfStart, indexOfEnd - indexOfStart + 1);
						}
					}
					else {
						// Having no index of ID means it's a parameter flag
						// TODO:
						newText += ReplaceFlagsWithParameter (flaggedText, parameters); 
					}
				}
				else {
					// Flagged section is not properly formatted, so keep the entirety of it and move on.
					Debug.LogError ("the '" + FLAG_END + "' in '" + unparsedText + "' does not line up to any '" + FLAG_START + "'.");
					newText += unparsedText.Substring(indexOfStart, indexOfEnd - indexOfStart + 1);
				}
				// Reset all flags to start looking for the next section
				newTextIndex = indexOfEnd + 1;

				indexOfStart = -1;
				indexOfId = -1;
				indexOfDivision = -1;
				indexOfEnd = -1;
				idIdentified = false;
			}
		}
		newText += unparsedText.Substring (newTextIndex, unparsedText.Length - newTextIndex);
		return newText;
	}
	
	static private string ReplaceFlagsWithGender(string text, int indexOfId, int indexOfDivision, Enums.Gender gender)
	{
		string maleText = text.Substring (indexOfId + 1, indexOfDivision - (indexOfId + 1));
		string femaleText = text.Substring (indexOfDivision + 1, text.Length - (indexOfDivision + 1));

		switch (gender)
		{
		case Enums.Gender.male:
			return maleText;
		case Enums.Gender.female:
			return femaleText;
		default:
			return "UNDEFINED";
		}
	}

	static private string ReplaceFlagsWithStat(string text, int indexOfId, Resources_Character subject)
	{
		string stat = text.Substring (indexOfId + 1, text.Length -(indexOfId + 1));

		PropertyInfo myPropertyInfo = typeof(Resources_Character).GetProperty(stat);
		if (myPropertyInfo.GetValue (subject, null) != null) {
			return myPropertyInfo.GetValue (subject, null).ToString();
		}
		else {
			Debug.LogError ("The subject '" + subject + "' does not have a stat for '" + stat +"'.");
			return text;
		}
	}

	static private string ReplaceFlagsWithParameter(string text, List<string> parameters)
	{
		int id;
		if(int.TryParse (text, out id)) {
			if(parameters[id] != null) {
				return parameters[id].ToString ();
			}
			else {
				Debug.LogError("There is no available parameter at index '" + id + "'. Make sure you are passing in a parameter that exists on this character.");
				return text;
			}
		}
		else {
			Debug.LogError("The index '" + id + "'. is not a valid parameter id. Make sure to pass in any referenced parameters");
			return text;
		}
	}
}