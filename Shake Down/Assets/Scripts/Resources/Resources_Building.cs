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
	protected bool _hasPaid = false;

	public Enums.BuildingType type 				{ get { return _type; 		} }
	public Enums.DayOfTheWeek day 				{ get { return _day; 		} }
	public int rent 							{ get { return _rent; 		} } 
	public int payment 							{ get { return _payment; 	} }
	public Resources_InventoryShop inventory	{ get { return _inventory;	} }
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
	}

	static public List<Resources_Building> GetBuildings()
	{
		List<Resources_Building> list = new List<Resources_Building>();
		for (int i = 0; i < Resources_Root.resources.Count; i++)
		{
			if (Resources_Root.resources[i] is Resources_Building)
			{
				list.Add ((Resources_Building)Resources_Root.resources[i]);
			}
		}
		return list;
	}

	static public Resources_Building GetBuildingByID(string id)
	{
		Resources_Root resource = Resources_Root.GetResourceByID(id);
		if(resource is Resources_Building)
		{
			return (Resources_Building)resource;
		}
		return null;
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