using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class TweenFader : MonoBehaviour
{
	[SerializeField]
	private CanvasGroup canvasGroup;

	public UnityEvent FadeInCompleteEvent;
	public UnityEvent FadeOutCompleteEvent;

	[SerializeField]
	private float Duration;
	[SerializeField]
	private float FadeInDelay = 0f;
	[SerializeField]
	private float FadeOutDelay = 0f;

	private void OnEnable()
	{
		FadeIn(Duration);
	}

	public void FadeIn(float duration)
	{
		if (canvasGroup != null)
		{
			DOTween.To(() => canvasGroup.alpha, x => canvasGroup.alpha = x, 1f, duration).SetDelay(FadeInDelay).OnComplete(() => OnFadeComplete(FadeInCompleteEvent));
		}
		else
		{
			Debug.LogWarning("CanvasGroup이 없습니다!");
			return;
		}
	}

	public void FadeOut(float duration)
	{
		if (canvasGroup != null)
		{
			DOTween.To(() => canvasGroup.alpha, x => canvasGroup.alpha = x, 0f, duration).SetDelay(FadeOutDelay).OnComplete(() => OnFadeComplete(FadeOutCompleteEvent));
		}
		else
		{
			Debug.LogWarning("CanvasGroup이 없습니다!");
			return;
		}
	}

	public void OnFadeComplete(UnityEvent e)
	{
		if (e == null) return;
		e?.Invoke();
	}
}
