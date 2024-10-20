using System.Runtime.InteropServices;
using UnityEngine;

/// <summary>
/// 回転式ダイヤル錠パズル
/// </summary>
internal sealed class RotateDialPuzzle : Puzzle
{
// 定数フィールド
#region Constant Field
  private const float MAX_ROTATE_SPEED = 270f;
  private const float MAX_INPUT_ANGLE = 120f;
#endregion Constant Field
// End of 定数フィールド

// プライベートフィールド
#region Private Field
  // ダイヤル錠(回す)パズルデータ
  private RotateDialPuzzleInfo _dialPuzzleInfo; 
  // 回した角度の度数(Degree)(時計回りは正、反時計回りは負)
  private float _totalRotateAngle;  
  // 何周回したカウンター                 
  private int _rotateRoundCnt;  
  // 謎を解くため回す必要がある度数(Degree)                
  private float _clearTargetAngle;  
  // マウスでドラッグしているか                 
  private bool _isDragging;
  // 前フレームのマウス座標(Unity ワールド座標)
  private Vector2 _previousMousePos;
  // 現在フレームのマウス座標(Unity ワールド座標)
  private Vector2 _currentMousePos;
  // メインカメラ
  private Camera _currentMainCamera;
  private IPuzzlePanel _puzzlePanel;
#endregion Private Field
// End of プライベートフィールド

// インターフェース
#region Interface
// IPuzzleインターフェース
#region IPuzzle
  public override void HidePuzzle()
  {
    if (_puzzlePanel.IsAlive())
    {
      _puzzlePanel.HidePanel();
    }

    gameObject.SetActive(false);
    IsPuzzleActive = false;

    // TODO
    if (_puzzleText.IsAlive())
    {
      _puzzleText.enabled = false;
      _puzzleExitHint.enabled = false;
    }
  }

  public override void ResetPuzzle()
  {
    _totalRotateAngle = 0f;
    gameObject.transform.rotation = Quaternion.identity;
  }
  public override void ShowPuzzle()
  {
    if (_puzzlePanel.IsAlive())
    {
      _puzzlePanel.ShowPanel();
    }
    gameObject.SetActive(true);
    IsPuzzleActive = true;

    // TODO
    if (_puzzleText.IsAlive())
    {
      _puzzleText.enabled = true;
      _puzzleExitHint.enabled = true;
    }
  }  
  public override void UpdatePuzzle()
  {
    if (!IsPuzzleActive)
    {
      return;
    }

    // マウス座標を更新する
    UpdateMouseMove();

    // マウスの座標で移動する角度を計算(正だったら反時計回り、負だったら時計回り)
    float inputMoveAngle = Vector2.SignedAngle(_previousMousePos - (Vector2)transform.position, _currentMousePos - (Vector2)transform.position);

    float maxInputAngleCurrentFrame = MAX_INPUT_ANGLE * Time.unscaledDeltaTime;
    inputMoveAngle = Mathf.Clamp(inputMoveAngle,-maxInputAngleCurrentFrame,maxInputAngleCurrentFrame);

    var rotateAngle = inputMoveAngle / maxInputAngleCurrentFrame * MAX_ROTATE_SPEED * Time.unscaledDeltaTime; 
    transform.Rotate(0,0,rotateAngle);

    _totalRotateAngle += -rotateAngle;

    // Strategy
    if (_totalRotateAngle >= _clearTargetAngle)
    {
        var adjustRotate = transform.rotation;
        adjustRotate.z = - _clearTargetAngle % 360f;
        transform.rotation = adjustRotate;

        _onPuzzleClear?.Invoke();
    }
    _puzzleText.text = $"時計回り{_dialPuzzleInfo.Round - (int)(_totalRotateAngle / 360f)}周";
  }
#endregion IPuzzle
// End of IPuzzleインターフェース
  public void InitInfo(RotateDialPuzzleInfo puzzleInfo)
  {
    _dialPuzzleInfo = puzzleInfo;

    _clearTargetAngle = (int)_dialPuzzleInfo.RotateDial * _dialPuzzleInfo.Round * 360f; 

    if (_puzzlePanel == null)
    {
      IPuzzleGenerator puzzleGenerator = PuzzleGenerator.Instance;
      _puzzlePanel = puzzleGenerator.GetPanel();
    }

    // TODO
    _puzzleText = GameObject.Find("PuzzleHint").GetComponent<TMPro.TMP_Text>();
    _puzzleText.text = $"時計回り{_dialPuzzleInfo.Round}周";

    _puzzleExitHint = GameObject.Find("PuzzleExitUI").GetComponent<TMPro.TMP_Text>();
  }

  public override IPuzzle AcceptDifficulty(EPuzzleDifficulty difficulty)
  {
    return PuzzleDifficultySetter.GetDifficultySetter(difficulty).SetDifficulty(this);
  }
#endregion Interface
// End of Interface


  /// <summary>
  /// マウス座標更新内部実装
  /// </summary>
  private void UpdateMouseMove()
  {
    if (_isDragging)
    {
      _previousMousePos = _currentMousePos;     
      _currentMousePos = _currentMainCamera.ScreenToWorldPoint(Input.mousePosition);
    }
  }
// Unityメインループメッセージ
#region Unity Main Loop Message
  private void Awake()
  {
    transform.localPosition = Vector3.zero;
    _isDragging = false;

    _currentMainCamera = Camera.main;
    ((IPuzzle)this).OnPuzzleClear += () =>
    {
      IsPuzzleCleared = true;
      _isDragging = false;
      Destroy(gameObject);
    };
  }
#endregion Unity Main Loop Message
// End of Unityメインループメッセージ

#region Unity Mouse Input Message
#endregion
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

  protected override void OnDestroy()
  {
    base.OnDestroy();
    if(_puzzlePanel.IsAlive())
    {
      _puzzlePanel.DisposePanel();
    }
  }

}