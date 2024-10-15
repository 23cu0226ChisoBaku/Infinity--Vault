using UnityEngine;
using System;

public sealed class DigitDialPuzzle : Puzzle, IDialPuzzle
{
  private GameObject _dialObject;
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
  public void HidePuzzle()
  {
    if (_dialObject != null)
    {
      _dialObject.SetActive(false);
    }
  }

  public void InitTargetGameObject(GameObject targetGameObject)
  {
      _dialObject = targetGameObject;
  }

  public void ResetPuzzle()
  {
      throw new NotImplementedException();
  }

  public void ShowPuzzle()
  {
    if (_dialObject != null)
    {
      _dialObject.SetActive(true);
    }
  }

  public void UpdateDial(Vector2 startPos, Vector2 endPos)
  {
      throw new NotImplementedException();
  }
}