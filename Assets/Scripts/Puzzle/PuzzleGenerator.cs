using System;
using MDesingPattern.MFactory;
using MSingleton;

/// <summary>
/// パズルの難易度
/// </summary>
public enum EPuzzleDifficulty
{
  Easy = 1,
  Medium = 2,
  Hard = 3,
}

// ボタンパズルの種類
public enum EButtonPuzzleType
{
  Sequence,     // 順番に押す形式
}

// ダイヤル式パズルの種類
public enum EDialPuzzleType
{
  Rotate,       // 回転式
  DigitCombi,   // 桁式
}
internal interface IPuzzleGenerator
{
  IButtonPuzzleUpdater GenerateButtonPuzzle(EPuzzleDifficulty difficulty, EButtonPuzzleType buttonPuzzleType);
  IDialPuzzle GenerateDialPuzzle(EPuzzleDifficulty difficulty, EDialPuzzleType dialPuzzleType);
}
/// <summary>
/// パズルを生成するクラス
/// </summary>
public class PuzzleGenerator : Singleton<PuzzleGenerator>,IPuzzleGenerator
{
  private IFactory<RotateDialPuzzle> _rotateDialPuzzle;
  private IFactory<DigitDialPuzzle> _digitDialPuzzle;

  public PuzzleGenerator()
  {
    _rotateDialPuzzle = new RotateDialPuzzleFactory(() => { return new RotateDialPuzzle();});
  }

  IButtonPuzzleUpdater IPuzzleGenerator.GenerateButtonPuzzle(EPuzzleDifficulty difficulty, EButtonPuzzleType buttonPuzzleType)
  {
    throw new NotImplementedException();
  }

  IDialPuzzle IPuzzleGenerator.GenerateDialPuzzle(EPuzzleDifficulty difficulty, EDialPuzzleType dialPuzzleType)
  {
    return dialPuzzleType switch
    {
      EDialPuzzleType.Rotate      => _rotateDialPuzzle.GetProduct().SetDifficulty(difficulty),
      EDialPuzzleType.DigitCombi  => _digitDialPuzzle.GetProduct().SetDifficulty(difficulty),
      _                           => throw new ArgumentException(message : $"invalid Dial Puzzle Type"),
    };
  }
}

