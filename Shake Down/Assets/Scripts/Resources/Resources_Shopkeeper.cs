using UnityEngine;
using System.Collections;

public class Resources_Shopkeeper : Resources_NPC
{
	private int _respect;
	
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
}