using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class BinarySerialization
{
	public static BinaryFormatter myBinaryFormatter = new BinaryFormatter();

	public static void SaveToPlayerPrefs(string _IDTag, object _obj)
	{
		MemoryStream memoryStream = new MemoryStream ();
		myBinaryFormatter.Serialize (memoryStream, _obj);
		string temp = System.Convert.ToBase64String (memoryStream.ToArray ());
		PlayerPrefs.SetString (_IDTag, temp);
	}

	public static object LoadFromPlayerPrefs(string _IDTag)
	{
		string temp = PlayerPrefs.GetString (_IDTag);
		if (string.IsNullOrEmpty (temp))
			return null;

		MemoryStream memoryStream = new MemoryStream (System.Convert.FromBase64String (temp));
		return myBinaryFormatter.Deserialize(memoryStream);
	}

}
