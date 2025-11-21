using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

// 화면(씬 -> 씬, 캔버스 -> 캔버스 등)을 전환할 때 필요한 메서드들을 모아놓은 클래스입니다.
public class SceneTransition : MonoBehaviour
{
    private Animator animator;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private CanvasManager canvasManager;
    private void Start()
    {
        canvas = GetComponent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas.worldCamera = Camera.main;
        animator = GetComponent<Animator>();
        canvasManager = FindFirstObjectByType<CanvasManager>();
    }
    public void LoadScene(string scene)
    {
        StartCoroutine(loadScene(scene));
    }
    public void LoadCanvas(int n, UnityEvent OnTransitionComplete = null)
    {
        StartCoroutine(loadCanvas((CanvasType)n, OnTransitionComplete));
    }
    public void LoadCanvas(int n)
    {
        StartCoroutine(loadCanvas((CanvasType)n));
    }
    private IEnumerator loadScene(string scene)
    {
        animator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(scene);
    }

    private IEnumerator loadCanvas(CanvasType canvasType, UnityEvent e)
    {
        animator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(0.5f);
        canvasManager.LoadCanvas(canvasType);
        animator.SetTrigger("FadeIn");
        e?.Invoke();
    }
    private IEnumerator loadCanvas(CanvasType canvasType)
    {
        animator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(0.5f);
        canvasManager.LoadCanvas(canvasType);
        animator.SetTrigger("FadeIn");
    }
    public void SetInteractive(int n)
    {
        canvasGroup.blocksRaycasts = n > 0 ? true : false;
    }
}
