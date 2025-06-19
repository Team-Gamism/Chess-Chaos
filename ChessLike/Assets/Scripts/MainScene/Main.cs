using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem particle;
    private bool isOn = false;
    void Start()
    {
        Application.targetFrameRate = 65;
        UpdateParticle();
        isOn = true;
    }
    void OnEnable()
    {
        if (isOn) UpdateParticle();
    }

    private void UpdateParticle()
    {
        AtlasManager.instance.SetParticleTexture(particle);
    }
}
