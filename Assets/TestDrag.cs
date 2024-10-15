using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDrag : MonoBehaviour
{
  private IPuzzle monoPuzzle;
  private void Awake()
  {
    var rotate = Resources.Load<GameObject>("Prefabs/Puzzle/RotateDialPuzzle");
    var rotateObj = Instantiate(rotate,Vector3.zero, Quaternion.identity);

    if (rotateObj.TryGetComponent(out ICanSetPuzzleDifficulty set))
    {
        set.AcceptDifficulty(EPuzzleDifficulty.Hard);
    }

    monoPuzzle = rotateObj.GetComponent<IPuzzle>();
  }

  private void Update()
  {
    if (monoPuzzle.IsAlive())
    {
      monoPuzzle.UpdatePuzzle();
    }
  }
}
