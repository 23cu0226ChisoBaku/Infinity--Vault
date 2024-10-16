/// <summary>
/// 拾えるオブジェクト
/// </summary>
public interface IPickable
{
  /// <summary>
  /// 拾われたときに呼び出されるコールバック
  /// </summary>
  /// <param name="getable">アイテムを拾えるオブジェクト</param>
  void OnPick(IItemGetable getable);
}