using UnityEngine;

public class EMSetter : MonoBehaviour
{
    EnemyManager manager;
    void Start()
    {
        manager = FindObjectOfType<EnemyManager>();
    }
    public void DestroyManager()
    {
        Destroy(manager.gameObject);
    }
}
