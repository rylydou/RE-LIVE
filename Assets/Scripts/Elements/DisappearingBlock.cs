using UnityEngine;

public class DisappearingBlock : Element
{
	// Preams
	[SerializeField] Vector2 size;
	[Space(8)]
	[SerializeField] LayerMask lmPlayer;
	[SerializeField] Vector2 v2CheckRemoveSize;
	[Space]
	[SerializeField] BoxCollider2D boxCollider;
	[SerializeField] SpriteRenderer srBlock;
	[SerializeField] SpriteRenderer srOutline;

	void OnValidate()
	{
		boxCollider.size = size;
		srBlock.size = size;
		srOutline.size = size;
	}

	public override void OnToggle()
	{
		base.OnToggle();

		srBlock.gameObject.SetActive(state);
	}

	protected override void OnToggleOn()
	{
		var hit = Physics2D.OverlapBox(transform.position, size - v2CheckRemoveSize, transform.eulerAngles.z, lmPlayer);

		if (hit && hit.CompareTag("Player"))
		{
			GAME.current.player.Die();
		}
	}

	void OnDrawGizmos()
	{
		Gizmos.color = new Color(1, 0, 0, 0.5f);

		Gizmos.DrawWireCube(transform.position, size - v2CheckRemoveSize);
	}
}