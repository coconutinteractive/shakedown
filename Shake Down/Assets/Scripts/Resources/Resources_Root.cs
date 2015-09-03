using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Resources_Root
{
	static protected List<Resources_Root> _resources = new List<Resources_Root>();

	protected string _id;
	protected string _name;
	protected string _image;
	protected int _money;
	protected int _income;
	protected int _expenses;

	static public List<Resources_Root> resources { get { return _resources; } }

	public string id { get { return _id; } }
	public string name { get { return _name; } }
	public string image { get { return _image; } }
	public int money { get { return _money; } set{_money = value;} }
	public int income { get { return _income; } set{_income = value;}}
	public int expenses { get { return _expenses; } set{_expenses = value;}}

	public Resources_Root (string id, string name, string image, int money, int income, int expenses)
	{
		if (GetResourceByID(id) == null)
		{
			_id = id;
			_name = name;
			_image = image;
			_money = money;
			_income = income;
			_expenses = expenses;

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