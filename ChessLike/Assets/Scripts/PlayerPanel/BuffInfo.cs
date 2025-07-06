using UnityEngine.Events;

public class BuffInfo
{
    public string BuffName;
    public BuffType Buff;
    public BuffTarget Target;
    public UnityEvent BuffEvent;
    public void ExecuteBuff()
    {
        
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