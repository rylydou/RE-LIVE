#pragma warning disable 649
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
	// Public Data
	[HideInInspector] public int ghostIndex = -1;

	// Data
	bool bIsFollowingPath = true;
	int step = 0;

	// Cache
	Rigidbody2D rb;

	void Awake()
	{
		rb = GetComponent<Rigidbody2D>();

		GAME.current.onStart.AddListener(() => OnStart());
		GAME.current.onRespawn.AddListener(() => Destroy());
	}

	void FixedUpdate()
	{
		if (!bIsFollowingPath || ghostIndex == -1) return;

		if (step < GAME.current.datas[ghostIndex].steps.Count)
		{
			Vector2 position = GAME.current.datas[ghostIndex].steps[step];

			if (!Mathf.Approximately(transform.position.x, position.x))
			{
				if (position.x - transform.position.x > 0)
					transform.localScale = new Vector3(1, 1, 1);
				else
					transform.localScale = new Vector3(-1, 1, 1);
			}

			rb.position = position;
		}
		else
		{
			bIsFollowingPath = false;
		}

		step++;
	}

	void OnStart()
	{
		bIsFollowingPath = true;
	}

	void Destroy()
	{
		bIsFollowingPath = false;
		Destroy(gameObject);
	}
}