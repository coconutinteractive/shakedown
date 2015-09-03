using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Item_Root
{
	static protected List<Item_Root> _items = new List<Item_Root>();
	static public List<Item_Root> items { get { return _items; } }

	protected string _id;
	public string id { get { return _id; } }

	protected int _price;
	protected int _strengthBonus;
	protected int _presenceBonus;
	protected int _opinionBonus;
	protected int _energyBonus;

	public virtual int price {				get { return _price; } }
	public virtual int strengthBonus { 		get { return _strengthBonus; } }
	public virtual int presenceBonus { 		get { return _presenceBonus; } }
	public virtual int opinionBonus { 		get { return _opinionBonus; } }
	public virtual int energyBonus { 		get { return _energyBonus; } }

	public Item_Root (string id, int price, int strength, int presence, int opinion, int energy)
	{
		bool uniqueID = true;
		for (int i = 0; i < _items.Count; i++)
		{
			if(_items[i].id == id)
			{
				uniqueID = false;
				Debug.LogError ("Duplicate Item ID Detected: " + id);
				break;
			}
		}
		if(uniqueID)
		{
			_id = id;
			_price = price;
			_strengthBonus = strength;
			_presenceBonus = presence;
			_opinionBonus = opinion;
			_energyBonus = energy;
			_items.Add (this);
		}
	}

	public Item_Root CreateInstanceOfItem(string id)
	{
		if(Item_Root.GetItemByID(id) != null) {
			return (Item_Root)Item_Root.GetItemByID(id).MemberwiseClone();
		} else {
			Debug.LogError ("Could not find a definition for '" + id + "' to create an instance of.");
			return null;
		}
	}

	static public Item_Root GetItemByID(string id)
	{
		for (int i = 0; i < _items.Count; i++)
		{
			if(_items[i].id == id) {
				return _items[i];
			}
		}
		return null; 
	}
}