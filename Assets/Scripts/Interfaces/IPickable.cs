/// <summary>
/// 拾えるオブジェクト
/// </summary>
public interface IPickable
{
    /// <summary>
    /// 拾られるとき呼び出されるコールバック
    /// </summary>
    /// <param name="getable"></param>
    void OnPick(IItemGetable getable);
}