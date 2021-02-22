using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using RELIVE;

namespace RELIVE.Stage.Editor
{
	public enum EditorMode
	{
		None,
		Tile,
		Element,
		Wire,
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
		public UnityEvent<EditorMode> onModeChange = new UnityEvent<EditorMode>();
		[Space]
		[SerializeField] GameObject pfTile;

		// Public Data
		EditorMode m_mode;
		public EditorMode mode { get => m_mode; set { m_mode = value; onModeChange.Invoke(m_mode); } }

		// Data
		int iActiveItem = 1;

		// Input
		Controls controls;
		Vector2Int v2iMousePos;
		bool bUsingPrimary;
		bool bIsUsingSecondary;

		void Ready()
		{
			m_current = this;

			mode = EditorMode.Tile;

			onModeChange.AddListener((x) =>
			{
				iActiveItem = 0;

				RefreshUI();

				switch (x)
				{
					case EditorMode.Tile:
						Pointer.current.pointer = 2;
						break;
					case EditorMode.Element:
						Pointer.current.pointer = 3;
						break;
					case EditorMode.Wire:
						Pointer.current.pointer = 4;
						break;
				}
			});

			controls = new Controls();
			controls.Editor.Select.performed += (x) => bUsingPrimary = true;
			controls.Editor.Select.canceled += (x) => bUsingPrimary = false;
			controls.Editor.Erase.performed += (x) => bIsUsingSecondary = true;
			controls.Editor.Erase.canceled += (x) => bIsUsingSecondary = false;
			controls.Editor.TileMode.performed += (x) => mode = EditorMode.Tile;
			controls.Editor.ElementMode.performed += (x) => mode = EditorMode.Element;
			controls.Editor.WireMode.performed += (x) => mode = EditorMode.Wire;
			controls.Editor.CycleMode.performed += (x) => { if (mode == EditorMode.Wire) mode = EditorMode.Tile; else mode++; };
		}

		void Update()
		{
			if (GAME.current.pointerPosition == PointerPosition.Game)
			{
				v2iMousePos = Util.ToVector2Int(GAME.current.v2MouseWorldPos - Vector2.one / 2);

				tTilePlacement.position = (Vector2)v2iMousePos + Vector2.one / 2;

				if (!(bUsingPrimary && bIsUsingSecondary))
				{
					if (bUsingPrimary) OnPrimaryUse(v2iMousePos);
					else if (bIsUsingSecondary) OnSecondaryUse(v2iMousePos);
				}
			}
			else
			{
				tTilePlacement.position = -Vector2.one;
			}

		}

		void OnPrimaryUse(Vector2Int position)
		{
			switch (m_mode)
			{
				case EditorMode.Tile:
					StageBuilder.current.data.SetTile(position, iActiveItem);
					StageBuilder.current.RefreshTile(position, true);
					break;
			}
		}

		void OnSecondaryUse(Vector2Int position)
		{
			switch (m_mode)
			{
				case EditorMode.Tile:
					StageBuilder.current.data.SetTile(position, 0);
					StageBuilder.current.RefreshTile(position, true);
					break;
				case EditorMode.Element:
					// // StageBuilder.current.data.elements.Add();
					break;
			}
		}

		public void RefreshItems()
		{
			foreach (Transform tile in tTilesContainer) Destroy(tile.gameObject);

			switch (mode)
			{
				case EditorMode.Tile:
					foreach (TilePalette palette in DataManager.current.tilePalettes)
					{
						if (palette.name == "None") continue;

						Transform tileObj = Instantiate(pfTile, tTilesContainer).transform;

						var button = tileObj.GetComponent<UnityEngine.UI.Button>();

						button.onClick.AddListener(() => iActiveItem = tileObj.GetSiblingIndex() + 1);

						(button.targetGraphic as Image).sprite = palette.spIcon;
					}
					break;
				case EditorMode.Element:
					foreach (GameObject element in DataManager.current.elements)
					{
						if (element == null) continue;

						Transform tileObj = Instantiate(pfTile, tTilesContainer).transform;

						var button = tileObj.GetComponent<UnityEngine.UI.Button>();

						button.onClick.AddListener(() => iActiveItem = tileObj.GetSiblingIndex() + 1);

						(button.targetGraphic as Image).sprite = element.GetComponent<ElementInfo>().icon;
					}
					break;
				case EditorMode.Wire:
					break;
			}
		}

		public void RefreshItemUI()
		{

		}

		public void RefreshUI()
		{
			RefreshItems();
			RefreshItemUI();
		}

		void OnEnable() => controls.Enable();

		void OnDisable() => controls.Disable();
	}
}