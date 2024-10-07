public interface IPickable
{
    void OnPick(IItemGetable getable);
    ItemInfo Info {get;}
}