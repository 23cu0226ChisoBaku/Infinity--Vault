using UnityEngine;
using MEvent;
using System;
using TMPro;
/// <summary>
/// パズルの親クラス
/// </summary>
internal abstract class Puzzle : MonoBehaviour, IPuzzle,ICanSetPuzzleDifficulty
{
  // TODO
  protected TMP_Text _puzzleText;
  protected TMP_Text _puzzleExitHint;
  // end TODO
  protected DisposableEvent _onPuzzleClear = new DisposableEvent();
  public bool IsPuzzleCleared {get;protected set;}
  public bool IsPuzzleActive {get;protected set;}
  event Action IPuzzle.OnPuzzleClear
  {
    add
    {
      _onPuzzleClear.Subscribe(value);
    }
    remove
    {
      _onPuzzleClear.Unsubscribe(value);
    }
  }
  public abstract IPuzzle AcceptDifficulty(EPuzzleDifficulty difficulty);

  /// <summary>
  /// パズルを表示する
  /// </summary>
  public abstract void ShowPuzzle();
  /// <summary>
  /// パズルを隠す
  /// </summary>
  public abstract void HidePuzzle();
  /// <summary>
  /// パズルをリセットする
  /// </summary>
  public abstract void ResetPuzzle();
  public abstract void UpdatePuzzle();
  protected virtual void OnDestroy() 
  {
    _onPuzzleClear.Dispose();
  }
}
