using UnityEngine;
using System.Collections;

public class Item_Decay : Item_Root
{
	protected int _duration;
	protected int _remainingDuration;
	protected float percentValue { get { return _duration/_remainingDuration; } }

	public virtual int price {				get { return Mathf.FloorToInt(percentValue * _price); } }
	public virtual int strengthBonus { 		get { return Mathf.FloorToInt(percentValue * _strengthBonus); } }
	public virtual int presenceBonus { 		get { return Mathf.FloorToInt(percentValue * _presenceBonus); } }
	public virtual int opinionBonus { 		get { return Mathf.FloorToInt(percentValue * _opinionBonus); } }
	public virtual int energyBonus { 		get { return Mathf.FloorToInt(percentValue * _energyBonus); } }

	public Item_Decay (string id, int price, int strength, int presence, int opinion, int energy, int duration)
		: base (id, price, strength, presence, opinion, energy)
	{
		_duration = duration;
		_remainingDuration = duration;
	}

	static public bool DecayDurationByTime ()
	{
		return true;
	}
}