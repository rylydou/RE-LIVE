using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Tile Palette", menuName = "RE-LIVE/Tile Palette", order = 1)]
public class TilePalette : ScriptableObject
{
	public Sprite spIcon;
	[Space]
	public Tile tileNull;
	[Space]
	public Tile[] tiles;

	#region Utils
	public Tile FindTile(TileConnections connections)
	{
		var tile = tiles.ToList().Find((x) => x.connections == connections);
		if (tile.tile == null) return tileNull;
		return tile;
	}
	#endregion
}