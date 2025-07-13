using UnityEngine;

public class LevelCreator : MonoBehaviour
{
    public GameObject EnemyManager;
    private EnemyManager manager;
    void OnEnable()
    {
        manager = Instantiate(EnemyManager).GetComponent<EnemyManager>();
    }

    public void SetLevel(int n)
    {
        manager.SetLevel(n);
    }
}
