using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

public class StageGen : MonoBehaviour
{
	// Singleton
	static StageGen m_current;
	public static StageGen current { get => m_current; }

	// Perams
	[SerializeField] Vector2Int v2iSize;
	[SerializeField] string sStageName;
	[Space]
	[SerializeField] string extention = ".relive";
	[Space]
	[SerializeField] TilePallet pallet;
	[SerializeField] TileBase tileTest;
	[Space]
	[SerializeField] Tilemap groundTilemap;
	[SerializeField] Transform tStage;
	[SerializeField] GameObject pfTestObject;

	// Public Data
	[HideInInspector] public StageData data;

	void Ready()
	{
		m_current = this;

		Load();
	}

	#region Generation
	public void BuildStage()
	{
		if (!data.Check())
		{
			Debug.LogError("Stage Data is invalid!");
			return;
		}

		foreach (Transform child in tStage) Destroy(child.gameObject);

		groundTilemap.ClearAllTiles();

		for (int y = 0; y < data.tiles.GetLength(1); y++)
		{
			for (int x = 0; x < data.tiles.GetLength(0); x++)
			{
				RefreshTile(x, y);
			}
		}
	}

	public void RefreshTile(Vector2Int position)
	{
		if (data.GetTile(position) != 0)
		{
			// groundTilemap.SetTile((Vector3Int)position, tileTest);
			groundTilemap.SetTile((Vector3Int)position, pallet.FindTile(data.GetTileConnections(position.x, position.y, 1)).tile);
		}
	}

	public void RefreshTile(int x, int y) => RefreshTile(new Vector2Int(x, y));
	#endregion

	#region Saving and Loading
	[NaughtyAttributes.Button]
	public void Save()
	{
		Save(Application.dataPath + "/" + sStageName);
	}

	[NaughtyAttributes.Button]
	public void Load()
	{
		Load(Application.dataPath + "/" + sStageName);
	}

	public void Save(string path)
	{
		if (!File.Exists(path + extention)) File.CreateText(path + extention);

		data.PackData();

		File.WriteAllText(path + extention, Serializer.Encode(data));
	}

	public bool Load(string path)
	{
		if (!File.Exists(path + extention)) return false;

		data = Serializer.Decode<StageData>(File.ReadAllText(path + extention));

		data.UnpackData();

		BuildStage();

		return true;
	}
	#endregion

	void OnDrawGizmos()
	{
		if (string.IsNullOrEmpty(data.name)) return;

		Gizmos.color = new Color(1, 0, 0, 0.75f);
		Gizmos.DrawWireCube((Vector2)v2iSize / 2, (Vector2)v2iSize);

		Gizmos.color = new Color(0, 0, 1, 0.75f);
		Gizmos.DrawWireCube(data.v2StartPos, Vector3.one);
	}
}