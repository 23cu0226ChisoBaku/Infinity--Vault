using UnityEngine;
using MEvent;
using System;
/// <summary>
/// パズルの親クラス
/// </summary>
public abstract class Puzzle : MonoBehaviour, IPuzzle,ICanSetPuzzleDifficulty
{
  protected DisposableEvent _onPuzzleClear = new DisposableEvent();
  public bool IsPuzzleCleared {get;protected set;}
  public event Action OnPuzzleClear
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
  public abstract void AcceptDifficulty(EPuzzleDifficulty difficulty);

  public virtual void HidePuzzle()
  {
    gameObject.SetActive(false);
  }
  public abstract void ResetPuzzle();
  public abstract void ShowPuzzle();
  public abstract void UpdatePuzzle();
  protected virtual void OnDestroy() 
  {
    Debug.LogWarning("HogeHoge");
    _onPuzzleClear.Dispose();
  }
}
