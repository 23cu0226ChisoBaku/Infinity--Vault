using UnityEngine;

public abstract class ConsumeItemContainer : ItemContainer
{
    protected Sprite _itemSprite;
    public Sprite ItemSprite => _itemSprite;
    public override void OnPick(IItemGetable getable)
    {
        if (getable.IsAlive())
        {
            getable.GetItem(this);
        }
    }
}