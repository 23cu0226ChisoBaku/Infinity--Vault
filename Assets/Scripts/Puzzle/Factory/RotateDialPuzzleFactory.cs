using System;
using MDesingPattern.MFactory;

public class RotateDialPuzzleFactory : IFactory<RotateDialPuzzle>
{
  private event Func<RotateDialPuzzle> _rotateDialPuzzleFactory;
  public RotateDialPuzzleFactory(Func<RotateDialPuzzle> factoryFunc)
  {
    _rotateDialPuzzleFactory = factoryFunc;
  }
  RotateDialPuzzle IFactory<RotateDialPuzzle>.GetProduct()
  {
    return _rotateDialPuzzleFactory?.Invoke();
  }
}