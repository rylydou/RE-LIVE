using UnityEngine;
using UnityEngine.UI;

public enum EditorMode
{
	None,
	Tile,
	Wire,
	Element
}

public class StageEditor : MonoBehaviour
{
	// Singleton
	static StageEditor m_current;
	public static StageEditor current { get => m_current; }

	// Perams
	[SerializeField] Transform tTilePlacement;
	[SerializeField] Transform tTilesContainer;
	[Space]
	[SerializeField] GameObject pfTile;

	// Data
	int iActivePalette = 1;

	// Input
	Controls controls;
	Vector2Int v2iMousePos;
	bool bIsPlacing;
	bool bIsErasing;

	void Ready()
	{
		m_current = this;

		controls = new Controls();
		controls.Editor.Select.performed += (x) => bIsPlacing = true;
		controls.Editor.Select.canceled += (x) => bIsPlacing = false;
		controls.Editor.Erase.performed += (x) => bIsErasing = true;
		controls.Editor.Erase.canceled += (x) => bIsErasing = false;

		RefreshTilesUI();
	}

	void Update()
	{
		if (GAME.current.pointerPosition == PointerPosition.Game)
		{
			v2iMousePos = Util.ToVector2Int(GAME.current.v2MouseWorldPos - Vector2.one / 2);

			tTilePlacement.position = (Vector2)v2iMousePos + Vector2.one / 2;

			if (!(bIsPlacing && bIsErasing))
			{
				if (bIsPlacing) PlaceTile(v2iMousePos);
				else if (bIsErasing) EraseTile(v2iMousePos);
			}
		}
		else
		{
			tTilePlacement.position = -Vector2.one;
		}
	}

	void PlaceTile(Vector2Int position)
	{
		StageBuilder.current.data.SetTile(position, iActivePalette);
		StageBuilder.current.RefreshTile(position, true);
	}

	void EraseTile(Vector2Int position)
	{
		StageBuilder.current.data.SetTile(position, 0);
		StageBuilder.current.RefreshTile(position, true);
	}

	public void RefreshTilesUI()
	{
		foreach (Transform tile in tTilesContainer) Destroy(tile);

		foreach (TilePalette palette in DataManager.current.tilePalettes)
		{
			if (palette.name == "None") continue;

			Transform tileObj = Instantiate(pfTile, tTilesContainer).transform;

			var button = tileObj.GetComponent<UnityEngine.UI.Button>();

			button.onClick.AddListener(() => SetActivePalette(tileObj.GetSiblingIndex() + 1));

			(button.targetGraphic as Image).sprite = palette.spIcon;
		}
	}

	public void SetActivePalette(int id)
	{
		iActivePalette = id;
	}

	void OnEnable() => controls.Enable();

	void OnDisable() => controls.Disable();
}