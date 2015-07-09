using UnityEngine;
using System.Collections;

public class Scenario_01 : Scenario_Master
{
	override protected void setup()
	{
		new Resources_Player (20, 20, 20, 100);
		new Resources_Shopkeeper ("alfred", 20, 20, 20, 100, 20, 15);
		new Resources_Shopkeeper ("bigsby", 20, 20, 20, 100, 20, 15);
		new Resources_Shopkeeper ("charlie", 20, 20, 20, 100, 20, 15);
		new Resources_Officer ("douglas", 20, 20, 20);
		new Resources_Officer ("evan", 20, 20, 20);
		new Resources_Shopkeeper ("francois", 20, 20, 20, 100, 20, 15);
	}
}