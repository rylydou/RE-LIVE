using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ElementData
{
	public int id;
	public Vector2 position;

	public ElementData(int id, Vector2 position)
	{
		this.id = id;
		this.position = position;
	}
}