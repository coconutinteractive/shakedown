using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Resources_Building : Resources_Root
{
	private Enums.BuildingType _type;
	private int _rent;
	private int _payment;
	private Enums.DayOfTheWeek _day;
	private bool _hasPaid = false;

	public Enums.BuildingType type 	{ get { return _type; 		} }
	public int rent 				{ get { return _rent; 		} } 
	public int payment 				{ get { return _payment; 	} }
	public Enums.DayOfTheWeek day 	{ get { return _day; 		} }
	public bool hasPaid 			{ get { return _hasPaid; 	} set { _hasPaid = value; } }


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
	                            Resources_Inventory inventory)
		: base (id,
		        name,
		        image,
		        money,
		        income,
		        expenses,
		        inventory)
	{
		_type = type;
		_payment = payment;
		_day = day;
		_type = type;
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
}