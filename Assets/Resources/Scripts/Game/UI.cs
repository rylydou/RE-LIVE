using UnityEngine;

namespace RELIVE.UI
{
	public class UI : MonoBehaviour
	{
		// Singleton
		static UI m_current;
		public static UI current { get => m_current; }

		// Perams
		[SerializeField] GameObject goMenu;
		[SerializeField] Menu menu;

		void Ready()
		{
			m_current = this;

			GAME.current.onPauseToggled.AddListener((x) =>
			{
				if (x)
				{
					var menuOptionsDisplay = new MenuData("Display", new (string, System.Action)[]
					{
						("Window Mode: Fullscreen", null),
						("Resolution: 1920x1080", null),
						("V-Sync: Every V-Blank", null),
						("Use System Cursor: Off", null),
					});

					var menuOptionsGraphics = new MenuData("Graphics", new (string, System.Action)[]
					{
						("UI Blur: On", null),
						("Post-Processing: On", null),
						("Particles: Normal", null),
					});

					var menuOptions = new MenuData("Options", new (string, System.Action)[]
					{
						("Audio", null),
						("Display", () => menu.OpenMenu(menuOptionsDisplay)),
						("Graphics", () => menu.OpenMenu(menuOptionsGraphics)),
						("Accessibility", null),
				});

					var menuMain = new MenuData("Menu", new (string, System.Action)[]
					{
						("Resume", () => GAME.current.PAUSED = false),
						("Options", () => menu.OpenMenu(menuOptions)),
						("Quit", () => { Debug.Log("Quitting..."); Application.Quit(); }),
					});

					menu.menus.Clear();
					menu.OpenMenu(menuMain);
				}

				goMenu.SetActive(x);
			});
		}
	}
}