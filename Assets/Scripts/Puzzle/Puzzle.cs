using MEvent;
/// <summary>
/// �p�Y���̐e�N���X
/// </summary>
public abstract class Puzzle
{
  protected DisposableEvent _onPuzzleClear = new DisposableEvent();
  ~Puzzle()
  {
    UnityEngine.Debug.LogWarning("HogeHoge");
    _onPuzzleClear.Dispose();
  }
}
