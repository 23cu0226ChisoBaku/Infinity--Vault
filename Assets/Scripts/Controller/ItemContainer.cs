using UnityEngine;

public abstract class ItemContainer : MonoBehaviour, IPickable
{
    protected Item _itemInstance;
    public ItemInfo Info => _itemInstance.Info;

    public abstract void OnPick(IItemGetable getable);
}