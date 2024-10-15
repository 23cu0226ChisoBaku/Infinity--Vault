using System;
using MDesingPattern.MFactory;

public class DigitDialPuzzleFactory : IFactory<DigitDialPuzzle>
{
  private event Func<DigitDialPuzzle> _rotateDialPuzzleFactory;
  public DigitDialPuzzleFactory(Func<DigitDialPuzzle> factoryFunc)
  {
    _rotateDialPuzzleFactory = factoryFunc;
  }
  DigitDialPuzzle IFactory<DigitDialPuzzle>.GetProduct()
  {
    return _rotateDialPuzzleFactory?.Invoke();
  }
}
