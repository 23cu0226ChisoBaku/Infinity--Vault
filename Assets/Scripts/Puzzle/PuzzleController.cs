
using System;
using MEvent;
using MSingleton;
using System.Collections.Generic;

public interface IPuzzleController
{
  public event Action OnClear;
  public event Action OnActive;
  public event Action OnDeactive;
  public void InitPuzzle(EPuzzleDifficulty difficulty);
  public void UpdatePuzzleState();
  public void TermPuzzle();
  public bool IsActive {get;}
  public void Active();
  public void Deactive();

}
public class PuzzleController : IPuzzleController
{
  // TODO
  private enum EPuzzleType
  {
    SequenceButton = 0,
    RotateDial = 1,
    PuzzleTypeCount,
  }
  private DisposableEvent _onClearEvent;
  private IPuzzle _puzzle;
  private bool _isAllPuzzleClear;
  private bool _isActive;

  bool IPuzzleController.IsActive => _puzzle.IsAlive() && _isActive;

    event Action IPuzzleController.OnClear
  {
    add
    {
      _onClearEvent.Subscribe(value);
    }

    remove
    {
      _onClearEvent.Unsubscribe(value);
    }
  }

  event Action IPuzzleController.OnActive
  {
    add
    {
        throw new NotImplementedException();
    }

    remove
    {
        throw new NotImplementedException();
    }
  }

  event Action IPuzzleController.OnDeactive
  {
    add
    {
        throw new NotImplementedException();
    }

    remove
    {
        throw new NotImplementedException();
    }
  }

  public PuzzleController()
  {
    _onClearEvent = new DisposableEvent();
    _puzzle = null;
    _isAllPuzzleClear = false;
    _isActive = false;

    PuzzleManager.Instance.RegisterPuzzle(this);
    PuzzleManager.Instance.OnExitPuzzle += ((IPuzzleController)this).Deactive;

  }
  void IPuzzleController.InitPuzzle(EPuzzleDifficulty difficulty)
  {
    IPuzzleGenerator puzzleGenerator = PuzzleGenerator.Instance;
    var puzzleType = (EPuzzleType)UnityEngine.Random.Range(0,(int)EPuzzleType.PuzzleTypeCount);

    switch(puzzleType)
    {
      // TODO switch case以外の方法を考えろ！
      case EPuzzleType.SequenceButton:
      {
        _puzzle = puzzleGenerator.GeneratePuzzle(difficulty,EButtonPuzzleType.Sequence);
      }
      break;
      case EPuzzleType.RotateDial:
      {
        _puzzle = puzzleGenerator.GeneratePuzzle(difficulty,EDialPuzzleType.Rotate);
      }
      break;
      default:
      {
        throw new ArgumentException($"Invalid Puzzle Type {puzzleType}");
      }
    }

    if (_puzzle.IsAlive())
    {
      _puzzle.OnPuzzleClear += () =>  {
                                        _isAllPuzzleClear = true;
                                      };
      _puzzle.HidePuzzle();
    }

  }

  void IPuzzleController.UpdatePuzzleState()
  {
    if (!_puzzle.IsAlive())
    {
      return;
    }

    _puzzle.UpdatePuzzle();

    if (_isAllPuzzleClear)
    {
      _onClearEvent?.Invoke();
      PuzzleManager.Instance.UnregisterPuzzle(this);
    }
  }

  void IPuzzleController.TermPuzzle()
  {
    PuzzleManager.Instance.OnExitPuzzle += ((IPuzzleController)this).Deactive;
  }

  void IPuzzleController.Active()
  {
    if (_isActive)
    {
      return;
    }

    if (_puzzle.IsAlive())
    {
      _isActive = true;
      _puzzle.ResetPuzzle();
      _puzzle.ShowPuzzle();
    }
  }

  void IPuzzleController.Deactive()
  {
    if(!_isActive)
    {
      return;
    }
    
    _isActive = false;
    {
      if(_puzzle.IsAlive())
      {
        _puzzle.HidePuzzle();
      }
    }
  }
}

public class PuzzleManager : SingletonMono<PuzzleManager>
{
  private List<IPuzzleController> _puzzleCtrls;

  public event Action OnExitPuzzle;

  protected override void Awake()
  {
    base.Awake();
    _puzzleCtrls = new List<IPuzzleController>();
  }

  private void Update() 
  {
    for(int i = 0; i < _puzzleCtrls.Count; ++i)
    {
      if (_puzzleCtrls[i].IsActive)
      {
        _puzzleCtrls[i].UpdatePuzzleState();
      }
    }
  }
  public void RegisterPuzzle(IPuzzleController puzzleController)
  {
    _puzzleCtrls.Add(puzzleController);
  }
  public void UnregisterPuzzle(IPuzzleController puzzleController)
  {
    var unregisterPuzzle = _puzzleCtrls.Find( entity => { return entity == puzzleController;});

    if (unregisterPuzzle != null)
    {
      _puzzleCtrls.Remove(unregisterPuzzle);
    }
  }

  public void ExitPuzzle()
  {
    // TODO Temp Code
    OnExitPuzzle?.Invoke();
  }
}