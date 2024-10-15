using System;
using MDesingPattern.MFactory;
using UnityEngine;

public class DigitDialPuzzleFactory : IFactory<GameObject>
{
  private event Func<GameObject> _rotateDialPuzzleFactory;
  public DigitDialPuzzleFactory(Func<GameObject> factoryFunc)
  {
    _rotateDialPuzzleFactory = factoryFunc;
  }
  GameObject IFactory<GameObject>.GetProduct()
  {
    return _rotateDialPuzzleFactory?.Invoke();
  }
}
