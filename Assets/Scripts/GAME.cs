#pragma warning disable 649
using UnityEngine;

public class GAME : MonoBehaviour
{
	// Singleton
	static GAME m_current;
	public static GAME current { get => m_current; }

	void Awake()
	{
		if (m_current != null)
			Destroy(gameObject);

		m_current = this;
		DontDestroyOnLoad(gameObject);

		transform.SendMessage("Ready", SendMessageOptions.DontRequireReceiver);
	}
}