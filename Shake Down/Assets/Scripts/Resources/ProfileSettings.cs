using UnityEngine;
using System.Collections;

public class ProfileSettings
{
	private string _id = "player";
	private string _name;
	private string _image;
	private Enums.Gender _gender;

	public string id { get { return _id; } }
	public string name { set { _name = value; } get { return _name; } }
	public string image { set { _image = value; } get { return _image; } }
	public Enums.Gender gender { set { _gender = value; } get { return _gender; } }
}