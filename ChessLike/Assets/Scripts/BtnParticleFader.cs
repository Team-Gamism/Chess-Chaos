using UnityEngine;

public class BtnParticleFader : MonoBehaviour
{
    public CanvasGroup cg;
    public ParticleSystem particle;

    private ParticleSystem.MainModule mainModule;

    private void Start()
    {
        mainModule = particle.main;
    }

    private void Update()
    {
        Color startColor = mainModule.startColor.color;
        startColor.a = cg.alpha;
        mainModule.startColor = new ParticleSystem.MinMaxGradient(startColor);
    }
}
