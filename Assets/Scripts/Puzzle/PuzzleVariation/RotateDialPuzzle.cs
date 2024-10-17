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
internal sealed class RotateDialPuzzle : Puzzle
{
  private const float MAX_ROTATE_SPEED = 270f;
  private const float MAX_INPUT_ANGLE = 120f;

  private RotateDialPuzzleModel _dialPuzzleInfo; // ダイヤル錠(回す)パズルデータ
  private float _totalRotateAngle;                   // 回した角度の度数(Degree)(時計回りは正、反時計回りは負)
  private int _rotateRoundCnt;                  // 何周回したカウンター
  private float _targetAngle;                   // 謎を解くため回す必要がある度数(Degree)
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
    _totalRotateAngle = 0f;
    gameObject.transform.rotation = Quaternion.identity;
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

    float maxInputAngleCurrentFrame = MAX_INPUT_ANGLE * Time.deltaTime;
    inputMoveAngle = Mathf.Clamp(inputMoveAngle,-maxInputAngleCurrentFrame,maxInputAngleCurrentFrame);

    var rotateAngle = inputMoveAngle / maxInputAngleCurrentFrame * MAX_ROTATE_SPEED * Time.deltaTime; 
    transform.Rotate(0,0,rotateAngle);

    _totalRotateAngle += -rotateAngle;

    Debug.Log(_totalRotateAngle);
    // Strategy
    if (_totalRotateAngle >= _targetAngle)
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
    transform.localPosition = Vector3.zero;
    _isDragging = false;

    _currentMainCamera = Camera.main;
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