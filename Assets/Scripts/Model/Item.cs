using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item
{
    protected Item(EItemType type,string itemName)
    {
        _itemInfo = new ItemInfo
        {
            Type = type,
            Name = itemName,
        };
    }
    private ItemInfo _itemInfo;

    public ItemInfo Info
    {
        get
        {
            return _itemInfo;
        }
    }

}
