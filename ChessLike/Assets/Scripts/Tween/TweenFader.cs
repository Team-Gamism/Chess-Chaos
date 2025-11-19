using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class TweenFader : MonoBehaviour
{
	[SerializeField]
	private CanvasGroup canvasGroup;
	public bool PlayOnEnable;

	public UnityEvent FadeInCompleteEvent;
	public UnityEvent FadeOutCompleteEvent;
	public UnityEvent FadeInStartEvent;
	public UnityEvent FadeOutStartEvent;

	[SerializeField]
	private float duration;
	[SerializeField]
	private float fadeInDelay = 0f;
	[SerializeField]
	private float fadeOutDelay = 0f;

	private void OnEnable()
	{
		if(PlayOnEnable)
			FadeIn(duration);
	}

	public void FadeIn(float duration)
	{
		if (canvasGroup != null)
		{
			DOTween.To(() => canvasGroup.alpha, 
				x => canvasGroup.alpha = x, 1f, 
				duration == 0 ? this.duration : duration)
				.SetDelay(fadeInDelay)
				.OnComplete(() => OnFadeComplete(FadeInCompleteEvent))
			.OnStart(() => OnFadeStart(FadeInStartEvent));
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
			DOTween.To(() => canvasGroup.alpha, 
				x => canvasGroup.alpha = x, 0f,
				duration == 0 ? this.duration : duration)
				.SetDelay(fadeOutDelay)
				.OnComplete(() => OnFadeComplete(FadeOutCompleteEvent))
			.OnStart(() => OnFadeStart(FadeOutStartEvent));
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

	public void OnFadeStart(UnityEvent e)
	{
		if (e == null) return;
		e?.Invoke();
	}
}
