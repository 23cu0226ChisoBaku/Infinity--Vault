using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestButtonPuzzle : MonoBehaviour
{
  private IPuzzle buttonPuzzle;
  private void Awake() 
  {
    IPuzzleGenerator puzzleGenerator = PuzzleGenerator.Instance;

    buttonPuzzle = puzzleGenerator.GeneratePuzzle(EPuzzleDifficulty.Hard,EButtonPuzzleType.Sequence);

    buttonPuzzle.ShowPuzzle();

    buttonPuzzle.OnPuzzleClear += () => { Destroy(buttonPuzzle.GameObject,5f);};
  }
}
