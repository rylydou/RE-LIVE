using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class GAME : MonoBehaviour
{
	// Singleton
	static GAME m_current;
	public static GAME current { get => m_current; }

	// Perams
	[SerializeField] RectTransform rtPointer;
	[SerializeField] RectTransform rtGameRender;
	[Space]
	[SerializeField] GameObject pfPlayer;
	[SerializeField] GameObject pfGhost;

	// Public Data
	[HideInInspector] public Player player;
	[HideInInspector] public UnityEvent onStart = new UnityEvent();
	[HideInInspector] public UnityEvent onRespawn = new UnityEvent();
	[HideInInspector] public Vector2 v2MouseWorldPos;

	// Data
	[HideInInspector] public List<GhostData> datas = new List<GhostData>();

	// Input
	Controls controls;

	void Awake()
	{
		if (m_current != null)
			Destroy(gameObject);

		m_current = this;
		DontDestroyOnLoad(gameObject);

		controls = new Controls();
		// TODO: Make this not suck
		controls.General.MousePosition.performed += (x) =>
		{
			v2MouseWorldPos = (rtPointer.position / new Vector2(Screen.width, Screen.height)) * (Camera.main.orthographicSize * new Vector2((float)Screen.width / (float)Screen.height * 2, (float)Screen.width / (float)Screen.height * 1.1f));
		};

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
			Transform ghost = Instantiate(pfGhost, StageGen.current.data.v2StartPos, Quaternion.identity).transform;
			ghost.GetComponent<Ghost>().ghostIndex = i;
		}

		player = Instantiate(pfPlayer, StageGen.current.data.v2StartPos, Quaternion.identity).GetComponent<Player>();
	}

	void OnEnable() => controls.Enable();

	void OnDisable() => controls.Disable();
}