using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDrag : MonoBehaviour
{
  public event Action OnAllPuzzleClear;
  private IPuzzle monoPuzzle;
  private IPuzzleGenerator puzzleGenerator;
  private bool _isAllClear;
  private void Awake()
  {
    _isAllClear = false;
    puzzleGenerator = PuzzleGenerator.Instance;
    monoPuzzle = puzzleGenerator.GeneratePuzzle(EPuzzleDifficulty.Hard,EDialPuzzleType.Rotate);
    monoPuzzle.GameObject.transform.position = Vector2.zero;

    monoPuzzle.OnPuzzleClear += Clear;
  }

  private void Update()
  {
    if (monoPuzzle.IsAlive())
    {
      monoPuzzle.UpdatePuzzle();
    }

    if (_isAllClear)
    {
      OnAllPuzzleClear?.Invoke();
      Destroy(gameObject);
    }
  }

  private void Clear()
  {
    _isAllClear = true;
    Debug.Log("Puzzle Clear");
  }
}
