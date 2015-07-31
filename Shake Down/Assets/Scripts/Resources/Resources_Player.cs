using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Resources_Player : Resources_Character
{
	private int _presence;
	private int _opinion;
	public Resources_Player (int startingStrength, int startingPresence, int startingOpinion, int startingMoney, string gender, string buildingID, string profileImageID = "DefaultFace") : base ("player", gender, buildingID, profileImageID)
	{
		Debug.Log ("Fixin' Stuff!");
		_strength = startingStrength;
		_energy = startingStrength;
		_presence = startingPresence;
		_opinion = startingOpinion;
		_money = startingMoney;
		Manager_Resources.NewPlayer(this);
		/*proof = new Dictionary<string, bool>();*/
	}

	// Strength
	private const uint maxStrength = 100;
	[SerializeField] private int _strength;
	public int strength { get { return _strength; } }
	public void AugmentStrength (int value)
	{	_strength = (int)Mathf.Clamp (_strength + value, 0, maxStrength);
		if (_strength <= 0) { DeathByLackOfResources(this); } }
>>>>>>> origin/master

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