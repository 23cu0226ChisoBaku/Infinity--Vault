
public enum EItemType
{
    None,
    Worth,
    Item,
    KeyItem,
}
public struct ItemInfo
{
    //public Guid UniqueID;
    public string Name;
    public EItemType Type;

}

public struct GemInfo
{

}

internal interface IItem
{

}

public interface IItemGetable
{
    void GetItem(ItemInfo item);
}

internal interface IItemSettable
{
    void SetItem(ItemInfo item);
}

internal interface IItemContainer : IItemGetable, IItemSettable
{
 
}
