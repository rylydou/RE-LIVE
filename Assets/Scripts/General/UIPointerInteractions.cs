using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIPointerInteractions : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
	public UnityEvent<PointerEventData> onPointerEnter = new UnityEvent<PointerEventData>();
	public UnityEvent<PointerEventData> onPointerExit = new UnityEvent<PointerEventData>();
	public UnityEvent<PointerEventData> onPointerClick = new UnityEvent<PointerEventData>();
	public UnityEvent<PointerEventData> onPointerUp = new UnityEvent<PointerEventData>();
	public UnityEvent<PointerEventData> onPointerDown = new UnityEvent<PointerEventData>();

	[HideInInspector] public RectTransform rt;

	void Awake() => rt = GetComponent<RectTransform>();

	public void OnPointerEnter(PointerEventData eventData) => onPointerEnter.Invoke(eventData);

	public void OnPointerExit(PointerEventData eventData) => onPointerExit.Invoke(eventData);

	public void OnPointerClick(PointerEventData eventData) => onPointerClick.Invoke(eventData);

	public void OnPointerUp(PointerEventData eventData) => onPointerUp.Invoke(eventData);

	public void OnPointerDown(PointerEventData eventData) => onPointerDown.Invoke(eventData);
}