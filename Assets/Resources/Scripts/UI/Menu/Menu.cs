using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace RELIVE.UI
{
	public class Menu : MonoBehaviour
	{
		// Perams
		[SerializeField] TMP_Text textHeader;
		[Space]
		[SerializeField] GameObject pfButton;

		// Public Data
		public Stack<MenuData> menus = new Stack<MenuData>();

		public void Regenerate()
		{
			foreach (Transform button in transform) if (button.name != "Header") Destroy(button.gameObject);

			textHeader.text = $"[ {menus.Peek().sHeader} ]";

			foreach (var button in menus.Peek().buttons)
			{
				MenuButton menuButton = Instantiate(pfButton, transform).GetComponent<MenuButton>();
				menuButton.text.text = button.title;
				menuButton.onClick.AddListener(() => button.onClick.Invoke());
			}

			if (menus.Count > 1)
			{
				MenuButton backButton = Instantiate(pfButton, transform).GetComponent<MenuButton>();
				backButton.text.text = "< Back";
				backButton.onClick.AddListener(() => { menus.Pop(); Regenerate(); });
			}
		}

		public void OpenMenu(MenuData menu)
		{
			menus.Push(menu);
			Regenerate();
		}
	}
}