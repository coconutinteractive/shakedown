using UnityEngine;
using System.Collections;

[System.Serializable]
public class Resources_Shopkeeper : Resources_NPC
{
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
	                             Enums.Personality personality,
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
		        personality,
		        inventory)
	{
		Manager_Resources.NewShopkeeper(this);
	}
}