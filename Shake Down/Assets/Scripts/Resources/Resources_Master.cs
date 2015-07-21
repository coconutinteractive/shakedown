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

	private Material _profileImage;
	public Material profileImage { get { return _profileImage; } }

	private string _homeID;
	public string homeID { get { return _homeID; } }

	public Resources_Master(string newID, string gender, string buildingID, string profileImageID = "DefaultFace")
	{
		Debug.Log ("Fixin' Stuff!");
		_gender = gender;
		_referenceID = newID;
		_homeID = buildingID;
		_profileImage = Utilities.GetMaterialFromID(profileImageID);
	}
	
	protected void DeathByLackOfResources(Resources_Master script)
	{
		Debug.LogError (script.referenceID + " is Dead.");
	}
}
