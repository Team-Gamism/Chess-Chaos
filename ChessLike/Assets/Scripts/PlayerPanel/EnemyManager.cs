using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    public string EnemyName;
    public int ELO = 1200;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void AddELO(int value)
    {
        ELO += value;
    }
}
