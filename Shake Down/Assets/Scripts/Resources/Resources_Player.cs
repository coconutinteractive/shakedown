using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Resources_Player : Resources_Character
{
	private int _presence;
	private int _opinion;

	public int presence { get { return _presence; } }
	public int opinion { get { return _opinion; } }

	public Resources_Player (ProfileSettings profile,
							 Resources_Building home,
	                         int money,
	                         int income,
	                         int expenses,
	                         int strength,
	                         int presence,
	                         int opinion,
	                         Resources_Inventory inventory)
		: base (profile.id,
		        profile.name,
		        profile.image,
		        profile.gender,
		        home,
		        money,
		        income,
		        expenses,
		        strength,
		        inventory)
	{
		_presence = presence;
		_opinion = opinion;

		Manager_Resources.NewPlayer(this);
	}
}