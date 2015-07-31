using UnityEngine;
using System.Collections;

public class Resources_Character : Resources_Root
{
	private Enums.Gender _gender;
	private Resources_Building _home;
	private int _strength;

	public Enums.Gender gender { get { return _gender; } }
	public Resources_Building home { get { return _home; } }
	public int strength { get { return _strength; } }

	public Resources_Character (string id,
	                            string name,
	                            string image,
	                            Enums.Gender gender,
	                            Resources_Building home,
	                            int money,
	                            int income,
	                            int expenses,
	                            int strength,
	                            Resources_Inventory inventory) 
		: base (id,
		        name,
		        image,
		        money,
		        income,
		        expenses,
		        inventory)
	{
		_gender = gender;
		_home = home;
		_strength = strength;
	}
}