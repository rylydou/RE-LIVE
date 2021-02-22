using UnityEngine;

public struct MenuData
{
	public string sHeader;
	public MenuButtonData[] buttons;

	// public MenuData(string sHeader, MenuButtonData[] buttons)
	// {
	// 	this.sHeader = sHeader;
	// 	this.buttons = buttons;
	// }

	public MenuData(string sHeader, (string, System.Action)[] buttons)
	{
		this.sHeader = sHeader;

		this.buttons = new MenuButtonData[buttons.Length];

		int write = 0;
		foreach (var button in buttons)
		{
			this.buttons[write].title = button.Item1;
			if (button.Item2 != null) this.buttons[write].onClick = button.Item2;
			else this.buttons[write].onClick = () => Debug.Log($"Pressed \"{button.Item1}\"");

			write++;
		}
	}
}