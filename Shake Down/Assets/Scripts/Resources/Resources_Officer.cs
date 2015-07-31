using UnityEngine;
using System.Collections;

public class Resources_Officer : Resources_NPC
{
	public Resources_Officer (string id,
	                          string name,
	                          string image,
	                          Enums.Gender gender,
	                          Resources_Building home,
	                          int money,
	                          int income,
	                          int expenses,
	                          int strength,
	                          int respect,
	                          int fear,
	                          int greed,
	                          int integrity,
	                          int stubbornness,
	                          Resources_Inventory inventory)
		: base (id,
		        name,
		        image,
		        gender,
		        home,
		        money,
		        income,
		        expenses,
		        strength,
		        respect,
		        fear,
		        greed,
		        integrity,
		        stubbornness,
		        inventory)
	{
		Manager_Resources.NewOfficer (this);
	}
<<<<<<< HEAD
=======
	
	// Strength
	private const uint maxStrength = 100;
	[SerializeField] private int _strength;
	public int strength { get { return _strength; } }
	public void AugmentStrength (int value)
	{	_strength = (int)Mathf.Clamp (_strength + value, 0, maxStrength);
		if (_strength <= 0) { DeathByLackOfResources(this); } }
	
	// Greed
	private const uint maxGreed = 100;
	[SerializeField] private int _greed;
	public int greed { get { return _greed; } }
	public void AugmentGreed (int value)
	{
		_greed = (int)Mathf.Clamp (_greed + value, 0, maxGreed);
	}
	
	// Integrity
	private const uint maxIntegrity = 100;
	[SerializeField] private int _integrity;
	public int integrity { get { return _integrity; } }
	public void AugmentIntegrity (int value)
	{
		_integrity = (int)Mathf.Clamp (_integrity + value, 0, maxIntegrity);
	}
>>>>>>> origin/master
}