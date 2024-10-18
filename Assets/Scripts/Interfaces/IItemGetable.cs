
/// <summary>
/// アイテムを取得できるオブジェクト
/// Visitor Pattern
/// </summary>
public interface IItemGetable
{
  void GetItem(GemContainer gem);
  void GetItem(KeyItemContainer keyItem);
  void GetItem(ConsumeItemContainer consumeItem);
}
