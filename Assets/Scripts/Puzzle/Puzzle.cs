using UnityEngine;
using MEvent;
using System;
/// <summary>
/// パズルの親クラス
/// </summary>
internal abstract class Puzzle : MonoBehaviour, IPuzzle,ICanSetPuzzleDifficulty
{
  private GameObject _puzzlePanel;
  protected DisposableEvent _onPuzzleClear = new DisposableEvent();
  public bool IsPuzzleCleared {get;protected set;}
  public bool IsPuzzleActive {get;protected set;}
  public GameObject GameObject => gameObject;
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
  public abstract IPuzzle AcceptDifficulty(EPuzzleDifficulty difficulty);

  public abstract void HidePuzzle();
  public abstract void ResetPuzzle();
  public abstract void ShowPuzzle();
  public abstract void UpdatePuzzle();
  protected virtual void OnDestroy() 
  {
    _onPuzzleClear.Dispose();
  }
}
