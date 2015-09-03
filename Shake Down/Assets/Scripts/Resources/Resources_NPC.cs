using UnityEngine;
using System.Collections;

public class Resources_NPC : Resources_Character
{
	static protected GraphPoint BOT = new GraphPoint (30 ,  0);
	static protected GraphPoint TOP = new GraphPoint (30 ,100);
	static protected GraphPoint LFT = new GraphPoint (0  , 30);
	static protected GraphPoint RGT = new GraphPoint (100, 30);

	protected int 				_respect;
	protected int 				_fear;
	protected int 				_greed;
	protected int 				_integrity;
	protected int 				_stubbornness;
	protected Enums.Personality 	_personality;
	protected Enums.Attitude 		_attitude;

	public int 					respect 		{ get { return _respect; 		} set {_respect = value; 	} }
	public int 					fear 			{ get { return _fear; 			} set {_fear = value;		} }
	public int 					greed 			{ get { return _greed; 			} }
	public int 					integrity 		{ get { return _integrity; 		} }
	public int 					stubbornness 	{ get { return _stubbornness; 	} }
	public Enums.Personality 	personality 	{ get { return _personality; 	} }
	public Enums.Attitude 		attitude 		{ get { return _attitude; 		} }

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
	                      Enums.Personality personality,
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
		_personality = personality;

		UpdateAttitude();
	}

	public void UpdateAttitude()
	{
		if (LineSign (TOP.x, TOP.y, RGT.x, RGT.y, respect, fear) >= 0)
			_attitude = Enums.Attitude.Reliant;
		else if (LineSign (RGT.x, RGT.y, BOT.x, BOT.y, respect, fear) >= 0)
			_attitude = Enums.Attitude.Loyal;
		else if (LineSign (BOT.x, BOT.y, LFT.x, LFT.y, respect, fear) >= 0)
			_attitude = Enums.Attitude.Disinterest;
		else if (LineSign (LFT.x, LFT.y, TOP.x, TOP.y, respect, fear) >= 0)
			_attitude = Enums.Attitude.Terror;
		else
			_attitude = Enums.Attitude.Neutral;
	}

	protected int LineSign(int AX, int AY, int BX, int BY, int PX, int PY)
	{
		return (int)Mathf.Sign ((BX-AX)*(PY-AY) - (BY-AY)*(PX-AX));
	}
}

public class GraphPoint {
	public int x; public int y;
	public GraphPoint(int _x, int _y) {
		x = _x; y = _y; } 
}