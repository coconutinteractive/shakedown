using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Resources_Inventory
{
	static protected List<Resources_Inventory> _inventories = new List<Resources_Inventory>();

	private string					_id;
	private Item_Weapon_Gun			_gun;
	private Item_Weapon_Melee		_melee;
	private Item_Gear_Clothes		_clothes;
	private Item_Gear_Shoes			_shoes;
	private Item_Gear_Pin			_pin;
	private Item_Gear_Neck			_neck;
	private Item_Gear_Ring			_ring;
	private Item_Gear_Wrist			_wrist;
	private Item_Decay_Hair			_hair;
	private Item_Decay_Flower		_flower;
	private List<Item_Consumable>	_consumables = new List<Item_Consumable>();

	static public List<Resources_Inventory> inventories { get { return _inventories; } }

	public string					id			{ get { return _id; } }
	public Item_Weapon_Gun			gun			{ get { return _gun; } }
	public Item_Weapon_Melee		melee		{ get { return _melee; } }
	public Item_Gear_Clothes		clothes		{ get { return _clothes; } }
	public Item_Gear_Shoes			shoes		{ get { return _shoes; } }
	public Item_Gear_Pin			pin			{ get { return _pin; } }
	public Item_Gear_Neck			neck		{ get { return _neck; } }
	public Item_Gear_Ring			ring		{ get { return _ring; } }
	public Item_Gear_Wrist			wrist		{ get { return _wrist; } }
	public Item_Decay_Hair			hair		{ get { return _hair; } }
	public Item_Decay_Flower		flower		{ get { return _flower; } }
	public List<Item_Consumable>	consumables	{ get { return _consumables; } }

	public List<Item_Root> items {
		get {
			List<Item_Root> list = new List<Item_Root>();
			if(gun != null) { list.Add (gun); }
			if(melee != null) { list.Add (melee); }
			if(clothes != null) { list.Add (clothes); }
			if(shoes != null) { list.Add (shoes); }
			if(pin != null) { list.Add (pin); }
			if(neck != null) { list.Add (neck); }
			if(ring != null) { list.Add (ring); }
			if(wrist != null) { list.Add (wrist); }
			if(hair != null) { list.Add (hair); }
			if(flower != null) { list.Add (flower); }
			for (int i = 0; i < consumables.Count; i++)
			{
				if(consumables[i] != null) {list.Add(consumables[i]);}
			}
			return list;
		} 
	}

	public Resources_Inventory (string					id,
	                            Item_Weapon_Gun			gun,
	                            Item_Weapon_Melee		melee,
	                            Item_Gear_Clothes		clothes,
	                            Item_Gear_Shoes			shoes,
	                            Item_Gear_Pin			pin,
	                            Item_Gear_Neck			neck,
	                            Item_Gear_Ring			ring,
	                            Item_Gear_Wrist			wrist,
	                            Item_Decay_Hair			hair,
	                            Item_Decay_Flower		flower)
	{
		_id = id;
		_gun = gun;
		_melee = melee;
		_clothes = clothes;
		_shoes = shoes;
		_pin = pin;
		_neck = neck;
		_ring = ring;
		_wrist = wrist;
		_hair = hair;
		_flower = flower;

		_inventories.Add (this);
	}

	static public Resources_Inventory GetInventoryByID(string id)
	{
		for (int i = 0; i < inventories.Count; i++)
		{
			if (inventories[i].id == id)
			{
				return inventories[i];
			}
		}
		return null;
	}

	static public void ClearInventories()
	{
		_inventories.Clear();
	}
}