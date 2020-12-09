#pragma warning disable 649
using UnityEngine;

public class WaterAnim : MonoBehaviour
{
	// Perams
	[SerializeField] GameObject pfWaterSlice;
	[SerializeField] Vector2 v2WaveSize;
	[SerializeField] float fScrollSpeed;
	[SerializeField] int iSlicesAmount;

	// Data
	float fOffset;

	void Awake()
	{
		for (int i = -iSlicesAmount / 2; i < iSlicesAmount / 2; i++)
		{
			Instantiate(pfWaterSlice, transform.position + new Vector3(i, 0, 0), Quaternion.identity, transform);
		}
	}

	void Update()
	{
		float i = 0;
		foreach (Transform slice in transform)
		{
			float y = Mathf.Sin((fOffset + i) * v2WaveSize.x) * v2WaveSize.y;

			slice.localPosition = new Vector2(slice.localPosition.x, y);

			i += fScrollSpeed;
		}

		fOffset += Time.deltaTime * fScrollSpeed;
	}
}