using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pointer : MonoBehaviour
{
	// Singleton
	static Pointer m_current;
	public static Pointer current { get => m_current; }

	// Parameters
	[SerializeField] int m_pointer;
	public int pointer
	{
		get => m_pointer; set
		{
			int newValue = Mathf.Clamp(value, 0, pointers.Count - 1);
			if (m_pointer == newValue) return;
			m_pointer = newValue;
			RefreshPointer();
		}
	}
	[Space]
	[SerializeField] Image imgShadow;
	[SerializeField] List<GameObject> pointers = new List<GameObject>();
	[Space]
	[SerializeField] RectTransform rtPointer;

	// Input
	Controls controls;
	Vector2 v2MousePos;

	void Ready()
	{
		m_current = this;

		controls = new Controls();
		controls.General.MousePosition.performed += (x) => v2MousePos = x.ReadValue<Vector2>();
		controls.Enable();

		RefreshPointer();
	}

	void Update()
	{
		rtPointer.position = v2MousePos;

		Cursor.visible = !(v2MousePos.x >= 0 && v2MousePos.x < Screen.width && v2MousePos.y >= 0 && v2MousePos.y < Screen.height);
	}

	void RefreshPointer()
	{
		pointers.ForEach(x => x.SetActive(false));

		pointers[pointer].SetActive(true);
	}
}