using UnityEngine;
using UnityEngine.Events;

public class GameStart : TweenEventer
{
	private TweenFader tweenFader;

	[SerializeField]
	private float duration;

	[SerializeField]
	private UnityEvent e;
	private void Start()
	{
		tweenFader = GetComponentInParent<TweenFader>();
	}

	public override void Eventer()
	{
		//기존에 있던 이벤트를 지우고 원하는 이벤트를 삽입
		//예시 : 선택 패널에서 랭킹 패널로 이동
		tweenFader.FadeOutCompleteEvent = e;
		tweenFader.FadeOut(duration);
	}
}
