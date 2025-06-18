using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    private Animator animator;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private void Start()
    {
        canvas = GetComponent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas.worldCamera = Camera.main;
        animator = GetComponent<Animator>();
    }
    public void LoadScene(string scene)
    {
        StartCoroutine(loadScene(scene));
    }
    private IEnumerator loadScene(string scene)
    {
        animator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(scene);
    }
    public void SetInteractive(int n)
    {
        canvasGroup.blocksRaycasts = n > 0 ? true : false;
    }
}
