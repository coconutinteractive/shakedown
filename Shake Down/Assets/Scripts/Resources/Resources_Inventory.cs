using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Resources_Inventory
{
	static private List<Resources_Inventory> _inventories = new List<Resources_Inventory>();

	private string _id;
	private object _weapon;
	private object _clothing;
	private object _jewelry;

	static public List<Resources_Inventory> inventories { get { return _inventories; } }

	public string id { get { return _id; } }
	public object weapon { get { return _weapon; } }
	public object clothing { get { return _clothing; } }
	public object jewelry { get { return _jewelry; } }

	public Resources_Inventory (object weapon, object clothing, object jewelry)
	{
		if (GetInventoryByID(id) != null)
		{
			_id = id;
			_weapon = weapon;
			_clothing = clothing;
			_jewelry = jewelry;

			_inventories.Add (this);
			Debug.Log (this);
			Debug.Log (inventories.Count);
		}
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

	static public Resources_Inventory GenerateInventoryFromJSON(string jsonKey, string playerID)
	{
		return new Resources_Inventory (new Object(), new Object(), new Object());
	}

	static public void ClearInventories()
	{
		_inventories.Clear();
	}
}