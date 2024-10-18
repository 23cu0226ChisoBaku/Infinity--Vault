using UnityEngine;

/// <summary>
/// 消耗品アイテム
/// </summary>
public abstract class ConsumeItemContainer : ItemContainer
{
  // 消耗品画像
  protected Sprite _itemSprite;
  public Sprite ItemSprite => _itemSprite;
  public override void OnPick(IItemGetable getable)
  {
      if (getable.IsAlive())
      {
          getable.GetItem(this);
      }
  }
}