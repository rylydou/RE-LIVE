using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace RELIVE
{
	public class DataManager : MonoBehaviour
	{
		// Singleton
		static DataManager m_current;
		public static DataManager current { get => m_current; }

		// Public Data
		[NaughtyAttributes.ReadOnly] public TilePalette[] tilePalettes;
		[NaughtyAttributes.ReadOnly] public GameObject[] elements;

		void Ready()
		{
			m_current = this;

			FolderIndex index;

			index = Resources.Load<FolderIndex>("Objects/Palettes/Tiles/[ Index ]");
			tilePalettes = new TilePalette[index.objects.Length + 1];
			tilePalettes[0] = Resources.Load<TilePalette>("Objects/Palettes/None");
			for (int i = 0; i < index.objects.Length; i++)
			{
				tilePalettes[i + 1] = index.objects[i] as TilePalette;
			}

			index = Resources.Load<FolderIndex>("Prefabs/Elements/[ Index ]");
			elements = new GameObject[index.objects.Length + 1];
			elements[0] = null;
			for (int i = 0; i < index.objects.Length; i++)
			{
				elements[i + 1] = index.objects[i] as GameObject;
			}
		}

		public int FindPalette(string name)
		{
			return System.Array.FindIndex(tilePalettes, (x) => x.name == name);
		}
	}
}