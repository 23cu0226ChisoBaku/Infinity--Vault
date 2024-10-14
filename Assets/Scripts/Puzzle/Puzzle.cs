using System;
using System.Runtime.InteropServices;
using MEvent;
using UnityEngine;
using UnityEngine.Assertions;

public abstract class Puzzle
{
  protected DisposableEvent _onPuzzleClear = new DisposableEvent();
  ~Puzzle()
  {
    UnityEngine.Debug.LogWarning("HogeHoge");
    _onPuzzleClear.Dispose();
  }
}

[Serializable]
public struct RotateDialPuzzle
{
  public enum ERotateDial
  {
    Clockwise = 1,          // 時計回り
    CounterClockwise = -1,  // 反時計回り
  }

  // ダイヤルを回す方向
  [SerializeField]
  public ERotateDial RotateDial;
  // 回す回数（周）
  [SerializeField]
  public int Round;
}

public sealed class DialPuzzle : Puzzle, IDialPuzzle
{
  private GameObject _dialObj;                // 回すオブジェクト
  private RotateDialPuzzle _dialPuzzleInfo;   // ダイヤル錠パズルデータ
  private float _rotateAngle;                 // 回す度数(時計回りは正、反時計回りは負)
  private int _rotateRoundCnt;                // 回す周数
  private float _targetAngle;                 // 謎が解けるため回す必要の度数

  public DialPuzzle(GameObject gameObject, RotateDialPuzzle puzzleInfo)
  {
    Assert.IsNotNull(gameObject,"Dial Puzzle Obj is null");

    _dialObj = gameObject;
    _dialPuzzleInfo = puzzleInfo;
    _rotateAngle = 0f;

    _targetAngle = (int)_dialPuzzleInfo.RotateDial * _dialPuzzleInfo.Round * 360f; 
  }
  event Action IPuzzle.OnPuzzleClear
  {
    add
    {
      _onPuzzleClear.Subscribe(value);
    }
    remove
    {
      _onPuzzleClear.Unsubscribe(value);
    }
  }
  void IPuzzle.HidePuzzle()
  {
      _dialObj.SetActive(false);
  }

  void IPuzzle.ResetPuzzle()
  {
      _rotateAngle = 0f;
  }

  void IPuzzle.ShowPuzzle()
  {
      _dialObj.SetActive(true);
  }

  void IDialPuzzle.UpdateDial(Vector2 startPos, Vector2 endPos)
  {
      float inputMoveAngle = Vector2.SignedAngle(startPos - (Vector2)_dialObj.transform.position, endPos - (Vector2)_dialObj.transform.position);
      var RotateSpeed = inputMoveAngle / 45f * 1000f;  
      
      _dialObj.transform.Rotate(0,0,RotateSpeed * Time.deltaTime);

      _rotateAngle += -RotateSpeed * Time.deltaTime;

      // Strategy
      if (_rotateAngle >= _targetAngle)
      {
          var adjustRotate = _dialObj.transform.rotation;
          adjustRotate.z = - _targetAngle % 360f;
          _dialObj.transform.rotation = adjustRotate;

          _onPuzzleClear?.Invoke();
      }
  }
}