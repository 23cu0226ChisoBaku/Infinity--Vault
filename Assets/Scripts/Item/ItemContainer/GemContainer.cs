using UnityEngine;

public abstract class GemContainer : ItemContainer
{
    public EGemType GemType {get;protected set;}
    public int Worth {get;protected set;}

    public override void OnPick(IItemGetable getable)
    {
        if (getable.IsAlive())
        {
            getable.GetItem(this);
        }
        Destroy(gameObject);
    }

}