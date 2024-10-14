using System;
using UnityEngine;

/// <summary>
/// パズルインターフェース
/// </summary>
internal interface IPuzzle
{
  /// <summary>
  /// パズルを表示
  /// </summary>
  public void ShowPuzzle();
  /// <summary>
  /// パズルを隠す
  /// </summary>
  public void HidePuzzle();
  /// <summary>
  /// パズルをリセット
  /// </summary>
  public void ResetPuzzle();
  /// <summary>
  /// パズルが解かれたら呼び出されるコールバック
  /// </summary>
  public event Action OnPuzzleClear;   
}

internal interface IDialPuzzle : IPuzzle
{
  /// <summary>
  /// ダイヤル錠を更新する
  /// </summary>
  /// <param name="startPos">操作カーソルの始点座標(ワールド座標)</param>
  /// <param name="endPos">操作カーソルの終点座標(ワールド座標)</param>
  public void UpdateDial(Vector2 startPos, Vector2 endPos);
}

internal interface IButtonPuzzleUpdater : IPuzzle
{
  public void UpdateClick();
}