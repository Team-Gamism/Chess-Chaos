using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem particle;
    private bool isOn = false;
    public string fileName;
    void Start()
    {
        Application.targetFrameRate = 65;
        QualitySettings.vSyncCount = 1;
        GetComponent<NNUEFileManager>().AddNNUEFile(fileName);
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
