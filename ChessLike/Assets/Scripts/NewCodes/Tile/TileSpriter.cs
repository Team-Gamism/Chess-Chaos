using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileSpriter : MonoBehaviour
{
    public List<TileAnimator> objs = new List<TileAnimator>();
    private TileAnimator tileAnimators = null;
    public void SpriteOn(HighlightType type)
    {
        tileAnimators = objs[(int)type];
        tileAnimators.Sequence();
    }
    public void SpriteOff()
    {
        tileAnimators.EndSequence();
    }
}

public enum HighlightType
{
    Select = 0,
    Move,
    Attack,
    NotEnter,
    FrontSelect
}
