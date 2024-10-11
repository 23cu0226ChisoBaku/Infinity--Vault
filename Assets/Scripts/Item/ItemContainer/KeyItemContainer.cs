using UnityEngine;

public class KeyItemContainer : ItemContainer
{
    public override void InitProduct()
    {
        _itemName = "Key_Improtant";
    }

    public override void OnPick(IItemGetable getable)
    {
        if (getable.IsAlive())
        {
            getable.GetItem(this);
        }
    }
}