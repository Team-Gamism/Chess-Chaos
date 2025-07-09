using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    [Header("상대 정보")]
    public string EnemyName;
    public Sprite EnemyIcon;
    [Header("인게임 상대 정보")]
    public int ELO = 1200;

    public readonly int DefaultELO = 700;

    //디버프 수열
    public readonly int[] DecSeq = { 10, 20, 40, 50, 100 };

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            SetDefault();
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

    public void SetDefault()
    {
        ELO = DefaultELO;
    }
}
