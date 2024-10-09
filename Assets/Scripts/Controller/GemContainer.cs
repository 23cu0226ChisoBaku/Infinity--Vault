using UnityEngine;

public abstract class GemContainer : ItemContainer
{
    protected EGemType _gemType;

    public override void OnPick(IItemGetable getable)
    {
        if (getable.IsAlive())
        {
            getable.GetItem(_itemInstance.Info);
        }
        Destroy(gameObject,2);
    }
}