using System;
using UnityEngine;

/// <summary>
/// パズルインターフェース
/// </summary>
internal interface IPuzzle
{
  /// <summary>
  /// パズルを表示する
  /// </summary>
  public void ShowPuzzle();
  /// <summary>
  /// パズルを隠す
  /// </summary>
  public void HidePuzzle();
  /// <summary>
  /// パズルをリセットする
  /// </summary>
  public void ResetPuzzle();
  /// <summary>
  /// パズル更新処理をする
  /// </summary>
  public void UpdatePuzzle();
  /// <summary>
  /// パズルがクリアしたときのコールバック登録インターフェース
  /// </summary>
  public event Action OnPuzzleClear;
}
