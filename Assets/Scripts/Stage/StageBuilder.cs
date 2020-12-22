using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

public class StageBuilder : MonoBehaviour
{
	// Singleton
	static StageBuilder m_current;
	public static StageBuilder current { get => m_current; }

	// Perams
	[SerializeField] Vector2Int v2iSize;
	[SerializeField] string sStageName;
	[Space]
	[SerializeField] string extention = ".relive";
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

	public void RefreshTile(Vector2Int position, bool bUpdateSurroundingTiles = false)
	{
		if (position.x < 0 && position.x >= data.size.x && position.y < 0 && position.y >= data.size.y) return;

		if (data.GetTile(position) == 0)
			groundTilemap.SetTile((Vector3Int)position, null);
		else
		{
			int id = data.GetTile(position.x, position.y);

			groundTilemap.SetTile((Vector3Int)position, DataManager.current.tilePalettes[id].FindTile(data.GetTileConnections(position.x, position.y, id)).tile);
		}

		if (!bUpdateSurroundingTiles) return;

		for (int y = -1; y <= 1; y++)
		{
			for (int x = -1; x <= 1; x++)
			{
				RefreshTile(position + new Vector2Int(x, y), false);
			}
		}
	}

	public void RefreshTile(int x, int y, bool bUpdateSurroundingTiles = false) => RefreshTile(new Vector2Int(x, y), bUpdateSurroundingTiles);
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

		// var dicLol = new System.Collections.Generic.Dictionary<string, float>();
		// dicLol.Add("size_x", 20f);
		// dicLol.Add("size_y", 1f);
		// data.elements.Add(new ElementData(1, Vector2.zero, dicLol));
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