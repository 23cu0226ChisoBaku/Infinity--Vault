using System;
using MDesingPattern.MFactory;
using MSingleton;
using UnityEngine;

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
  IPuzzle GenerateButtonPuzzle(EPuzzleDifficulty difficulty, EButtonPuzzleType buttonPuzzleType);
  IPuzzle GenerateDialPuzzle(EPuzzleDifficulty difficulty, EDialPuzzleType dialPuzzleType);
}
/// <summary>
/// パズルを生成するクラス
/// </summary>
public class PuzzleGenerator : Singleton<PuzzleGenerator>,IPuzzleGenerator
{
  private IFactory<GameObject> _rotateDialPuzzle;
  private IFactory<GameObject> _digitDialPuzzle;

  public PuzzleGenerator()
  {
    var rotateDialPuzzlePrefab = Resources.Load<GameObject>("Prefabs/Puzzle/RotateDialPuzzle");
    var digitDialPuzzlePrefab = Resources.Load<GameObject>("Prefabs/Puzzle/DigitDialPuzzle");
    _rotateDialPuzzle = new RotateDialPuzzleFactory(() => { return UnityEngine.Object.Instantiate(rotateDialPuzzlePrefab);});
    _digitDialPuzzle = new DigitDialPuzzleFactory(() => { return UnityEngine.Object.Instantiate(digitDialPuzzlePrefab);});
  }

  IPuzzle IPuzzleGenerator.GenerateButtonPuzzle(EPuzzleDifficulty difficulty, EButtonPuzzleType buttonPuzzleType)
  {
    throw new NotImplementedException();
  }

  IPuzzle IPuzzleGenerator.GenerateDialPuzzle(EPuzzleDifficulty difficulty, EDialPuzzleType dialPuzzleType)
  {
    // return dialPuzzleType switch
    // {
    //   EDialPuzzleType.Rotate      => _rotateDialPuzzle.GetProduct().GetComponent<RotateDialPuzzleController>().AcceptDifficulty(difficulty),
    //   EDialPuzzleType.DigitCombi  => _digitDialPuzzle.GetProduct().GetComponent<DigitDialPuzzleController>().AcceptDifficulty(difficulty),
    //   _                           => throw new ArgumentException(message : $"invalid Dial Puzzle Type"),
    // };
    return null;
  }
}

