using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
	[SerializeField, NaughtyAttributes.ReorderableList] List<Element> triggers = new List<Element>();
	[Space(8)]
	[SerializeField] Sprite spUp;
	[SerializeField] Sprite spDown;
	[Space]
	[SerializeField] LayerMask lmTriggeredFrom;
	[SerializeField] Transform tButton;

	bool isBeingPressed = true;
	SpriteRenderer sr;

	void Awake()
	{
		sr = GetComponent<SpriteRenderer>();
	}

	void FixedUpdate()
	{
		bool tmpState = Physics2D.OverlapBox(tButton.position, tButton.localScale, tButton.eulerAngles.z, lmTriggeredFrom);

		if (tmpState != isBeingPressed)
		{
			isBeingPressed = tmpState;
			if (isBeingPressed)
			{
				sr.sprite = spDown;
			}
			else
			{
				sr.sprite = spUp;
			}

			triggers.ForEach((x) => x.Toggle());
		}
		else isBeingPressed = tmpState;
	}

	void OnDrawGizmos()
	{
		if (!tButton) return;

		Gizmos.color = new Color(0, 1, 0, 0.5f);

		Gizmos.DrawWireCube(tButton.position, tButton.localScale);
	}
}