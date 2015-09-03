using UnityEngine;
using System.Collections;

public class Item_Consumable : Item_Root
{
	public Item_Consumable (string id, int price, int strength, int presence, int opinion, int energy)
		: base (id, price, strength, presence, opinion, energy)
	{

	}
}