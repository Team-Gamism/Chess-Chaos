using UnityEngine.Events;

public class Skin : TweenEventer
{
    private SceneTransition transition;
    public UnityEvent OnTransitionStart;
    public UnityEvent OnTransitionComplete;

    private void Start()
    {
        transition = FindObjectOfType<SceneTransition>();
    }

    public override void Eventer()
    {
        //기존에 있던 이벤트를 지우고 원하는 이벤트를 삽입
        //예시 : 선택 패널에서 랭킹 패널로 이동
        transition.LoadCanvas(2, OnTransitionComplete);
        OnTransitionStart?.Invoke();
    }
}
