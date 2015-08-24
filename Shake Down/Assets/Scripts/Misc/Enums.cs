using UnityEngine;
using System.Collections;

public class Enums
{
	public enum DayOfTheWeek {Sun = 0, Mon, Tue, Wed, Thu, Fri, Sat};
	public enum BuildingType { building_empty = 0, building_jewelry, building_clothes, building_gym, building_restaurant,
							    building_butcher, building_safehouse, building_bar, building_police,
								building_liquor, building_tobacco, building_grocery, building_strip_club};
	public enum OfficerState {};
	public enum ShopkeeperState {Robbed, Vandalized, Aggressive, Passive};
	public enum ShopkeeperAttitude {Neutral, Awe, Admiration, Disdain, Terror, High_Fear, High_Respect};
	public enum ShopkeeperPersonality {Casual, Formal, Gruff, Bubbly};

	public enum Attitude {Neutral = 0, Loyal, Reliant, Disinterest, Terror};
	public enum Personality {Outgoing, Introvert, Gruff, Formal};
	public enum Gender {female = 0, male = 1};

	public enum IntimidateAction {None, Imply = 2, Threaten = 3, Act = 4};
	public enum Language {test, englishus, englishuk};

	static public Enums.Gender GenderFromString(string text)
	{
		Enums.Gender value = Enums.Gender.female;
		switch(text) {
		case "male":	value = Enums.Gender.male; break;
		case "female":	value = Enums.Gender.female; break;
		} return value;
	}

	static public Enums.BuildingType BuildingTypeFromStatic(string text)
	{
		Enums.BuildingType value = Enums.BuildingType.building_empty;
		switch (text) {
		case "building_jewelry":		value = Enums.BuildingType.building_jewelry; break;
		case "building_clothes":		value = Enums.BuildingType.building_clothes; break;
		case "building_gym":			value = Enums.BuildingType.building_gym; break;
		case "building_restaurant":		value = Enums.BuildingType.building_restaurant; break;
		case "building_butcher":		value = Enums.BuildingType.building_butcher; break;
		case "building_safehouse":		value = Enums.BuildingType.building_safehouse; break;
		case "building_bar":			value = Enums.BuildingType.building_bar; break;
		case "building_police":			value = Enums.BuildingType.building_police; break;
		case "building_liquor":			value = Enums.BuildingType.building_liquor; break;
		case "building_tobacco":		value = Enums.BuildingType.building_tobacco; break;
		case "building_grocery":		value = Enums.BuildingType.building_grocery; break;
		case "building_strip_club":		value = Enums.BuildingType.building_strip_club; break;
		} return value;
	}

	static public Enums.DayOfTheWeek DayOfTheWeekFromStatic(string text)
	{
		Enums.DayOfTheWeek value = Enums.DayOfTheWeek.Mon;
		switch (text) {
		case "day_monday":		value = Enums.DayOfTheWeek.Mon; break;
		case "day_tuesday":		value = Enums.DayOfTheWeek.Tue; break;
		case "day_wednesday":	value = Enums.DayOfTheWeek.Wed; break;
		case "day_thursday":	value = Enums.DayOfTheWeek.Thu; break;
		case "day_friday":		value = Enums.DayOfTheWeek.Fri; break;
		case "day_saturday":	value = Enums.DayOfTheWeek.Sat; break;
		case "day_sunday":		value = Enums.DayOfTheWeek.Sun; break;
		} return value;
	}

	static public Enums.Personality PersonalityFromStatic(string text)
	{
		Enums.Personality value = Enums.Personality.Formal;
		switch (text) {
		case "personality_outgoing":	value = Enums.Personality.Outgoing; break;
		case "personality_introvert":	value = Enums.Personality.Introvert; break;
		case "personality_gruff":		value = Enums.Personality.Gruff; break;
		case "personality_formal":		value = Enums.Personality.Formal; break;
		} return value;
	}

	static public Enums.Attitude AttitudeFromStatic(string text)
	{
		Enums.Attitude value = Enums.Attitude.Neutral;
		switch (text) {
		case "attitude_neutral":		value = Enums.Attitude.Neutral; break;
		case "attitude_disinterest": 	value = Enums.Attitude.Disinterest; break;
		case "attitude_loyal":			value = Enums.Attitude.Loyal; break;
		case "attitude_reliant":		value = Enums.Attitude.Reliant; break;
		case "attitude_terror":			value = Enums.Attitude.Terror; break;
		} return value;
	}
}
