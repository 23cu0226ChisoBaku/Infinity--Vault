
using System;
using System.Runtime.InteropServices;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

/// <summary>
/// パズルの難易度設定関数を取得するクラス
/// TODO Strategy Pattern
/// </summary>
public static class PuzzleDifficultySetter
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
public interface IPuzzleDifficultySetter
{
  public RotateDialPuzzleController SetDifficulty(RotateDialPuzzleController puzzle);
  public DigitDialPuzzleController SetDifficulty(DigitDialPuzzleController puzzle);
}

public class EasyPuzzleSetter : IPuzzleDifficultySetter
{
  RotateDialPuzzleController IPuzzleDifficultySetter.SetDifficulty(RotateDialPuzzleController puzzle)
  {
    RotateDialPuzzleModel info = new RotateDialPuzzleModel();

    info.RotateDial = RotateDialPuzzleModel.ERotateDial.Clockwise;
    info.Round = 2;
    puzzle.InitInfo(info);

    return puzzle;
  }
  DigitDialPuzzleController IPuzzleDifficultySetter.SetDifficulty(DigitDialPuzzleController puzzle)
  {
      throw new System.NotImplementedException();
  }
}

public class MediumPuzzleSetter : IPuzzleDifficultySetter
{
  RotateDialPuzzleController IPuzzleDifficultySetter.SetDifficulty(RotateDialPuzzleController puzzle)
  {
    RotateDialPuzzleModel info = new RotateDialPuzzleModel();

    info.RotateDial = RotateDialPuzzleModel.ERotateDial.Clockwise;
    info.Round = 3;
    puzzle.InitInfo(info);

    return puzzle;
  }
  DigitDialPuzzleController IPuzzleDifficultySetter.SetDifficulty(DigitDialPuzzleController puzzle)
  {
      throw new System.NotImplementedException();
  }
}

public class HardPuzzleSetter : IPuzzleDifficultySetter
{
  RotateDialPuzzleController IPuzzleDifficultySetter.SetDifficulty(RotateDialPuzzleController puzzle)
  {
    RotateDialPuzzleModel info = new RotateDialPuzzleModel();

    info.RotateDial = RotateDialPuzzleModel.ERotateDial.Clockwise;
    info.Round = 4;
    puzzle.InitInfo(info);

    return puzzle;
  }
  DigitDialPuzzleController IPuzzleDifficultySetter.SetDifficulty(DigitDialPuzzleController puzzle)
  {
      throw new System.NotImplementedException();
  }
}