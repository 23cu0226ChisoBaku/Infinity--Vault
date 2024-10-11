using MDesingPattern.MFactory;
using UnityEngine;

public abstract class ItemContainer : MonoBehaviour, IPickable,IProduct
{
    protected string _itemName = "ItemName_ItemType";
    public abstract void InitProduct();
    public abstract void OnPick(IItemGetable getable);
}