using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public struct Tile
{
	[NaughtyAttributes.ShowAssetPreview]
	public Sprite sprite;
	[NaughtyAttributes.ShowAssetPreview]
	public TileBase tile;
	[Space]
	public TileConnections connections;
}