using UnityEngine;
using System.Collections;

public class Resources_NPC : Resources_Character
{
	private int _respect;
	private int _fear;
	private int _greed;
	private int _integrity;
	private int _stubbornness;
	private int _attitude;

	public int respect { get { return _respect; } }
	public int fear { get { return _fear; } }
	public int greed { get { return _greed; } }
	public int integrity { get { return _integrity; } }
	public int stubbornness { get { return _stubbornness; } }
	public int attitude { get { return _attitude; } }

	public Resources_NPC (string id,
	                      string name,
	                      string image,
	                      Enums.Gender gender,
	                      Resources_Building home,
	                      int money,
	                      int income,
	                      int expenses,
	                      int strength,
	                      int respect,
	                      int fear,
	                      int greed,
	                      int integrity,
	                      int stubbornness,
	                      Resources_Inventory inventory)
		: base (id,
		        name,
	            image,
		        gender,
		        home,
	            money,
	            income,
		        expenses,
	            strength,
	            inventory)
	{
		_respect = respect;
		_fear = fear;
		_greed = greed;
		_integrity = integrity;
		_stubbornness = stubbornness;
	}
}