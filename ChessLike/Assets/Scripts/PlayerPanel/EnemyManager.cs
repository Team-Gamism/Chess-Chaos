using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
     [Header("상대 정보")]
    public string EnemyName;
    public Sprite EnemyIcon;
     [Header("인게임 상대 정보")]
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
