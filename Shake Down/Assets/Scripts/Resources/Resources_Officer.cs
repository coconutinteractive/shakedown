using UnityEngine;
using System.Collections;

public class Resources_Officer : Resources_Master
{
	public Resources_Officer (string newID, int startingStrength, int startingGreed, int startingIntegrity) : base (newID)
	{
		Debug.Log ("Fixin' Stuff!");
		_strength = startingStrength;
		_greed = startingGreed;
		_integrity = startingIntegrity;
		Manager_Resources.NewOfficer (this);
	}

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
}
