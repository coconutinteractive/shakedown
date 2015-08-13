using UnityEngine;
using System.Collections;

public class Enums
{
	public enum DayOfTheWeek {Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday};
	public enum BuildingType { building_empty, building_jewelry, building_clothes, building_gym, building_restaurant,
							    building_butcher, building_safehouse, building_bar, building_police,
								building_liquor, building_tobacco, building_grocery, building_strip_club};
	public enum OfficerState {};
	public enum ShopkeeperState {Robbed, Vandalized, Aggressive, Passive};
	public enum ShopkeeperAttitude {Neutral, Awe, Admiration, Disdain, Terror, High_Fear, High_Respect};
	public enum ShopkeeperPersonality {Casual, Formal, Gruff, Bubbly};
	public enum Gender {female = 0, male = 1};

	public enum IntimidateAction {None, Imply = 2, Threaten = 3, Act = 4};
	public enum Language {englishus, englishuk};

	static public Enums.Gender GenderFromString(string text)
	{
		Enums.Gender value = Enums.Gender.female;
		switch(text) {
		case "male":	value = Enums.Gender.male; break;
		case "female":	value = Enums.Gender.female; break;
		} return value;
	}

	static public Enums.BuildingType BuildingTypeFromString(string text)
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

	static public Enums.DayOfTheWeek DayOfTheWeekFromString(string text)
	{
		Enums.DayOfTheWeek value = Enums.DayOfTheWeek.Monday;
		switch (text) {
		case "day_monday":		value = Enums.DayOfTheWeek.Monday; break;
		case "day_tuesday":		value = Enums.DayOfTheWeek.Tuesday; break;
		case "day_wednesday":	value = Enums.DayOfTheWeek.Wednesday; break;
		case "day_thursday":	value = Enums.DayOfTheWeek.Thursday; break;
		case "day_friday":		value = Enums.DayOfTheWeek.Friday; break;
		case "day_saturday":	value = Enums.DayOfTheWeek.Saturday; break;
		case "day_sunday":		value = Enums.DayOfTheWeek.Sunday; break;
		} return value;
	}
}
