using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO 循環参照になる可能性が？(gameObject <=> IDialPuzzle)
public class TestDrag : MonoBehaviour
{
  public Vector2 PreviousMousePos;
  public Vector2 CurrentMousePos;
  public bool IsDragging;
  public bool WaitDestroy;
  private IDialPuzzle _dialPuzzle;
  // Start is called before the first frame update
  void Start()
  {
    Application.targetFrameRate = 60;
    WaitDestroy= false;

    IPuzzleGenerator generator = PuzzleGenerator.Instance;
    _dialPuzzle = generator.GenerateDialPuzzle(EPuzzleDifficulty.Hard,EDialPuzzleType.Rotate);

    _dialPuzzle.InitTargetGameObject(gameObject);
    _dialPuzzle.OnPuzzleClear += () =>
    {
      WaitDestroy = true;
      IsDragging = false;
      Destroy(gameObject,5f);
      _dialPuzzle = null;
    };
  }

  // Update is called once per frame
  void Update()
  {
    if (IsDragging)
    {
      PreviousMousePos = CurrentMousePos;     
      CurrentMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
      
      _dialPuzzle?.UpdateDial(PreviousMousePos,CurrentMousePos);
    }

  }

  private void OnMouseDrag() 
  {
    if(!WaitDestroy)
    {
      IsDragging = true;
      Debug.Log("Dragging");
    }
  }

  private void OnMouseDown() 
  {
    CurrentMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    PreviousMousePos = CurrentMousePos; 
  }
  private void OnMouseUp()
  {
    if (!WaitDestroy)
    {
      IsDragging = false;
      Debug.Log("Exit Dragging");
    }
  }

  internal void InitPuzzle(IDialPuzzle dialPuzzle)
  {
    _dialPuzzle = dialPuzzle;
  }

}
