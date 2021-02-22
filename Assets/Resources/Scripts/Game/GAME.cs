using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using RELIVE.Stage;
using RELIVE.UI;

namespace RELIVE
{
	public enum PointerPosition
	{
		Offscreen,
		Game,
		UI
	}

	public class GAME : MonoBehaviour
	{
		// Singleton
		static GAME m_current;
		public static GAME current { get => m_current; }

		// Perams
		[SerializeField] UIPointerInteractions gameRender;
		[SerializeField] RectTransform rtPointer;
		[SerializeField] RectTransform rtGameRender;
		[Space]
		[SerializeField] GameObject pfPlayer;
		[SerializeField] GameObject pfGhost;

		// Public Data
		bool m_PAUSED = false;
		[HideInInspector] public bool PAUSED { get => m_PAUSED; set { m_PAUSED = value; onPauseToggled.Invoke(m_PAUSED); } }
		[HideInInspector] public Player player;
		[HideInInspector] public UnityEvent<bool> onPauseToggled = new UnityEvent<bool>();
		[HideInInspector] public UnityEvent onStart = new UnityEvent();
		[HideInInspector] public UnityEvent onRespawn = new UnityEvent();

		// Data
		[HideInInspector] public List<GhostData> datas = new List<GhostData>();

		// Input
		Controls controls;
		[HideInInspector] public PointerPosition pointerPosition;
		[HideInInspector] public Vector2 v2MouseWorldPos;
		[HideInInspector] public Vector2 v2MouseScreenPos;

		void OnEnable()
		{
			if (m_current != null)
				Destroy(gameObject);
			m_current = this;
			DontDestroyOnLoad(gameObject);

			gameRender.onPointerEnter.AddListener((x) => pointerPosition = PointerPosition.Game);
			gameRender.onPointerExit.AddListener((x) => pointerPosition = PointerPosition.UI);

			onPauseToggled.AddListener((x) => Time.timeScale = x ? 0 : 1);

			controls = new Controls();
			controls.General.Menu.performed += (x) => PAUSED = !PAUSED;
			controls.General.ToggleFullscreen.performed += (x) => Screen.fullScreen = !Screen.fullScreen;
			// TODO: Make this not suck
			controls.General.MousePosition.performed += (x) =>
			{
				v2MouseScreenPos = x.ReadValue<Vector2>();

				v2MouseWorldPos = (v2MouseScreenPos / new Vector2(Screen.width, Screen.height)) * (Camera.main.orthographicSize * new Vector2((float)Screen.width / (float)Screen.height * 2, (float)Screen.width / (float)Screen.height * 1.1f));
			};
			controls.Enable();

			transform.SendMessage("Ready", SendMessageOptions.DontRequireReceiver);

			onPauseToggled.Invoke(m_PAUSED);

			Respawn();
		}

		void OnDisable() => controls.Disable();

		public void Reload()
		{
			onRespawn.Invoke();

			datas.Clear();

			SpawnThings();

			onStart.Invoke();
		}

		public void Respawn()
		{
			if (player) datas.Add(player.data);

			SpawnThings();

			onStart.Invoke();
		}

		public void SpawnThings()
		{
			onRespawn.Invoke();

			for (int i = 0; i < datas.Count; i++)
			{
				Transform ghost = Instantiate(pfGhost, StageBuilder.current.data.v2StartPos, Quaternion.identity).transform;
				ghost.GetComponent<Ghost>().ghostIndex = i;
			}

			player = Instantiate(pfPlayer, StageBuilder.current.data.v2StartPos, Quaternion.identity).GetComponent<Player>();
		}
	}
}