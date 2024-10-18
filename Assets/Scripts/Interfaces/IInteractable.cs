using UnityEngine;

/// <summary>
/// 操作するオブジェクトの情報
/// </summary>
public struct InteractTargetInfo
{
  /// <summary>
  /// オブジェクトの名前
  /// </summary>
  public string Name;
  /// <summary>
  /// 操作するとき使うキー
  /// </summary>
  public KeyCode InteractKey;
  /// <summary>
  /// 操作するオブジェクトのレイヤー(Unity Object Base)
  /// </summary>
  public LayerMask Layer;
}
internal interface IInteractable
{
  /// <summary>
  /// 操作するオブジェクトの情報を取得
  /// </summary>
  /// <returns></returns>
  InteractTargetInfo GetTargetInfo();
  /// <summary>
  /// 操作できる状態にする
  /// </summary>
  void ActiveInteract();
  /// <summary>
  /// 操作する
  /// </summary>
  void DoInteract();
  /// <summary>
  /// 操作終了
  /// </summary>
  void EndInteract();
}
