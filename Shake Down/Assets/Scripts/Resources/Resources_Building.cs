using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Resources_Building : Resources_Root
{
	protected Enums.BuildingType _type;
	protected Enums.DayOfTheWeek _day;
	protected int _rent;
	protected int _payment;
	protected Resources_InventoryShop _inventory;
	protected Building_Script _building;
	protected bool _hasPaid = false;

	public Enums.BuildingType type 				{ get { return _type; 		} }
	public Enums.DayOfTheWeek day 				{ get { return _day; 		} }
	public int rent 							{ get { return _rent; 		} } 
	public int payment 							{ get { return _payment; 	} }
	public Resources_InventoryShop inventory	{ get { return _inventory;	} }
	public Building_Script building				{ get { return _building;	} set { _building = value; } }
	public bool hasPaid 						{ get { return _hasPaid; 	} set { _hasPaid = value; } }
	
	public Resources_Building  (string id,
	                            string name,
	                            string image,
	                            Enums.BuildingType type,
	                            int money,
	                            int income,
	                            int expenses,
	                            int rent,
	                            int payment,
	                            Enums.DayOfTheWeek day,
	                            Resources_InventoryShop inventory)
		: base (id,
		        name,
		        image,
		        money,
		        income,
		        expenses)
	{
		_type = type;
		_payment = payment;
		_day = day;
		_type = type;
		_inventory = inventory;

		Manager_Resources.NewBuilding(this);
	}

	public string DefaultProtectionOffer()
	{
		int profit = _income - (_expenses - _rent);
		if (profit > 0) {
			profit = Mathf.CeilToInt((float)(profit * 0.15 * 7));
		} else {
			profit = 0;
		}
		string text = "";
		for (int i = 5; i > 0; i--)
		{
			if(profit.ToString ().Length >= i)
			{
				text += profit.ToString();
				break;
			}
			else
			{
				text += "0";
			}
		}
		return text;
	}
}