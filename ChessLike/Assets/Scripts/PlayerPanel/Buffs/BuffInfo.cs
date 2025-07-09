using UnityEngine;

public class BuffInfo : MonoBehaviour
{
    public string BuffName;
    public BuffType Buff;
    //이후 필요한 변수들은 다른 클래스에서 생성할 것
}
public enum BuffType
{
    Buff,

    Debuff
}