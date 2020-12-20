using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public struct Tile
{
	[NaughtyAttributes.ShowAssetPreview]
	public TileBase tile;
	[Space]
	public TileConnections connections;

	public Sprite sprite
	{
		get
		{
			TileData tileData = new TileData();
			tile.GetTileData(Vector3Int.zero, null, ref tileData);
			return tileData.sprite;
		}
	}
}