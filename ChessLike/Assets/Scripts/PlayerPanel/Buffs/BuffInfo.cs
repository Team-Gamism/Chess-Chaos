using UnityEngine.Events;

public class BuffInfo
{
    // PM/EM에 있는 함수 중 하나를 가져와서 조정하면 될듯
    // 아니면 PM/EM에서 어떤 타입의 버프인지 확인한 후 그 내용에 맞게 수정하는것도 괜찮을지도

    // PM/EM에서 큐에 저장되어 있는 버프들을 가져온 후, 오버라이드한 ExecuteBuff를 실행시키면 될 듯
    // 게임이 끝나면 큐 제외 모든 내용을 기본으로 돌린 후, 큐 내용을 다시 적용시키는 방식을 사용할 예정
    // 아니면 기본 상태를 저장해놓고 게임이 끝날 때마다 기본 상태로 되돌리는 것도 괜찮을듯
    public string BuffName;
    public BuffType Buff;
    public BuffTarget Target;
    public BuffInfo(string BuffName, BuffType type, BuffTarget target)
    {
        this.BuffName = BuffName;
        Buff = type;
        Target = target;
    }
    public void ExecuteBuff(UnityEvent buff)
    {
        buff?.Invoke();
    }
}
public enum BuffType
{
    Buff,
 
    Debuff
}

public enum BuffTarget
{
    Player,
    Enemy
}