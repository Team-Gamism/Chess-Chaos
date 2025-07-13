using UnityEngine;

public class LevelCreator : MonoBehaviour
{
    public GameObject EnemyManager;
    private EnemyManager manager;
    public AudioClip SFX;
    void OnEnable()
    {
        manager = Instantiate(EnemyManager).GetComponent<EnemyManager>();
    }

    public void SetLevel(int n)
    {
        SoundManager.Instance.SFXPlay("start", SFX);
        manager.SetLevel(n);
    }
}
