using UnityEngine;
using System.Collections;
using System;

public class Resources_Shopkeeper : Resources_Master
{
	public Resources_Shopkeeper (string newID, int startingStrength, int startingRespect, int startingFear, int startingMoney, int startingIncome, int startingExpenses, int startingProtectionPayment, string gender, string buildingID, string profileImageID = "DefaultFace") : base (newID, gender, buildingID, profileImageID)
	{
		_strength = startingStrength;
		_respect = startingRespect;
		_fear = startingFear;
		_money = startingMoney;
		_income = startingIncome;
		_expenses = startingExpenses;
		_protectionPayment = startingProtectionPayment;
		Manager_Resources.NewShopkeeper(this);
		/*Resources_Player.setProof(newID, false);*/
	}
	
	// Strength
	private const uint maxStrength = 100;
	[SerializeField] private int _strength;
	public int strength { get { return _strength; } }
	public void AugmentStrength (int value)
	{	_strength = (int)Mathf.Clamp (_strength + value, 0, maxStrength);
		if (_strength <= 0) { DeathByLackOfResources(this); } }

	// Respect
	private const uint maxRespect = 100;
	[SerializeField] private int _respect;
	public int respect { get { return _respect; } }
	public void AugmentRespect (int value)
	{
		_respect = (int)Mathf.Clamp (_respect + value, 0, maxRespect);
	}

	// Fear
	private const uint maxFear = 100;
	[SerializeField] private int _fear;
	public int fear { get { return _fear; } }
	public void AugmentFear (int value)
	{
		_fear = (int)Mathf.Clamp (_fear + value, 0, maxFear);
	}

	// Money
	[SerializeField] private int _money;
	public int money { get { return _money; } }
	public bool AugmentMoney (int value) 
	{	if (_money + value >= 0) {
			_money += value;
			return true;
		} else { return false; } }

	// Income
	[SerializeField] private int _income;
	public int income { get { return _income; } }
	public void AugmentIncome (int value)
	{	_income += value;
		if (_income < 0) { _income = 0; } }

	//Expenses
	[SerializeField] private int _expenses;
	public int expenses { get { return _expenses; } }
	public void AugmentExpenses (int value)
	{	_expenses += value;
		if (_expenses < 0) {
			_expenses = 0; } }

	//Protection Payment
	[SerializeField] private int _protectionPayment;
	public int protectionPayment { get {return _protectionPayment; } }
	public void AugmentPayment (int value)
	{
		_protectionPayment += value;
		if(_protectionPayment < 0)
		{ _protectionPayment = 0; } }

	public bool CalculateFinances ()
	{	_money = _money + (_income - _expenses);
		if (_money < 0) {
			return false;
		}
		return true; }

	public bool IsShopkeeperAggrivated(int playerPresence)
	{
		float paymentRelatedAnger = 0;
		if (protectionPayment > profit)
		{ paymentRelatedAnger += (protectionPayment - profit) / 100; }
		float debtRelatedAnger = 0;
		if (profit < 0)
		{ debtRelatedAnger -= profit; }
		if((protectionPayment + debtRelatedAnger + fear - playerPresence) > 100)
		{ return true; }
		return false;
	}

	public int profit { get { return _income - _expenses; } }
	private Building_Script _buildingRef;
	public Building_Script buildingRef { get { return _buildingRef; } set { _buildingRef = value; } }
}