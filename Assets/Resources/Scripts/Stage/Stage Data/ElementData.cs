using System.Collections.Generic;
using UnityEngine;

namespace RELIVE.Stage
{
	[System.Serializable]
	public struct ElementData
	{
		public int id;
		public Vector2 position;

		[System.NonSerialized] public Dictionary<string, string> perams;

		public string[] peramVars;
		public string[] peramValues;

		public ElementData(int id, Vector2 position, Dictionary<string, string> data)
		{
			this.id = id;
			this.position = position;
			this.perams = data;
			this.peramVars = null;
			this.peramValues = null;
		}

		#region Saving and Loading
		public void UnpackData(bool bCleanupData = false)
		{
			if (peramVars == null) return;

			perams = new Dictionary<string, string>();

			for (int i = 0; i < peramVars.Length; i++)
			{
				perams.Add(peramVars[i], peramValues[i]);
			}

			if (bCleanupData)
			{
				peramVars = null;
				peramValues = null;
			}
		}

		public void PackData(bool bCleanupData = false)
		{
			if (perams == null) return;

			peramVars = new string[perams.Count];
			peramValues = new string[perams.Count];

			int write = 0;
			foreach (var keyPair in perams)
			{
				peramVars[write] = keyPair.Key;
				peramValues[write] = keyPair.Value;

				write++;
			}

			if (bCleanupData) perams = null;
		}
		#endregion

		#region Peram Functions
		#region Setters
		public bool SetString(string key, string value)
		{
			if (perams.ContainsKey(key))
			{
				perams[key] = value;
				return true;
			}

			perams.Add(key, value);
			return false;
		}

		public bool SetInt(string key, int value)
		{
			if (perams.ContainsKey(key))
			{
				perams[key] = value.ToString();
				return true;
			}

			perams.Add(key, value.ToString());

			return false;
		}

		public bool SetFloat(string key, float value)
		{
			if (perams.ContainsKey(key))
			{
				perams[key] = value.ToString();
				return true;
			}

			perams.Add(key, value.ToString());

			return false;
		}

		public bool SetVector2(string key, Vector2 value)
		{
			return SetFloat(key + "_X", value.x) == true && SetFloat(key + "_Y", value.y) == true;
		}

		public bool SetVector3(string key, Vector3 value)
		{
			return SetFloat(key + "_X", value.x) == true && SetFloat(key + "_Y", value.y) == true && SetFloat(key + "_Z", value.z) == true;
		}
		#endregion

		#region Getters
		public string GetString(string key)
		{
			string value;
			if (!perams.TryGetValue(key, out value)) Debug.LogError($"Key of \"{key}\" can not be found!");

			return value;
		}

		public int GetInt(string key)
		{
			string text;
			int value;
			if (!perams.TryGetValue(key, out text)) Debug.LogError($"Key of \"{key}\" can not be found!");
			if (!int.TryParse(text, out value)) Debug.LogError($"Key of \"{key}\" is not a {value.GetType()}");

			return value;
		}

		public float GetFloat(string key)
		{
			string text;
			float value;
			if (!perams.TryGetValue(key, out text)) Debug.LogError($"Key of \"{key}\" can not be found!");
			if (!float.TryParse(text, out value)) Debug.LogError($"Key of \"{key}\" is not a {value.GetType()}");

			return value;
		}

		public Vector2 GetVector2(string key)
		{
			return new Vector2(GetFloat(key + "_X"), GetFloat(key + "_Y"));
		}

		public Vector3 GetVector3(string key)
		{
			return new Vector3(GetFloat(key + "_X"), GetFloat(key + "_Y"), GetFloat(key + "_Z"));
		}
		#endregion
		#endregion
	}
}