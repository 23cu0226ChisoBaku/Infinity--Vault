
public struct Item
{
    //public Guid UniqueID;
    public string name;
}

internal interface IItem
{

}

internal interface IItemGetable
{
    Item GetItem();
}

internal interface IItemSettable
{
    void SetItem(Item item);
}

internal interface IItemContainer : IItemGetable, IItemSettable
{
 
}
