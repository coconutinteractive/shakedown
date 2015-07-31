using UnityEngine;
using System.Collections;

public class Enums
{
	public enum DaysOfWeek {Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday};
	public enum BuildingTypes { building_empty, building_jewelry, building_clothes, building_gym, building_restaurant,
							    building_butcher, building_safehouse, building_bar, building_police,
								building_liquor, building_tobacco, building_grocery, building_strip_club};
	public enum OfficerStates {};
	public enum ShopkeeperStates {Robbed, Vandalized, Aggressive, Passive};
	public enum Gender {female = 0, male = 1};

	static public Enums.Gender GenderFromString(string text)
	{
		Enums.Gender value = Enums.Gender.female;
		switch(text) {
		case "male":	value = Enums.Gender.male; break;
		case "female":	value = Enums.Gender.female; break;
		} return value;
	}

	static public Enums.BuildingTypes BuildingTypeFromString(string text)
	{
		Enums.BuildingTypes value = Enums.BuildingTypes.building_empty;
		switch (text) {
		case "building_jewelry":		value = Enums.BuildingTypes.building_jewelry; break;
		case "building_clothes":		value = Enums.BuildingTypes.building_clothes; break;
		case "building_gym":			value = Enums.BuildingTypes.building_gym; break;
		case "building_restaurant":		value = Enums.BuildingTypes.building_restaurant; break;
		case "building_butcher":		value = Enums.BuildingTypes.building_butcher; break;
		case "building_safehouse":		value = Enums.BuildingTypes.building_safehouse; break;
		case "building_bar":			value = Enums.BuildingTypes.building_bar; break;
		case "building_police":			value = Enums.BuildingTypes.building_police; break;
		case "building_liquor":			value = Enums.BuildingTypes.building_liquor; break;
		case "building_tobacco":		value = Enums.BuildingTypes.building_tobacco; break;
		case "building_grocery":		value = Enums.BuildingTypes.building_grocery; break;
		case "building_strip_club":		value = Enums.BuildingTypes.building_strip_club; break;
		} return value;
	}

	static public Enums.DaysOfWeek DaysOfWeekFromString(string text)
	{
		Enums.DaysOfWeek value = Enums.DaysOfWeek.Monday;
		switch (text) {
		case "day_monday":		value = Enums.DaysOfWeek.Monday; break;
		case "day_tuesday":		value = Enums.DaysOfWeek.Tuesday; break;
		case "day_wednesday":	value = Enums.DaysOfWeek.Wednesday; break;
		case "day_thursday":	value = Enums.DaysOfWeek.Thursday; break;
		case "day_friday":		value = Enums.DaysOfWeek.Friday; break;
		case "day_saturday":	value = Enums.DaysOfWeek.Saturday; break;
		case "day_sunday":		value = Enums.DaysOfWeek.Sunday; break;
		} return value;
	}
}
