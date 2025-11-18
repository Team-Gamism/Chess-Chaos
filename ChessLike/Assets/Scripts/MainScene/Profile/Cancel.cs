using UnityEngine;
using UnityEngine.Events;

public class Cancel : MonoBehaviour
{
    public UnityEvent OnIconCanceled = new UnityEvent();

    public void CancelProfile()
    {
        OnIconCanceled?.Invoke();
    }
}
