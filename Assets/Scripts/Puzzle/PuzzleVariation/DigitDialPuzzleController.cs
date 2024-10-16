using UnityEngine;
using System;

internal sealed class DigitDialPuzzleController : Puzzle
{
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
    
  }

  public override void ResetPuzzle()
  {
      _rotateAngle = 0f;
  }
  public override void ShowPuzzle()
  {
    
  }  
  public override void UpdatePuzzle()
  {
    UpdateMouseMove();

    float inputMoveAngle = Vector2.SignedAngle(_previousMousePos - (Vector2)transform.position, _currentMousePos - (Vector2)transform.position);
    var RotateSpeed = inputMoveAngle / 45f * 1000f;  
    
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
      _currentMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
  }

  private void Awake()
  {
    Application.targetFrameRate = 60;
    _isDragging = false;

    // IPuzzleGenerator generator = PuzzleGenerator.Instance;
    // _dialPuzzle = generator.GenerateDialPuzzle(EPuzzleDifficulty.Hard,EDialPuzzleType.Rotate);

    OnPuzzleClear += () =>
    {
      IsPuzzleCleared = true;
      _isDragging = false;
      Destroy(gameObject,5f);
    };
  }

  private void OnMouseDrag() 
  {
    if(!IsPuzzleCleared)
    {
      _isDragging = true;
      Debug.Log("Dragging");
    }
  }

  private void OnMouseDown() 
  {
    _currentMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    _previousMousePos = _currentMousePos; 
  }
  private void OnMouseUp()
  {
      _isDragging = false;
      Debug.Log("Exit Dragging");
  }

  public override IPuzzle AcceptDifficulty(EPuzzleDifficulty difficulty)
  {
    return PuzzleDifficultySetter.GetDifficultySetter(difficulty).SetDifficulty(this);
  }
}