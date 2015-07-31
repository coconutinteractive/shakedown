using UnityEngine;
using System.Collections;

public class Resources_Shopkeeper : Resources_NPC
{
<<<<<<< HEAD
	private int _respect;
=======
	public Resources_Shopkeeper (string newID, int startingStrength, int startingRespect, int startingFear, int startingMoney, int startingIncome, int startingExpenses, int startingProtectionPayment, string gender, string buildingID, string profileImageID = "DefaultFace") : base (newID, gender, buildingID, profileImageID)
	{
		Debug.Log ("Fixin' Stuff!");
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
>>>>>>> origin/master

	public int respect { get { return _respect; } }

	public Resources_Shopkeeper (string id,
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
		Manager_Resources.NewShopkeeper(this);
	}
<<<<<<< HEAD
=======

	public int profit { get { return _income - _expenses; } }
>>>>>>> origin/master
}