using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GAME : MonoBehaviour
{
	// Singleton
	static GAME m_current;
	public static GAME current { get => m_current; }

	// Perams
	[SerializeField] GameObject pfPlayer;
	[SerializeField] GameObject pfGhost;

	// Public Data
	[HideInInspector] public Player player;
	[HideInInspector] public UnityEvent onStart = new UnityEvent();
	[HideInInspector] public UnityEvent onRespawn = new UnityEvent();

	// Data
	[HideInInspector] public List<GhostData> datas = new List<GhostData>();

	void Awake()
	{
		if (m_current != null)
			Destroy(gameObject);

		m_current = this;
		DontDestroyOnLoad(gameObject);

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
}