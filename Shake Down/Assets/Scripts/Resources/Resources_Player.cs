using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Resources_Player : Resources_Master 
{
	public Resources_Player (int startingStrength, int startingPresence, int startingOpinion, int startingMoney, string gender, string buildingID, string profileImageID = "DefaultFace") : base ("player", gender, buildingID, profileImageID)
	{
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

	// Energy
	[SerializeField] private int _energy;
	public int energy { get { return _energy; } }
	public bool AugmentEnergy (int value)
	{	if (_energy + value >= 0) {
			_energy = (int)Mathf.Clamp (_energy + value, 0, maxStrength);
			return true;
		} else { return false; } }
	public void RefillEnergy () { _energy = _strength; }
	
	// Presence
	private const uint maxPresence = 100;
	[SerializeField] private int _presence;
	public int presence { get { return _presence; } }
	public void AugmentPresence (int value) { _presence = (int)Mathf.Clamp (_presence + value, 0, maxPresence); }
	
	// Opinion
	private const uint maxOpinion = 100;
	[SerializeField] private int _opinion;
	public int opinion { get { return _opinion; } }
	public void AugmentOpinion (int value) { _opinion = (int)Mathf.Clamp (_opinion + value, 0, maxOpinion); }

	// Money
	[SerializeField] private int _money;
	public int money { get { return _money; } }
	public bool AugmentMoney (int value) 
	{	if (_money + value >= 0) {
			_money += value;
			return true;
		} else { return false; } }

	/*
	// Proof
	[SerializeField] static private Dictionary<string, bool> proof;
	public bool getProof(string shopkeeperID)
	{ return proof[shopkeeperID]; }
	static public void setProof(string shopkeeperID, bool setTo = true)
	{ proof[shopkeeperID] = setTo; }
	*/
}