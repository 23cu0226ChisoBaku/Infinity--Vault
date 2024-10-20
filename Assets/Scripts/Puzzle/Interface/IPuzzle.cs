using System;
using UnityEngine;

/// <summary>
/// パズルインターフェース
/// </summary>
internal interface IPuzzle : IPuzzleBase
{
  /// <summary>
  /// パズル更新処理をする
  /// </summary>
  public void UpdatePuzzle();
  /// <summary>
  /// パズルがクリアしたときのコールバック登録インターフェース
  /// </summary>
  public event Action OnPuzzleClear;
}

