using UnityEngine;

public class EMSetter : MonoBehaviour
{
    EnemyManager manager;
    void Start()
    {
        manager = FindFirstObjectByType<EnemyManager>();
    }
    public void DestroyManager()
    {
        if (manager != null)
            Destroy(manager.gameObject);
        else
            Debug.LogWarning("메니저를 찾을 수 없습니다!");
    }
}
