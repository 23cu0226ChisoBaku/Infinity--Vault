/// <summary>
/// アイテムの情報
/// </summary>
public struct ItemInfo
{
  //public Guid UniqueID;
  public string Name;

}
/// <summary>
/// アイテムを取得する
/// 取得するアイテムの種類によって処理を分離させる
/// Visitor Pattern
/// </summary>
public interface IItemGetable
{
  void GetItem(GemContainer gem);
  void GetItem(KeyItemContainer keyItem);
  void GetItem(ConsumeItemContainer consumeItem);
}

/// <summary>
/// TODO
/// </summary>
internal interface IItemSettable
{
  void SetItem(ItemInfo item);
}
