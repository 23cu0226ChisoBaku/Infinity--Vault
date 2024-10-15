using System;
using MDesingPattern.MFactory;
using UnityEngine;

public class RotateDialPuzzleFactory : IFactory<GameObject>
{
  private event Func<GameObject> _rotateDialPuzzleFactory;
  public RotateDialPuzzleFactory(Func<GameObject> factoryFunc)
  {
    _rotateDialPuzzleFactory = factoryFunc;
  }
  GameObject IFactory<GameObject>.GetProduct()
  {
    return _rotateDialPuzzleFactory?.Invoke();
  }
}