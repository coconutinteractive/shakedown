using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Resources_Root
{
	protected string _id;
	protected string _name;
	protected string _image;
	protected int _money;
	protected int _income;
	protected int _expenses;

	public string id { get { return _id; } }
	public string name { get { return _name; } }
	public string image { get { return _image; } }
	public int money { get { return _money; } set{_money = value;} }
	public int income { get { return _income; } set{_income = value;}}
	public int expenses { get { return _expenses; } set{_expenses = value;}}

	public Resources_Root (string id, string name, string image, int money, int income, int expenses)
	{
		_id = id;
		_name = name;
		_image = image;
		_money = money;
		_income = income;
		_expenses = expenses;
	}
}