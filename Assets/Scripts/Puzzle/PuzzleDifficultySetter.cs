
using System;
using System.Runtime.InteropServices;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

/// <summary>
/// パズルの難易度設定関数を取得するクラス
/// TODO Strategy Pattern
/// </summary>
internal static class PuzzleDifficultySetter
{
  private readonly static IPuzzleDifficultySetter EASY_PUZZLE_SETTER = new EasyPuzzleSetter(); 
  private readonly static IPuzzleDifficultySetter MEDIUM_PUZZLE_SETTER = new MediumPuzzleSetter(); 
  private readonly static IPuzzleDifficultySetter HARD_PUZZLE_SETTER = new HardPuzzleSetter(); 

  public static IPuzzleDifficultySetter GetDifficultySetter(EPuzzleDifficulty difficulty)
  {
    return difficulty switch
    {
      EPuzzleDifficulty.Easy    => EASY_PUZZLE_SETTER,
      EPuzzleDifficulty.Medium  => MEDIUM_PUZZLE_SETTER,
      EPuzzleDifficulty.Hard    => HARD_PUZZLE_SETTER,
      _                         => throw new ArgumentException("Invalid Puzzle Difficulty"),
    };
  }
}

/// <summary>
/// 難易度設定を行うクラス
/// Visitor Pattern
/// </summary>
internal interface IPuzzleDifficultySetter
{
  public RotateDialPuzzle SetDifficulty(RotateDialPuzzle puzzle);
  public DigitDialPuzzle SetDifficulty(DigitDialPuzzle puzzle);
  public SequenceButtonPuzzle SetDifficulty(SequenceButtonPuzzle puzzle);
}

/// <summary>
/// 難易度設定簡単
/// </summary>
public class EasyPuzzleSetter : IPuzzleDifficultySetter
{
  RotateDialPuzzle IPuzzleDifficultySetter.SetDifficulty(RotateDialPuzzle puzzle)
  {
    RotateDialPuzzleInfo info = new RotateDialPuzzleInfo();

    info.RotateDial = ERotateDial.Clockwise;
    info.Round = 2;
    puzzle.InitInfo(info);

    return puzzle;
  }
  DigitDialPuzzle IPuzzleDifficultySetter.SetDifficulty(DigitDialPuzzle puzzle)
  {
      throw new System.NotImplementedException();
  }

  SequenceButtonPuzzle IPuzzleDifficultySetter.SetDifficulty(SequenceButtonPuzzle puzzle)
  {
    SequenceButtonPuzzleInfo puzzleInfo = new SequenceButtonPuzzleInfo();
    puzzleInfo.ButtonCnt = 3;

    puzzle.InitInfo(puzzleInfo);

    return puzzle;
  }
}

public class MediumPuzzleSetter : IPuzzleDifficultySetter
{
  RotateDialPuzzle IPuzzleDifficultySetter.SetDifficulty(RotateDialPuzzle puzzle)
  {
    RotateDialPuzzleInfo info = new RotateDialPuzzleInfo();

    info.RotateDial = ERotateDial.Clockwise;
    info.Round = 3;
    puzzle.InitInfo(info);

    return puzzle;
  }
  DigitDialPuzzle IPuzzleDifficultySetter.SetDifficulty(DigitDialPuzzle puzzle)
  {
      throw new System.NotImplementedException();
  }

  SequenceButtonPuzzle IPuzzleDifficultySetter.SetDifficulty(SequenceButtonPuzzle puzzle)
  {
    SequenceButtonPuzzleInfo puzzleInfo = new SequenceButtonPuzzleInfo();
    puzzleInfo.ButtonCnt = 5;

    puzzle.InitInfo(puzzleInfo);

    return puzzle;
  }
}

public class HardPuzzleSetter : IPuzzleDifficultySetter
{
  RotateDialPuzzle IPuzzleDifficultySetter.SetDifficulty(RotateDialPuzzle puzzle)
  {
    RotateDialPuzzleInfo info = new RotateDialPuzzleInfo();

    info.RotateDial = ERotateDial.Clockwise;
    info.Round = 5;
    puzzle.InitInfo(info);

    return puzzle;
  }
  DigitDialPuzzle IPuzzleDifficultySetter.SetDifficulty(DigitDialPuzzle puzzle)
  {
      throw new System.NotImplementedException();
  }

  SequenceButtonPuzzle IPuzzleDifficultySetter.SetDifficulty(SequenceButtonPuzzle puzzle)
  {
    SequenceButtonPuzzleInfo puzzleInfo = new SequenceButtonPuzzleInfo();
    puzzleInfo.ButtonCnt = 7;

    puzzle.InitInfo(puzzleInfo);

    return puzzle;
  }
}