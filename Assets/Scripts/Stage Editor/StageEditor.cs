using UnityEngine;
using UnityEngine.UI;

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

	void Ready()
	{
		m_current = this;

		controls = new Controls();
		controls.Editor.Select.performed += (x) => PlaceTile(v2iMousePos);
		controls.Editor.Erase.performed += (x) => EraseTile(v2iMousePos);

		RefreshTilesUI();
	}

	void Update()
	{
		v2iMousePos = Util.ToVector2Int(GAME.current.v2MouseWorldPos - Vector2.one / 2);

		tTilePlacement.position = (Vector2)v2iMousePos + Vector2.one / 2;
	}

	void PlaceTile(Vector2Int position)
	{
		StageGen.current.data.SetTile(position, iActivePalette);
		StageGen.current.RefreshTile(position, true);
	}

	void EraseTile(Vector2Int position)
	{
		StageGen.current.data.SetTile(position, 0);
		StageGen.current.RefreshTile(position, true);
	}

	public void RefreshTilesUI()
	{
		foreach (Transform tile in tTilesContainer) Destroy(tile);

		foreach (TilePalette palette in PaletteManager.current.palettes)
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