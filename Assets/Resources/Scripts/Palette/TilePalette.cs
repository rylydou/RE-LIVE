using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Tile Palette", menuName = "RE-LIVE/Tile Palette", order = 1)]
public class TilePalette : ScriptableObject
{
	public bool bIncludeInBuild = true;
	public Sprite spIcon;
	[Space]
	public PaletteTile tileNull;
	[Space]
	public PaletteTile[] tiles;

#if UNITY_EDITOR
	[NaughtyAttributes.Button]
	void TryToFindTiles()
	{
		var resourceTiles = Resources.LoadAll<TileBase>($"Tile Palettes/{name}/Tiles");

		tiles = new PaletteTile[resourceTiles.Length];

		for (int i = 0; i < resourceTiles.Length; i++)
		{
			tiles[i].tile = resourceTiles[i];

			var tileSuffix = resourceTiles[i].name.Split('_')[1];

			foreach (var suffix in tileSuffix)
			{
				switch (suffix)
				{
					case 't':
						tiles[i].connections.top = true;
						break;
					case 'r':
						tiles[i].connections.right = true;
						break;
					case 'b':
						tiles[i].connections.bottom = true;
						break;
					case 'l':
						tiles[i].connections.left = true;
						break;
				}
			}

		}

		spIcon = System.Array.Find(tiles, (x) => x.connections.bottom && x.connections.right).sprite;
	}
#endif

	#region Utils
	public PaletteTile FindTile(TileConnections connections)
	{
		var tile = tiles.ToList().Find((x) => x.connections == connections);
		if (tile.tile == null) return tileNull;
		return tile;
	}
	#endregion
}