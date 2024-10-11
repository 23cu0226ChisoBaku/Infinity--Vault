
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
internal interface IItem
{

}

/// <summary>
/// Visitor Pattern
/// </summary>
public interface IItemGetable
{
    void GetItem(GemContainer gem);
    void GetItem(KeyItemContainer keyItem);
    void GetItem(ConsumeItemContainer consumeItem);
}

internal interface IItemSettable
{
    void SetItem(ItemInfo item);
}

internal interface IItemContainer : IItemGetable, IItemSettable
{
 
}
