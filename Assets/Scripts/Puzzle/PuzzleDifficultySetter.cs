
using System;
using System.Runtime.InteropServices;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

/// <summary>
/// パズルの難易度設定クラス
/// TODO Strategy Patternで
/// </summary>
public static class PuzzleDifficultySetter
{
  private readonly static IPuzzleDifficultySetter EASY_PUZZLE_SETTER = new EasyPuzzleSetter(); 
  private readonly static IPuzzleDifficultySetter MEDIUM_PUZZLE_SETTER = new MediumPuzzleSetter(); 
  private readonly static IPuzzleDifficultySetter HARD_PUZZLE_SETTER = new HardPuzzleSetter(); 
  public static RotateDialPuzzle SetDifficulty(this RotateDialPuzzle puzzle, EPuzzleDifficulty puzzleDifficulty)
  {
    return puzzleDifficulty switch
    {
      EPuzzleDifficulty.Easy    => EASY_PUZZLE_SETTER.SetDifficulty(puzzle),
      EPuzzleDifficulty.Medium  => MEDIUM_PUZZLE_SETTER.SetDifficulty(puzzle),
      EPuzzleDifficulty.Hard    => HARD_PUZZLE_SETTER.SetDifficulty(puzzle),
      _                         => throw new ArgumentException($"{puzzle.GetType()} invalid Difficulty"),
    };
  }

  public static DigitDialPuzzle SetDifficulty(this DigitDialPuzzle puzzle, EPuzzleDifficulty puzzleDifficulty)
  {
    return puzzleDifficulty switch
    {
      EPuzzleDifficulty.Easy    => EASY_PUZZLE_SETTER.SetDifficulty(puzzle),
      EPuzzleDifficulty.Medium  => MEDIUM_PUZZLE_SETTER.SetDifficulty(puzzle),
      EPuzzleDifficulty.Hard    => HARD_PUZZLE_SETTER.SetDifficulty(puzzle),
      _                         => throw new ArgumentException($"{puzzle.GetType()} invalid Difficulty"),
    };
  }
}

public interface IPuzzleDifficultySetter
{
  public RotateDialPuzzle SetDifficulty(RotateDialPuzzle puzzle);
  public DigitDialPuzzle SetDifficulty(DigitDialPuzzle puzzle);
}

public class EasyPuzzleSetter : IPuzzleDifficultySetter
{
  RotateDialPuzzle IPuzzleDifficultySetter.SetDifficulty(RotateDialPuzzle puzzle)
  {
    RotateDialPuzzleModel info = new RotateDialPuzzleModel();

    info.RotateDial = RotateDialPuzzleModel.ERotateDial.Clockwise;
    info.Round = 2;
    puzzle.InitInfo(info);

    return puzzle;
  }
  DigitDialPuzzle IPuzzleDifficultySetter.SetDifficulty(DigitDialPuzzle puzzle)
  {
      throw new System.NotImplementedException();
  }
}

public class MediumPuzzleSetter : IPuzzleDifficultySetter
{
  RotateDialPuzzle IPuzzleDifficultySetter.SetDifficulty(RotateDialPuzzle puzzle)
  {
    RotateDialPuzzleModel info = new RotateDialPuzzleModel();

    info.RotateDial = RotateDialPuzzleModel.ERotateDial.Clockwise;
    info.Round = 3;
    puzzle.InitInfo(info);

    return puzzle;
  }
  DigitDialPuzzle IPuzzleDifficultySetter.SetDifficulty(DigitDialPuzzle puzzle)
  {
      throw new System.NotImplementedException();
  }
}

public class HardPuzzleSetter : IPuzzleDifficultySetter
{
  RotateDialPuzzle IPuzzleDifficultySetter.SetDifficulty(RotateDialPuzzle puzzle)
  {
    RotateDialPuzzleModel info = new RotateDialPuzzleModel();

    info.RotateDial = RotateDialPuzzleModel.ERotateDial.Clockwise;
    info.Round = 4;
    puzzle.InitInfo(info);

    return puzzle;
  }
  DigitDialPuzzle IPuzzleDifficultySetter.SetDifficulty(DigitDialPuzzle puzzle)
  {
      throw new System.NotImplementedException();
  }
}