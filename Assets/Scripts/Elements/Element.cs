using UnityEngine;

public abstract class Element : MonoBehaviour
{
	// Perams
	[SerializeField] protected bool initialState;
	[Space]

	// Data
	[HideInInspector] public bool state;

	protected virtual void Awake()
	{
		OnSaveState();
	}

	public void Toggle()
	{
		state = !state;

		OnToggle();

		if (state) OnToggleOn();
		else OnToggleOff();
	}

	public virtual void OnToggle()
	{
		// Not abstract b/c you don't always want to inplement it
	}

	protected virtual void OnToggleOn()
	{
		// Not abstract b/c you don't always want to inplement it
	}

	protected virtual void OnToggleOff()
	{
		// Not abstract b/c you don't always want to inplement it
	}

	protected virtual void OnSaveState()
	{
		// Not abstract b/c you don't always want to inplement it
	}

	protected virtual void OnReset()
	{
		state = initialState;
	}
}