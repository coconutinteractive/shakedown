using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Resources_Master
{
	public const string GENDER_MALE = "gender_male";
	public const string GENDER_FEMALE = "gender_female";

	private string _referenceID;
	public string referenceID { get { return _referenceID; } }
	
	private string _gender;
	public string gender { get { return _gender; } }

	private string _profileImage;
	public string profileImage { get { return _profileImage; } }

	private string _homeID;
	public string homeID { get { return _homeID; } }

	public Resources_Master(string newID, string gender, string buildingID, string profileImageID = "DefaultFace")
	{
		_gender = gender;
		_referenceID = newID;
		_homeID = buildingID;
		_profileImage = profileImageID;
	}
	
	protected void DeathByLackOfResources(Resources_Master script)
	{
		Debug.LogError (script.referenceID + " is Dead.");
	}
}
