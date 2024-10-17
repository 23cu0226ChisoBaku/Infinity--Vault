using UnityEngine;

using System.Collections.Generic;
using IV.Puzzle;
using MLibrary;

public struct SequenceButtonPuzzleInfo
{
  public int ButtonCnt;
}

internal class SequenceButtonPuzzle : Puzzle
{

  private List<IPuzzleButton> _puzzleButtons;
  private SequenceButtonPuzzleInfo _info;

  public override IPuzzle AcceptDifficulty(EPuzzleDifficulty difficulty)
  {
    return PuzzleDifficultySetter.GetDifficultySetter(difficulty).SetDifficulty(this);
  }

  public override void HidePuzzle()
  {
    foreach(var button in _puzzleButtons)
    {
      button.HidePuzzle();
    }
    gameObject.SetActive(false);
  }

  public override void ResetPuzzle()
  {
    Debug.Log("ResetButton");
    ResetPuzzleImpl();
  }

  public override void ShowPuzzle()
  {
    if (_puzzleButtons.Count > 0)
    {
      _puzzleButtons[0].ShowPuzzle();
    }
    gameObject.SetActive(true);
  }

  public override void UpdatePuzzle()
  {
    
  }

  public void InitInfo(SequenceButtonPuzzleInfo info)
  {
    _info = info;

    foreach(var button in _puzzleButtons)
    {
      if (button.IsAlive())
      {
        button.DisposeButton();
      }
    }

    _puzzleButtons.Clear();

    // パズル専用ボタンを生成し、シャッフルしてパズルを作る
    {
      for(int i = 0; i < _info.ButtonCnt; ++i)
      {
        IPuzzleGenerator puzzleGenerator = PuzzleGenerator.Instance;

        var button = puzzleGenerator.GeneratePuzzleButton();
        button.HidePuzzle();
        _puzzleButtons.Add(button);
      }

      ResetPuzzleImpl();
    }
  }

  private void Awake()
  {
    transform.localPosition = Vector3.zero;
    _puzzleButtons = new List<IPuzzleButton>();
  }

  /// <summary>
  /// パズルをリセット
  /// </summary>
  private void ResetPuzzleImpl()
  {
    // Clearコールバックを削除
    _onPuzzleClear.Unsubscribe(DisposeButtons);

    // ボタンをシャッフルする
    _puzzleButtons.Shuffle();

    // ボタンのイベントをバインドする
    for (int i = 0; i < _info.ButtonCnt-1; ++i)
    {
      var currentButton = _puzzleButtons[i];
      var nextButton = _puzzleButtons[i+1];
      currentButton.ResetPuzzle();
      currentButton.OnClick += () => {
                                        currentButton.HidePuzzle();
                                        nextButton.ShowPuzzle();
                                     };
    }

    // 最後のボタンのイベントをバインド
    _puzzleButtons[_info.ButtonCnt-1].ResetPuzzle();
    _puzzleButtons[_info.ButtonCnt-1].OnClick += () => {
                                                          _puzzleButtons[_info.ButtonCnt-1].HidePuzzle();
                                                          _onPuzzleClear?.Invoke();
                                                        };
    // ボタンの座標を設定する
    TestSetPos();

    // パズルが解かれたら実行するイベントをバインド
    _onPuzzleClear += DisposeButtons;
  }

  private void TestSetPos()
  {
    int test = 0;
    foreach(var button in _puzzleButtons)
    {
      if (button is PuzzleButton puzzleButton)
      {
        puzzleButton.transform.position = Vector2.right * test++;
      }
    }
  }

  private void DisposeButtons()
  {
    foreach(var button in _puzzleButtons)
    {
      if (button.IsAlive())
      {
        button.DisposeButton();
      }
    }
  }
}