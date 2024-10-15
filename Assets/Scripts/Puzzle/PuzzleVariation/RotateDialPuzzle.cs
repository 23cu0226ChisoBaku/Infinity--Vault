using UnityEngine;
using UnityEngine.Assertions;
using System;


public sealed class RotateDialPuzzle : Puzzle, IDialPuzzle
{
  private GameObject _dialObj;                  // 回すオブジェクト
  private RotateDialPuzzleModel _dialPuzzleInfo; // ダイヤル錠パズルデータ
  private float _rotateAngle;                   // 回す度数(時計回りは正、反時計回りは負)
  private int _rotateRoundCnt;                  // 回す周数
  private float _targetAngle;                   // 謎が解けるため回す必要の度数

  public RotateDialPuzzle(RotateDialPuzzleModel puzzleInfo)
    :this()
  {

    _dialPuzzleInfo = puzzleInfo;


    _targetAngle = (int)_dialPuzzleInfo.RotateDial * _dialPuzzleInfo.Round * 360f; 
  }

  public RotateDialPuzzle()
  {
    _rotateAngle = 0f;
  }

  // TODO 同じ処理っぽい
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

    public void InitTargetGameObject(GameObject targetGameObject)
    {
      Assert.IsNotNull(targetGameObject,"Dial Puzzle Obj is null");

      _dialObj = targetGameObject;
    }

  // TODO 同じ処理っぽい
  void IPuzzle.HidePuzzle()
  {
      _dialObj.SetActive(false);
  }

  void IPuzzle.ResetPuzzle()
  {
      _rotateAngle = 0f;
  }

  // TODO 同じ処理っぽい
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

  public void InitInfo(RotateDialPuzzleModel puzzleInfo)
  {
    _dialPuzzleInfo = puzzleInfo;

    _targetAngle = (int)_dialPuzzleInfo.RotateDial * _dialPuzzleInfo.Round * 360f; 
  }
}