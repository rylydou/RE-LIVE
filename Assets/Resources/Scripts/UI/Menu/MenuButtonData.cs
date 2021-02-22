using UnityEngine;

public struct MenuButtonData
{
	public string title;
	public System.Action onClick;

	public MenuButtonData(string title, System.Action onClick)
	{
		this.title = title;
		this.onClick = onClick;
	}
}