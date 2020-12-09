#pragma warning disable 649
using UnityEngine;

[RequireComponent(typeof(Mover2D))]
public class Player : MonoBehaviour
{
	// Public Data
	public GhostData data;

	// Controls
	Controls controls;

	// Cache
	Mover2D mover;

	void Awake()
	{
		mover = GetComponent<Mover2D>();

		controls = new Controls();
		controls.Player.Move.performed += (x) => mover.v2MoveInput = x.ReadValue<Vector2>();
		controls.Player.Jump.performed += (x) => mover.JumpDown();
		controls.Player.Jump.canceled += (x) => mover.JumpUp();
		controls.Player.Die.performed += (x) => GAME.current.Respawn();

		GAME.current.onRespawn.AddListener(() => Destroy());

		mover.bIsControlable = true;
		data = new GhostData(0);
	}

	void OnEnable()
	{
		controls.Enable();
	}

	void FixedUpdate()
	{
		data.AddStep(transform.position);
	}

	void LateUpdate()
	{
		// Dying lol
		if (transform.position.y < Camera.main.transform.position.y - Camera.main.orthographicSize - 1)
		{
			GAME.current.Respawn();
		}

		// Animation
		if (!Mathf.Approximately(mover.v2MoveInput.x, 0)) transform.localScale = new Vector3(mover.v2MoveInput.x > 0 ? 1 : -1, 1, 1);
	}

	void OnDisable()
	{
		controls.Disable();
	}

	void OnGUI()
	{
		GUI.Box(new Rect(0, 0, 48, 24), data.steps.Count.ToString());
	}

	void Destroy()
	{
		Destroy(gameObject);
	}
}