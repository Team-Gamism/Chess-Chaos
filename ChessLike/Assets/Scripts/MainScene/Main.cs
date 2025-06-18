using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem particle;
    void Start()
    {
        Application.targetFrameRate = 65;
        AtlasManager.instance.SetParticleTexture(particle);
    }
}
