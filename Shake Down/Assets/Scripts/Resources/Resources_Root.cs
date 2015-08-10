using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Resources_Root
{
	static private List<Resources_Root> _resources = new List<Resources_Root>();

	private string _id;
	private string _name;
	private string _image;
	private int _money = 350;
	private int _income;
	private int _expenses;
	private Resources_Inventory _inventory;

	static public List<Resources_Root> resources { get { return _resources; } }

	public string id { get { return _id; } }
	public string name { get { return _name; } }
	public string image { get { return _image; } }
	public int money { get { return _money; } }
	public int income { get { return _income; } }
	public int expenses { get { return _expenses; } }
	public Resources_Inventory inventory { get { return _inventory; } }

	public Resources_Root (string id, string name, string image, int money, int income, int expenses, Resources_Inventory inventory)
	{
		if (GetResourceByID(id) == null)
		{
			_id = id;
			_name = name;
			_image = image;
			_money = money;
			_income = income;
			_expenses = expenses;
			_inventory = inventory;

			_resources.Add (this);
		}
	}

	static public Resources_Root GetResourceByID(string id)
	{
		for (int i = 0; i < resources.Count; i++)
		{
			if (resources[i].id == id)
			{
				return resources[i];
			}
		}
		return null;
	}

	static public void ClearAllResourceLists()
	{
		_resources = new List<Resources_Root>();
		Resources_Inventory.ClearInventories();
	}
}