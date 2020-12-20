using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class PaletteManager : MonoBehaviour
{
	// Singleton
	static PaletteManager m_current;
	public static PaletteManager current { get => m_current; }

	public List<TilePalette> palettes = new List<TilePalette>();

	void Ready()
	{
		m_current = this;
	}

	public int FindPalette(string name)
	{
		return palettes.FindIndex((x) => x.name == name);
	}
}