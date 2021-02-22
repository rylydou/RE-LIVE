using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;

namespace RELIVE.UI
{
	public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
	{
		// Perams
		public float fTrasitionTime;
		public Color normalColor;
		public Color hoveredColor;
		[Space]
		public TMP_Text text;
		[Space]
		public UnityEvent onClick;

		public void OnPointerEnter(PointerEventData eventData)
		{
			text.DOColor(hoveredColor, fTrasitionTime);
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			text.DOColor(normalColor, fTrasitionTime);
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			onClick.Invoke();
		}

		void OnValidate()
		{
			text.color = normalColor;
		}
	}
}