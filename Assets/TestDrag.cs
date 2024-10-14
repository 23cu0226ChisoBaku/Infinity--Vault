using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO 循環参照になる可能性が？？？
public class TestDrag : MonoBehaviour
{
  public Vector2 PreviousMousePos;
  public Vector2 CurrentMousePos;
  public bool IsDragging;
  public bool WaitDestroy;

  public RotateDialPuzzle puzzleInfo;

  private IDialPuzzle dialPuzzle;
  // Start is called before the first frame update
  void Start()
  {
    Application.targetFrameRate = 60;
    WaitDestroy= false;

    dialPuzzle = new DialPuzzle(gameObject,puzzleInfo);
    dialPuzzle.OnPuzzleClear += () =>
    {
      WaitDestroy = true;
      IsDragging = false;
      Destroy(gameObject,5f);
      dialPuzzle = null;
    };
  }

  // Update is called once per frame
  void Update()
  {
    if (IsDragging)
    {
      PreviousMousePos = CurrentMousePos;     
      CurrentMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
      
      dialPuzzle?.UpdateDial(PreviousMousePos,CurrentMousePos);
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
  private void OnMouseUp()
  {
    if (!WaitDestroy)
    {
      IsDragging = false;
      Debug.Log("Exit Dragging");
    }
  }
}
