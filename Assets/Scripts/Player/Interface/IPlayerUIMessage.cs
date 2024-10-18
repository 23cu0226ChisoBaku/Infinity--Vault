/// <summary>
/// プレイヤーのデータでUIを更新するときに渡すインターフェース
/// </summary>
internal interface IPlayerUIMessage
{
  /// <summary>
  /// プレイヤーの財産(スコア)を取得
  /// </summary>
  /// <returns>財産(スコア)</returns>
  int GetWealth();
}
