using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Tile Pallet", menuName = "RE-LIVE/Tile Pallet", order = 1)]
public class TilePallet : ScriptableObject
{
	public string palletName;
	[Space]
	[SerializeField] Tile tileNull;
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