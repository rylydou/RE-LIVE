using UnityEngine;

[System.Serializable]
public struct StageData
{
	public int version;
	public string name;
	public Vector2Int size;

	public int iMaxGhosts;
	public float fMaxHistoryTime;

	public Vector2 v2StartPos;

	public ElementData[] elements;

	[SerializeField] int[] serializableTiles;
	[System.NonSerialized] public int[,] tiles;

	public StageData(string name, Vector2Int size)
	{
		this.version = 0;
		this.name = name;

		this.iMaxGhosts = 0;
		this.fMaxHistoryTime = 0;

		this.v2StartPos = Vector2.zero;

		this.size = size;

		this.tiles = new int[size.x, size.y];
		this.serializableTiles = new int[size.x * size.y];

		this.elements = new ElementData[1];
		this.elements[0] = new ElementData(1, new Vector2(5, 4));
	}

	#region Utils
	public bool Check()
	{
		if (iMaxGhosts < 0) return false;
		if (fMaxHistoryTime < 0) return false;

		if (v2StartPos.x < 0) return false;
		if (v2StartPos.y < 0) return false;
		if (v2StartPos.x > size.x) return false;
		if (v2StartPos.y > size.y) return false;

		if (size.x != tiles.GetLength(0)) return false;
		if (size.y != tiles.GetLength(1)) return false;

		return true;
	}

	public TileConnections GetTileConnections(int x, int y, int id)
	{
		TileConnections connections = new TileConnections();

		for (int checkY = -1; checkY <= 1; checkY++)
		{
			for (int checkX = -1; checkX <= 1; checkX++)
			{
				if (checkX == 0 && checkY == 0) continue;
				if (Mathf.Abs(checkX) + Mathf.Abs(checkY) > 1) continue;

				int reletiveCheckX = x + checkX;
				int reletiveCheckY = y + checkY;

				int tile = GetTile(reletiveCheckX, reletiveCheckY);
				if (tile == -1) connections.SetConnection(checkX, checkY, true);
				connections.SetConnection(checkX, checkY, tile == id);
			}
		}

		return connections;
	}
	#endregion

	#region Saving and Loading
	public void UnpackData()
	{
		tiles = new int[size.x, size.y];

		int read = 0;
		for (int y = 0; y < size.y; y++)
		{
			for (int x = 0; x < size.x; x++)
			{
				tiles[x, y] = serializableTiles[read++];
			}
		}
	}

	public void PackData()
	{
		int write = 0;
		for (int y = 0; y <= tiles.GetUpperBound(1); y++)
		{
			for (int x = 0; x <= tiles.GetUpperBound(0); x++)
			{
				serializableTiles[write++] = tiles[x, y];
			}
		}
	}
	#endregion

	#region TileFunctions
	public int GetTile(int x, int y)
	{
		if (x >= 0 && x < size.x && y >= 0 && y < size.y) return tiles[x, y];
		return -1;
	}

	public int GetTile(Vector2Int position) => GetTile(position.x, position.y);

	public void SetTile(int x, int y, int id)
	{
		if (x < 0 && x >= size.x && y < 0 && y >= size.y) return;
		tiles[x, y] = id;
	}

	public void SetTile(Vector2Int position, int id) => SetTile(position.x, position.y, id);
	#endregion
}