using UnityEngine;

namespace RELIVE.Element
{
	public abstract class Element : MonoBehaviour
	{
		// Public Data
		[NaughtyAttributes.ReadOnly] public ElementData data;
		[NaughtyAttributes.ReadOnly] public int index;

		protected virtual void Awake()
		{
			OnReset();
		}

		protected virtual void OnReset()
		{
			UpdatePerams();

			transform.position = data.position;
		}

		public void UpdatePerams()
		{
			data = StageBuilder.current.data.elements[index];
		}
	}
}