using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem particle;
    void Start()
    {
        AtlasManager.instance.SetParticleTexture(particle);
    }
}
