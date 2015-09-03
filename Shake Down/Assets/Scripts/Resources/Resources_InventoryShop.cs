using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Resources_InventoryShop
{
	static protected List<Resources_InventoryShop> _inventories = new List<Resources_InventoryShop>();

	private string _id;
	private Item_Root _shopItem1;
	private Item_Root _shopItem2;
	private Item_Root _shopItem3;
	private Item_Root _shopItem4;

	static public List<Resources_InventoryShop> inventories { get { return _inventories; } }
	
	public string	 id		   { get { return _id; } }
	public Item_Root shopItem1 { get { return _shopItem1; } }
	public Item_Root shopItem2 { get { return _shopItem2; } }
	public Item_Root shopItem3 { get { return _shopItem3; } }
	public Item_Root shopItem4 { get { return _shopItem4; } }

	public List<Item_Root> items {
		get {
			List<Item_Root> list = new List<Item_Root>();
			if(shopItem1 != null) { list.Add (shopItem1); }
			if(shopItem2 != null) { list.Add (shopItem2); }
			if(shopItem3 != null) { list.Add (shopItem3); }
			if(shopItem4 != null) { list.Add (shopItem4); }
			return list;
		} 
	}

	public Resources_InventoryShop (string id, Item_Root item1, Item_Root item2, Item_Root item3, Item_Root item4)
		//: base (id, null, null, null, null, null, null, null, null, null, null)
	{
		_id = id;
		_shopItem1 = item1;
		_shopItem2 = item2;
		_shopItem3 = item3;
		_shopItem4 = item4;
		_inventories.Add (this);
	}

	static public Resources_InventoryShop GetShopInventoryByID(string id)
	{
		for (int i = 0; i < inventories.Count; i++)
		{
			if (inventories[i].id == id && inventories[i] is Resources_InventoryShop)
			{
				return (Resources_InventoryShop)inventories[i];
			}
		}
		return null;
	}
}