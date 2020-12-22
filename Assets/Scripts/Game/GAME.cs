using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
	[HideInInspector] public Player player;
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

		controls = new Controls();
		// TODO: Make this not suck
		controls.General.MousePosition.performed += (x) =>
		{
			v2MouseScreenPos = x.ReadValue<Vector2>();

			v2MouseWorldPos = (v2MouseScreenPos / new Vector2(Screen.width, Screen.height)) * (Camera.main.orthographicSize * new Vector2((float)Screen.width / (float)Screen.height * 2, (float)Screen.width / (float)Screen.height * 1.1f));
		};
		controls.Enable();

		transform.SendMessage("Ready", SendMessageOptions.DontRequireReceiver);

		Respawn();
	}

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

	// void OnGUI()
	// {
	// 	GUI.Box(new Rect(0, 0, 64, 24), pointerPosition.ToString());
	// }

	void OnDisable() => controls.Disable();
}