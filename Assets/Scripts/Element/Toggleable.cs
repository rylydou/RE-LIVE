using UnityEngine;

namespace RELIVE.Element
{
	public class Toggleable : Element
	{
		// Data
		[HideInInspector] public bool state;

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

		protected override void OnReset()
		{
			base.OnReset();
		}
	}
}