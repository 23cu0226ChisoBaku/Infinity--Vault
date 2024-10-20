using System;
using IV.Puzzle;
using MDesingPattern.MFactory;
using MSingleton;
using UnityEngine;

/// <summary>
/// パズルの難易度
/// </summary>
public enum EPuzzleDifficulty
{
  Easy = 0,
  Medium,
  Hard,
  DifficultyCount,
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
  IPuzzle GeneratePuzzle(EPuzzleDifficulty difficulty, EButtonPuzzleType buttonPuzzleType);
  IPuzzle GeneratePuzzle(EPuzzleDifficulty difficulty, EDialPuzzleType dialPuzzleType);
  IPuzzleButton GeneratePuzzleButton();
  IPuzzlePanel GetPanel();
}
/// <summary>
/// パズルを生成するクラス
/// </summary>
public class PuzzleGenerator : Singleton<PuzzleGenerator>,IPuzzleGenerator
{
  private IFactory<GameObject> _rotateDialPuzzleFactory;
  private IFactory<GameObject> _digitDialPuzzleFactory;
  private IFactory<GameObject> _puzzleButtonFactory;
  private IFactory<GameObject> _sequenceButtonPuzzleFactory;
  private IFactory<GameObject> _puzzlePanelFactory;

  public PuzzleGenerator()
  {
    var rotateDialPuzzlePrefab = Resources.Load<GameObject>("Prefabs/Puzzle/RotateDialPuzzle");
    var digitDialPuzzlePrefab = Resources.Load<GameObject>("Prefabs/Puzzle/DigitDialPuzzle");
    var puzzleButtonPrefab = Resources.Load<GameObject>("Prefabs/Puzzle/PuzzleButton");
    var buttonPuzzlePrefab = Resources.Load<GameObject>("Prefabs/Puzzle/SequenceButtonPuzzle");
    var puzzlePanelPrefab = Resources.Load<GameObject>("Prefabs/Puzzle/PuzzlePanel");
    // TODO ファクトリーを見直す
    _rotateDialPuzzleFactory = new RotateDialPuzzleFactory(() => { return UnityEngine.Object.Instantiate(rotateDialPuzzlePrefab);});
    _digitDialPuzzleFactory = new DigitDialPuzzleFactory(() => { return UnityEngine.Object.Instantiate(digitDialPuzzlePrefab);});
    _puzzleButtonFactory = new TemplateFactory(() => { return UnityEngine.Object.Instantiate(puzzleButtonPrefab);});
    _sequenceButtonPuzzleFactory = new TemplateFactory(() => { return UnityEngine.Object.Instantiate(buttonPuzzlePrefab);});
    _puzzlePanelFactory = new TemplateFactory(() => { return UnityEngine.Object.Instantiate(puzzlePanelPrefab);});
  }

    IPuzzlePanel IPuzzleGenerator.GetPanel()
    {
      return _puzzlePanelFactory.GetProduct().GetComponent<IPuzzlePanel>();
    }

    IPuzzle IPuzzleGenerator.GeneratePuzzle(EPuzzleDifficulty difficulty, EButtonPuzzleType buttonPuzzleType)
  {
    return buttonPuzzleType switch
    {
      EButtonPuzzleType.Sequence  => _sequenceButtonPuzzleFactory.GetProduct().GetComponent<ICanSetPuzzleDifficulty>().AcceptDifficulty(difficulty),
      _                           => throw new ArgumentException(message : $"invalid Button Puzzle Type"),
    };
  }

  IPuzzle IPuzzleGenerator.GeneratePuzzle(EPuzzleDifficulty difficulty, EDialPuzzleType dialPuzzleType)
  {
    return dialPuzzleType switch
    {
      EDialPuzzleType.Rotate      => _rotateDialPuzzleFactory.GetProduct().GetComponent<ICanSetPuzzleDifficulty>().AcceptDifficulty(difficulty),
      EDialPuzzleType.DigitCombi  => _digitDialPuzzleFactory.GetProduct().GetComponent<ICanSetPuzzleDifficulty>().AcceptDifficulty(difficulty),
      _                           => throw new ArgumentException(message : $"invalid Dial Puzzle Type"),
    };
  }

    IPuzzleButton IPuzzleGenerator.GeneratePuzzleButton()
    {
      return _puzzleButtonFactory.GetProduct().GetComponent<IPuzzleButton>();
    }
}

public class TemplateFactory : IFactory<GameObject>
{
  private event Func<GameObject> _factory;
  public TemplateFactory(Func<GameObject> factoryFunc)
  {
    _factory = factoryFunc;
  }
  GameObject IFactory<GameObject>.GetProduct()
  {
    return _factory?.Invoke();
  }
}

