using MDesingPattern.MFactory;
using UnityEngine;

public abstract class ItemContainer : MonoBehaviour, IPickable,IProduct
{
    protected Item _itemInstance;
    public ItemInfo Info => _itemInstance.Info;
    public abstract void InitProduct();
    public abstract void OnPick(IItemGetable getable);
}