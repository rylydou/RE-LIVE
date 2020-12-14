using UnityEngine;

[RequireComponent(typeof(Mover2D))]
public class Player : MonoBehaviour
{
	// Perams
	[SerializeField] Transform tFeetPos;
	[Space]
	[SerializeField] GameObject pfJumpEffect;
	[SerializeField] GameObject pfLandEffect;

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
		controls.Player.Reload.performed += (x) => GAME.current.Reload();

		GAME.current.onRespawn.AddListener(() => Destroy());

		mover.bIsControlable = true;
		mover.onJump.AddListener(() => Util.TryInstantiate(pfJumpEffect, tFeetPos.position, Quaternion.identity));
		mover.onLand.AddListener(() => Util.TryInstantiate(pfLandEffect, tFeetPos.position, Quaternion.identity));

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
			Die();
		}

		// Animation
		if (!Mathf.Approximately(mover.v2MoveInput.x, 0)) transform.localScale = new Vector3(mover.v2MoveInput.x > 0 ? 1 : -1, 1, 1);

		transform.position = new Vector2(Mathf.Clamp(transform.position.x, 0, StageGen.current.data.size.x), Mathf.Clamp(transform.position.y, 0, StageGen.current.data.size.y));
	}

	void OnDisable()
	{
		controls.Disable();
	}

	void Destroy()
	{
		Destroy(gameObject);
	}

	public void Die()
	{
		GAME.current.Respawn();
	}

	void OnGUI()
	{
		GUI.Box(new Rect(0, 0, 48, 24), data.steps.Count.ToString());
	}
}