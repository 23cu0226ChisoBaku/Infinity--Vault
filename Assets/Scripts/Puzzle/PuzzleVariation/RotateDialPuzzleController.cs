using UnityEngine;
using UnityEngine.Assertions;
using System;
using System.Runtime.InteropServices;

internal interface ICanSetPuzzleDifficulty
{
  /// <summary>
  /// 難易度設定を行い、パズル(IPuzzle)を返す
  /// </summary>
  /// <param name="difficulty"></param>
  /// <returns></returns>
  IPuzzle AcceptDifficulty(EPuzzleDifficulty difficulty);
}
internal sealed class RotateDialPuzzleController : Puzzle
{
  private const float MAX_ROTATE_SPEED = 1000f;
  private RotateDialPuzzleModel _dialPuzzleInfo; // ダイヤル錠パズルデータ
  private float _rotateAngle;                   // 回す度数(時計回りは正、反時計回りは負)
  private int _rotateRoundCnt;                  // 回す周数
  private float _targetAngle;                   // 謎が解けるため回す必要の度数
  private bool _isDragging;
  private Vector2 _previousMousePos;
  private Vector2 _currentMousePos;
  private Camera _currentMainCamera;

  public override void HidePuzzle()
  {
    gameObject.SetActive(false);
    IsPuzzleActive = false;
  }

  public override void ResetPuzzle()
  {
      _rotateAngle = 0f;
  }
  public override void ShowPuzzle()
  {
    gameObject.SetActive(true);
    IsPuzzleActive = true;
  }  
  public override void UpdatePuzzle()
  {
    if (!IsPuzzleActive)
    {
      return;
    }

    UpdateMouseMove();

    float inputMoveAngle = Vector2.SignedAngle(_previousMousePos - (Vector2)transform.position, _currentMousePos - (Vector2)transform.position);
    var RotateSpeed = inputMoveAngle / 45f * MAX_ROTATE_SPEED;  
    
    transform.Rotate(0,0,RotateSpeed * Time.deltaTime);

    _rotateAngle += -RotateSpeed * Time.deltaTime;

    // Strategy
    if (_rotateAngle >= _targetAngle)
    {
        var adjustRotate = transform.rotation;
        adjustRotate.z = - _targetAngle % 360f;
        transform.rotation = adjustRotate;

        _onPuzzleClear?.Invoke();
    }
  }
  public void InitInfo(RotateDialPuzzleModel puzzleInfo)
  {
    _dialPuzzleInfo = puzzleInfo;

    _targetAngle = (int)_dialPuzzleInfo.RotateDial * _dialPuzzleInfo.Round * 360f; 
  }

  private void UpdateMouseMove()
  {
    if (_isDragging)
    {
      _previousMousePos = _currentMousePos;     
      _currentMousePos = _currentMainCamera.ScreenToWorldPoint(Input.mousePosition);
    }
  }

  private void Awake()
  {
    _isDragging = false;

    // IPuzzleGenerator generator = PuzzleGenerator.Instance;
    // _dialPuzzle = generator.GenerateDialPuzzle(EPuzzleDifficulty.Hard,EDialPuzzleType.Rotate);
    _currentMainCamera = Camera.main;
    OnPuzzleClear += () =>
    {
      IsPuzzleCleared = true;
      _isDragging = false;
      Destroy(gameObject,5f);
    };

    IsPuzzleActive = true;
  }

  private void OnMouseDrag() 
  {
    if(!IsPuzzleCleared)
    {
      _isDragging = true;
    }
  }

  private void OnMouseDown() 
  {
    _currentMousePos = _currentMainCamera.ScreenToWorldPoint(Input.mousePosition);
    _previousMousePos = _currentMousePos; 
  }
  private void OnMouseUp()
  {
    ExitDrag();
  }

  private void OnMouseExit() 
  {
    ExitDrag();
  }

  private void ExitDrag()
  {
    _isDragging = false;
    ResetMouseMove();
    Debug.Log("Exit Dragging");
  }

  private void ResetMouseMove()
  {
    _previousMousePos = Vector2.zero;
    _currentMousePos = Vector2.zero;
  }



  public override IPuzzle AcceptDifficulty(EPuzzleDifficulty difficulty)
  {
    return PuzzleDifficultySetter.GetDifficultySetter(difficulty).SetDifficulty(this);
  }
}