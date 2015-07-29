using UnityEngine;
using System.Collections;

public class ShopItem
{
	private string _id;
	public string id { get { return _id; } }

	private int _price;
	public int price { get { return _price; } }

	public ShopItem (string itemID, int itemPrice)
	{
		_id = itemID;
		_price = itemPrice;
	}
}
