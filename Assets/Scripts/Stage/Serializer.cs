using UnityEngine;
using Ciber_Turtle.IO;

public static class Serializer
{
	public static string Encode(object obj)
	{
		return JsonUtility.ToJson(obj, true).Replace("    ", "	");
	}

	public static T Decode<T>(string data)
	{
		return JsonUtility.FromJson<T>(data);
	}
}