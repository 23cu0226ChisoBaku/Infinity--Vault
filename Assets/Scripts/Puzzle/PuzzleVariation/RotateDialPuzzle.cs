using UnityEngine;
using UnityEngine.Assertions;
using System;


public sealed class RotateDialPuzzle : Puzzle, IDialPuzzle
{
  private GameObject _dialObj;                  // �񂷃I�u�W�F�N�g
  private RotateDialPuzzleModel _dialPuzzleInfo; // �_�C�������p�Y���f�[�^
  private float _rotateAngle;                   // �񂷓x��(���v���͐��A�����v���͕�)
  private int _rotateRoundCnt;                  // �񂷎���
  private float _targetAngle;                   // �䂪�����邽�߉񂷕K�v�̓x��

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

  // TODO �����������ۂ�
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

  // TODO �����������ۂ�
  void IPuzzle.HidePuzzle()
  {
      _dialObj.SetActive(false);
  }

  void IPuzzle.ResetPuzzle()
  {
      _rotateAngle = 0f;
  }

  // TODO �����������ۂ�
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