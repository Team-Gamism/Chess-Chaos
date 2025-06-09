using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class TweenMover : MonoBehaviour
{
	[SerializeField]
	private float Duration;

	[SerializeField]
	private float Delay;

	[SerializeField]
	private GameObject MoveObject;

	[SerializeField]
	private RectTransform PosRect;

	[SerializeField]
	private UnityEvent MoveEndEvent;

	public Ease ease;

	private RectTransform rectTransform;

	private Vector2 vec;

	private bool isInit = false;

	private void OnEnable()
	{
		if (!isInit) Init();
		else MoveTo(Duration);
	}

	private void Init()
	{
		rectTransform = MoveObject.GetComponent<RectTransform>();
		vec = rectTransform.anchoredPosition;

		isInit = true;

		MoveTo(Duration);
	}

	public void MoveTo(float duration)
	{
		if (isInit) rectTransform.anchoredPosition = vec;
		rectTransform.DOAnchorPos(PosRect.anchoredPosition, duration).OnComplete(() => OnMoveEnd(MoveEndEvent)).SetDelay(Delay).SetEase(ease);
	}
	
	private void OnMoveEnd(UnityEvent e)
	{
		if (e == null) return;
		e?.Invoke();
	}

}
