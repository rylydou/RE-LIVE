using System.Collections.Generic;
using UnityEngine;

public struct GhostData
{
	public List<Vector2> steps;
	public int cosmeticIndex;

	public GhostData(int cosmeticIndex)
	{
		steps = new List<Vector2>();
		this.cosmeticIndex = cosmeticIndex;
	}

	public void AddStep(Vector2 position)
	{
		steps.Add(position);
	}
}